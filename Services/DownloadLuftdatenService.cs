using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using DownloadAirInfo.Models.Download;
using DownloadAirInfo.Services.Helpers;
using HtmlAgilityPack;

namespace DownloadAirInfo.Services
{
    public class DownloadLuftdatenService : IDownloadService
    {
        public async Task DownloadAsync(Configuration configuration)
        {
            if(!Directory.Exists(configuration.ArchiveDirectory))
            {
                Directory.CreateDirectory(configuration.ArchiveDirectory);
            }

            using (WebClient client = new WebClient())
            {
                var endpointUri = new Uri(configuration.Endpoint);

                var endpointHtml = await client.DownloadStringTaskAsync(endpointUri);

                HtmlDocument document = new HtmlDocument();

                document.LoadHtml(endpointHtml);

                var links = document.DocumentNode.SelectNodes("//a");
                foreach (DateTime day in DateHelper.EachDay(configuration.StartDate, configuration.EndDate))
                {
                    var link = document.DocumentNode.SelectSingleNode("//a[starts-with(@href, '" + day.ToString("yyyy-MM-dd") + "')]");
                    if (link != null)
                    {
                        var sensorsUri = new Uri(endpointUri, link.GetAttributeValue("href", string.Empty));
                        var sensorsHtml = await client.DownloadStringTaskAsync(sensorsUri);

                        HtmlDocument sensorsPage = new HtmlDocument();
                        sensorsPage.LoadHtml(sensorsHtml);

                        var sensorsFileLink = sensorsPage.DocumentNode.SelectNodes("//a[starts-with(@href, '" + day.ToString("yyyy-MM-dd") + "')]");
                        foreach(var sensorFileLink in sensorsFileLink)
                        {
                            var fileName = sensorFileLink.GetAttributeValue("href", string.Empty);
                            var sensorFileUri = new Uri(sensorsUri, fileName);
                            await client.DownloadFileTaskAsync(sensorFileUri, Path.Combine(configuration.ArchiveDirectory, fileName));

                            Thread.Sleep(configuration.SleepTime);
                        }
                    }

                    Thread.Sleep(configuration.SleepTime);
                }
            }
        }
    }
}
