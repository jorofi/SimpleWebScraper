using System;

namespace DownloadAirInfo.Models.Download
{
    public class Configuration
    {
        public string Endpoint { get; set; }

        public string ArchiveDirectory { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int SleepTime { get; set; }
    }
}
