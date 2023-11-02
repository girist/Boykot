using Boykot.WebApp.Models;
using Boykot.WebApp.Models.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.IO;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Boykot.WebApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BoykotDbContext _boykotDbContext;

        public AdminController(ILogger<HomeController> logger, BoykotDbContext boykotDbContext = null)
        {
            _logger = logger;
            _boykotDbContext = boykotDbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CreateOrUpdate(int id)
        {
            //id ile akalaı kayıt döndürülecek
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrUpdate(IFormFile file, CreateUrunModel model)
        {
            if (model is null)
                return null;

            try
            {
                var urun = new Urun()
                {
                    Aciklama = model.Aciklama,
                    Adi = model.Adi,
                    Aktifmi = model.Aktifmi,
                    Barkod = model.Barkod,
                    Firma = model.Firma,
                    Kodu = model.Kodu,
                    Marka = model.Marka,
                    Not1 = model.Not1,
                    Not2 = model.Not2,
                    Ulke = model.Ulke,
                    Resim = $"/images/{ImageUpload(file).Result ?? "/images/no_product.png"}" 
                };

                await _boykotDbContext.Uruns.AddAsync(urun);
                await _boykotDbContext.SaveChangesAsync();

                ViewBag.Message = "Ekleme islemi basarili!";
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message.ToString();
            }
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> Delete(int? Id)
        {
            if (!Id.HasValue)
                return Json(null);

            var urun = await _boykotDbContext.Uruns.Where(x => x.Id == Id && x.Aktifmi == false).FirstAsync();

            if (urun == null)
                return Json(null);

            urun.Aktifmi = true;
            var result = await _boykotDbContext.SaveChangesAsync();
            return Json(result);
        }

        #region private
        public async Task<string> ImageUpload(IFormFile file)
        {
            if (file != null)
            {
                string imageExtension = Path.GetExtension(file.FileName);

                string imageName = Guid.NewGuid() + imageExtension;

                string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/images/{imageName}");

                using var stream = new FileStream(path, FileMode.Create);

                await file.CopyToAsync(stream);

                return imageName;
            }

            return string.Empty;
        }
        #endregion
    }
}
