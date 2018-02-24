using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SimpleWebScraper.Website.Models;
using SimpleWebScraper.Services;
using ScrapersModels = SimpleWebScraper.Models.Scrapers;

namespace SimpleWebScraper.Website.Controllers
{
    public class ScrapersController : Controller
    {
        public IScraperService _luftdatenScraperService { get; set; }

        public IScraperService _weatherUndergroundScraperService { get; set; }

        public ScrapersController(LuftdatenScraperService luftdatenScraperService, WeatherUndergroundScraperService weatherUndergroundScraperService)
        {
            _luftdatenScraperService = luftdatenScraperService;
            _weatherUndergroundScraperService = weatherUndergroundScraperService;
        }

        [HttpGet]
        public IActionResult Luftdaten()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Luftdaten(ScrapersModels.ScraperConfiguration configurationModel)
        {
            if (ModelState.IsValid)
            {
                await _luftdatenScraperService.StartScrapingAsync(configurationModel);
            }

            return View();
        }

        [HttpGet]
        public IActionResult WeatherUnderground()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> WeatherUnderground(ScrapersModels.ScraperConfiguration configurationModel)
        {
            if (ModelState.IsValid)
            {
                await _weatherUndergroundScraperService.StartScrapingAsync(configurationModel);
            }

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
