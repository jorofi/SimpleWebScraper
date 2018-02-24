using System;
using System.Collections.Generic;
using System.Text;

namespace DownloadAirInfo.Services.Helpers
{
    public static class StringExtensions
    {
        public static string HtmlNormalization(this String str)
        {
            return str
                .Replace("\n", string.Empty)
                .Replace("&nbsp;", " ")
                .Replace("&deg;", "°")
                .Replace("		", " ")
                .Replace("		", " ")
                .Trim();
        }
    }
}
