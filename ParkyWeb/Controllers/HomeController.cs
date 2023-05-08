using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using ParkyWeb.Models;
using ParkyWeb.Models.ViewModel;
using ParkyWeb.Repository.IRepository;
using System.Diagnostics;
using System.Security.Claims;

namespace ParkyWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly INationalParkRepository _nationalParkRepository;
        private readonly ITrailRepository _trailRepository;
        private readonly IAccountRepository _accountRepository;

        public HomeController(ILogger<HomeController> logger, ITrailRepository trailRepository, INationalParkRepository nationalParkRepository, IAccountRepository accountRepository)
        {
            _logger = logger;
            _trailRepository = trailRepository;
            _nationalParkRepository = nationalParkRepository;
            _accountRepository = accountRepository;
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
            var token = HttpContext.Session.GetString("JWToken") ?? "";
            var listOfParkAndTrails = new IndexVM
            {
                NationalParkList = await _nationalParkRepository.GetAllAsync(SD.NationalParkAPIPath, token),
                TrailList = await _trailRepository.GetAllAsync(SD.TrailAPIPath, token),
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

        [HttpGet]
        public IActionResult Login()
        {
            User obj = new User();// we don't need do this ,we can just tell the Razor view the Model Type
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(User obj)
        {
            var objUser = await _accountRepository.LoginAsync(SD.AccountAPIPath + "authenticate/", obj);
            if (objUser?.Token == null) return View();
            // authorized User
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.Name, obj.UserName!));
            identity.AddClaim(new Claim(ClaimTypes.Role, obj.Role!));
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            // These are for API calls
            HttpContext.Session.SetString("JWToken", objUser.Token);
            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User obj)
        {
            var result = await _accountRepository.RegisterAsync(SD.AccountAPIPath + "register/", obj);
            if (result == false) return View();
            return RedirectToAction("Login");
        }
        public async Task<IActionResult> LogoutAsync()
        {
            await HttpContext.SignOutAsync();

            HttpContext.Session.SetString("JWToken", "");
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}