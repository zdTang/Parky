using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ParkyWeb.Models;
using ParkyWeb.Models.ViewModel;
using ParkyWeb.Repository.IRepository;

namespace ParkyWeb.Controllers
{
    [Authorize]
    public class TrailsController : Controller
    {
        private readonly ILogger<TrailsController> _logger;
        private readonly ITrailRepository _trailRepository;
        private readonly INationalParkRepository _nationalParkRepository;

        public TrailsController(ILogger<TrailsController> logger, ITrailRepository trailRepository, INationalParkRepository nationalParkRepository)
        {
            _trailRepository = trailRepository;
            _logger = logger;
            _nationalParkRepository = nationalParkRepository;
        }

        public IActionResult Index()
        {
            return View(new Trail() { });
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Upsert(int? id)
        {
            var token = HttpContext.Session.GetString("JWToken") ?? "";
            IEnumerable<NationalPark>? npList = await _nationalParkRepository.GetAllAsync(SD.NationalParkAPIPath, token);
            TrailsVM objVM = new TrailsVM()
            {
                NationalParkList = npList?.Select(I => new SelectListItem
                {
                    Text = I.Name,
                    Value = I.Id.ToString()
                }),
                Trail = new Trail()
            };

            if (id == null) return View(objVM); //

            // Follow will come here for update
            objVM.Trail = await _trailRepository.GetAsync(SD.TrailAPIPath, id.GetValueOrDefault(), token); // get data from DB based on Id
            if (objVM.Trail == null) return NotFound();
            return View(objVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(TrailsVM? obj)
        {
            var token = HttpContext.Session.GetString("JWToken") ?? "";
            if (ModelState.IsValid)
            {
                if (obj?.Trail?.Id == 0)
                {
                    await _trailRepository.CreateAsync(SD.TrailAPIPath, obj.Trail, token);
                }
                else
                {
                    await _trailRepository.UpdateAsync(SD.TrailAPIPath + obj?.Trail?.Id, obj?.Trail!, token);
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                IEnumerable<NationalPark>? npList = await _nationalParkRepository.GetAllAsync(SD.NationalParkAPIPath, token);

                obj.NationalParkList = npList?.Select(I => new SelectListItem
                {
                    Text = I.Name,
                    Value = I.Id.ToString()
                });

                return View(obj);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var token = HttpContext.Session.GetString("JWToken") ?? "";
            var status = await _trailRepository.DeleteAsync(SD.TrailAPIPath, id, token);
            if (status)
            {
                return Json(new { success = true, message = "Delete Successful" });
            }
            return Json(new { success = false, message = "Delete Not Successful" });
        }

        public async Task<IActionResult> GetAllTrails()
        {
            var token = HttpContext.Session.GetString("JWToken") ?? "";
            var result = await _trailRepository.GetAllAsync(SD.TrailAPIPath, token);
            return Json(new { data = result });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}