using System;

namespace SimpleWebScraper.Services.Helpers
{
    public static class StringExtensions
    {
        public static string NormalizeWeatherUnderground(this String str)
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
