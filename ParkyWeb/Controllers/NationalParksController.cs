using Microsoft.AspNetCore.Mvc;
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

        public async Task<IActionResult> Upsert(int? id)
        {
            var obj = new NationalPark();
            if (id == null) return View(obj); //

            // Follow will come here for update
            var token = HttpContext.Session.GetString("JWToken") ?? "";
            obj = await _parkRepository.GetAsync(SD.NationalParkAPIPath, id.GetValueOrDefault(), token); // get data from DB based on Id
            if (obj == null) return NotFound();
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(NationalPark obj)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                var token = HttpContext.Session.GetString("JWToken") ?? "";
                if (files.Count > 0)
                {
                    byte[]? p1 = null;
                    using (var fs1 = files[0].OpenReadStream())
                    {
                        using (var ms1 = new MemoryStream())
                        {
                            fs1.CopyTo(ms1);
                            p1 = ms1.ToArray();
                        }
                    }
                    obj.Picture = p1;
                }
                else
                {

                    var objFromDb = await _parkRepository.GetAsync(SD.NationalParkAPIPath, obj.Id, token);
                    obj.Picture = objFromDb?.Picture;
                }
                if (obj.Id == 0)
                {
                    await _parkRepository.CreateAsync(SD.NationalParkAPIPath, obj, token);
                }
                else
                {
                    await _parkRepository.UpdateAsync(SD.NationalParkAPIPath + obj.Id, obj, token);
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
            var token = HttpContext.Session.GetString("JWToken") ?? "";
            var status = await _parkRepository.DeleteAsync(SD.NationalParkAPIPath, id, token);
            if (status)
            {
                return Json(new { success = true, message = "Delete Successful" });
            }
            return Json(new { success = false, message = "Delete Not Successful" });
        }

        public async Task<IActionResult> GetAllNationalPark()
        {
            var token = HttpContext.Session.GetString("JWToken") ?? "";
            var result = await _parkRepository.GetAllAsync(SD.NationalParkAPIPath, token);
            return Json(new { data = result });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}