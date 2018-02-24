using System.Threading.Tasks;
using SimpleWebScraper.Models.Scrapers;

namespace SimpleWebScraper.Services
{
    public interface IScraperService
    {
        Task StartScrapingAsync(ScraperConfiguration configuration);
    }
}
