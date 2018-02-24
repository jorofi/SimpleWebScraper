using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DownloadAirInfo.Models.Download;
using DownloadAirInfo.Services.Helpers;
using HtmlAgilityPack;

namespace DownloadAirInfo.Services
{
    public class DownloadWeatherUndergroundService : IDownloadService
    {
        public async Task DownloadAsync(Configuration configuration)
        {
            if (!Directory.Exists(configuration.ArchiveDirectory))
            {
                Directory.CreateDirectory(configuration.ArchiveDirectory);
            }

            using (WebClient client = new WebClient())
            {
                var csvFile = new StringBuilder();

                foreach (DateTime day in DateHelper.EachDay(configuration.StartDate, configuration.EndDate))
                {
                    var currentDayEndpointUri = configuration.Endpoint.Replace("{DATE}", day.ToString("yyyy/MM/dd"));
                    var currentDayWeatherPageHtml = await client.DownloadStringTaskAsync(currentDayEndpointUri);

                    HtmlDocument currentDayWeatherPage = new HtmlDocument();

                    currentDayWeatherPage.LoadHtml(currentDayWeatherPageHtml);

                    var weatherRows = currentDayWeatherPage.DocumentNode.SelectNodes("//table[starts-with(@id, 'obsTable')]//tr");

                    if(weatherRows.Count > 0 && day == configuration.StartDate)
                    {
                        var headerCells = weatherRows[0].SelectNodes("th");
                        if (headerCells != null)
                        {
                            var csvRow = GetCsvRow(headerCells, day, false);
                            csvFile.AppendLine(csvRow);
                        }
                    }

                    foreach (var row in weatherRows)
                    {
                        var cells = row.SelectNodes("td");
                        if (cells != null)
                        {
                            var csvRow = GetCsvRow(cells, day, true);
                            csvFile.AppendLine(csvRow);
                        }
                    }

                    Thread.Sleep(configuration.SleepTime);
                }

                String timeStamp = DateTime.UtcNow.ToString("yyyyMMddHHmmssffff");
                var csvFileName = $"{timeStamp}-CsvWeatherDate.csv";
                var csvFullName = Path.Combine(configuration.ArchiveDirectory, csvFileName);

                File.WriteAllText(csvFullName, csvFile.ToString());
            }
        }

        private string GetCsvRow(HtmlNodeCollection cells, DateTime currentDay, bool skipFirstCell)
        {
            if (cells == null)
            {
                return null;
            }

            var newLine = new StringBuilder();
            for (var i = 0; i < cells.Count; i++)
            {
                var cell = cells[i];
                if(i == 0 && skipFirstCell)
                {
                    var dateTime = DateTime.Parse(string.Concat(currentDay.ToShortDateString(), " ", cell.InnerText));
                    newLine.Append(dateTime.ToString("yyyy-MM-dd HH:mm:ss"));
                    newLine.Append(",");
                }
                else
                {
                    newLine.Append(cell.InnerText.HtmlNormalization());
                    newLine.Append(",");
                }
            }

            return newLine.ToString().TrimEnd(',');
        }
    }
}
