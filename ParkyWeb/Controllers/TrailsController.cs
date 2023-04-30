using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ParkyWeb.Models;
using ParkyWeb.Models.ViewModel;
using ParkyWeb.Repository.IRepository;
namespace ParkyWeb.Controllers
{
    //[Route("[controller]")]
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
        public async Task<IActionResult> Upsert(int? id)
        {
            IEnumerable<NationalPark>? npList = await _nationalParkRepository.GetAllAsync(SD.NationalParkAPIPath);
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
            objVM.Trail = await _trailRepository.GetAsync(SD.TrailAPIPath, id.GetValueOrDefault()); // get data from DB based on Id
            if (objVM.Trail == null) return NotFound();
            return View(objVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(TrailsVM? obj)
        {
            if (ModelState.IsValid)
            {
                if (obj?.Trail?.Id == 0)
                {
                    await _trailRepository.CreateAsync(SD.TrailAPIPath, obj.Trail);
                }
                else
                {
                    await _trailRepository.UpdateAsync(SD.TrailAPIPath + obj?.Trail?.Id, obj?.Trail!);
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(obj);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _trailRepository.DeleteAsync(SD.TrailAPIPath, id);
            if (status)
            {
                return Json(new { success = true, message = "Delete Successful" });
            }
            return Json(new { success = false, message = "Delete Not Successful" });
        }
        public async Task<IActionResult> GetAllTrails()
        {
            var result = await _trailRepository.GetAllAsync(SD.TrailAPIPath);
            return Json(new { data = result });
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}