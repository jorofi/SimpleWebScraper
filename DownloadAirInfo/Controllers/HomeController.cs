using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DownloadAirInfo.Website.Models;
using DownloadAirInfo.Services;
using DownloadModels = DownloadAirInfo.Models.Download;

namespace DownloadAirInfo.Website.Controllers
{
    public class HomeController : Controller
    {
        public IDownloadService _downloadService { get; set; }

        public HomeController(IDownloadService downloadService)
        {
            _downloadService = downloadService;
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
                await _downloadService.DownloadAsync(configurationModel);
            }

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
