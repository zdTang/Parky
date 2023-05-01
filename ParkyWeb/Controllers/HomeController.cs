using Microsoft.AspNetCore.Mvc;
using ParkyWeb.Models;
using ParkyWeb.Models.ViewModel;
using ParkyWeb.Repository;
using ParkyWeb.Repository.IRepository;
using System.Diagnostics;

namespace ParkyWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly INationalParkRepository _nationalParkRepository;
        private readonly ITrailRepository _trailRepository;

        public HomeController(ILogger<HomeController> logger, ITrailRepository trailRepository, INationalParkRepository nationalParkRepository)
        {
            _logger = logger;
            _trailRepository = trailRepository;
            _nationalParkRepository = nationalParkRepository;
        }

        //public IActionResult Index()
        //{
        //    var trail = _trailRepository.GetAllAsync(SD.TrailAPIPath);
        //    var nationalPark = _nationalParkRepository.GetAllAsync(SD.NationalParkAPIPath);
        //    IndexVM listOfParkAndTrails = new IndexVM
        //    {
        //        NationalPark = nationalPark.Result,
        //        Trails = trail.Result,
        //    };
        //    return View(listOfParkAndTrails);
        //}

        // Tutor's approach
        public async Task<IActionResult> Index()
        {
            var listOfParkAndTrails = new IndexVM
            {
                NationalPark = await _nationalParkRepository.GetAllAsync(SD.NationalParkAPIPath),
                Trails = await _trailRepository.GetAllAsync(SD.TrailAPIPath),
            };
            return View(listOfParkAndTrails);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}