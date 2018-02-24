using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DownloadAirInfo.Website.Models;
using DownloadAirInfo.Services;
using DownloadModels = DownloadAirInfo.Models.Download;

namespace DownloadAirInfo.Website.Controllers
{
    public class HomeController : Controller
    {
        public IDownloadService _downloadLuftdatenService { get; set; }

        public IDownloadService _downloadWeatherUndergroundService { get; set; }

        public HomeController(DownloadLuftdatenService downloadLuftdatenService, DownloadWeatherUndergroundService downloadWeatherUndergroundService)
        {
            _downloadLuftdatenService = downloadLuftdatenService;
            _downloadWeatherUndergroundService = downloadWeatherUndergroundService;
        }

        [HttpGet]
        public IActionResult DownloadLuftdaten()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DownloadLuftdaten(DownloadModels.Configuration configurationModel)
        {
            if (ModelState.IsValid)
            {
                await _downloadLuftdatenService.DownloadAsync(configurationModel);
            }

            return View();
        }

        [HttpGet]
        public IActionResult DownloadWeatherUnderground()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DownloadWeatherUnderground(DownloadModels.Configuration configurationModel)
        {
            if (ModelState.IsValid)
            {
                await _downloadWeatherUndergroundService.DownloadAsync(configurationModel);
            }

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
