using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ParkyWeb.Models;
using ParkyWeb.Repository.IRepository;
namespace ParkyWeb.Controllers
{
    //[Route("[controller]")]
    public class NationalParksController : Controller
    {
        private readonly ILogger<NationalParksController> _logger;
        private readonly INationalParkRepository _parkRepository;

        public NationalParksController(ILogger<NationalParksController> logger, INationalParkRepository parkyRepository)
        {
            _parkRepository = parkyRepository;
            _logger = logger;
        }


        public IActionResult Index()
        {
            return View(new NationalPark() { });
        }

        public async Task<IActionResult> GetAllNationalParks()
        {
            return Json(await _parkRepository.GetAllAsync(SD.NationalParkAPIPath));
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}