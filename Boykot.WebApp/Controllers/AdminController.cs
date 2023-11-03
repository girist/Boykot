﻿using Boykot.WebApp.Models;
using Boykot.WebApp.Models.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.IO;
using System;
using System.Linq;
using System.Threading.Tasks;
using Boykot.WebApp.Models.Response;

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
        public IActionResult CreateOrUpdate(int? id)
        {
            if (id.HasValue)
            {
                var product = _boykotDbContext.Uruns
                    .Where(x => x.Id == id && !x.Aktifmi)
                    .FirstOrDefault();

                if (product is null)
                    return View();

                var response = new UrunResponseModel
                {
                    Adi = product.Adi,
                    Barkod = product.Barkod,
                    Firma = product.Firma,
                    Id = product.Id,
                    KategoriAdi = product.Kategori?.Adi ?? string.Empty,
                    Kodu = product.Kodu,
                    Ulke = product.Ulke,
                    Not1 = product.Not1,
                    Not2 = product.Not2,
                    Aciklama = product.Aciklama,
                    Resim = product.Resim
                };

                return View(response);
            }
            return View();
        }
        /// <summary>
        /// Product Create or Update with Post Op.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateOrUpdate(IFormFile file, CreateUrunRequestModel model)
        {
            #region validation
            if (model is null)
                return null;

            if (model.Barkod is null)
            {
                ViewBag.Message = $"Barkod Bos Olamaz!!";
                return View();
            }

            bool hasProduct = _boykotDbContext.Uruns.Any(x => x.Barkod == model.Barkod.Trim());
            if (hasProduct)
            {
                ViewBag.Message = $"Bu Barkod Sistemde Kayitlidir! : {model.Barkod}";
                return View();
            }
            #endregion

            try
            {
                var product = new Urun()
                {
                    Aciklama = model.Aciklama,
                    Adi = model.Adi,
                    Barkod = model.Barkod,
                    Firma = model.Firma,
                    Kodu = model.Kodu,
                    Marka = model.Marka,
                    Not1 = model.Not1,
                    Not2 = model.Not2,
                    Ulke = model.Ulke,
                    Resim = $"/images/{ImageUpload(file).Result ?? "/images/no_product.png"}"
                };

                if (string.IsNullOrEmpty(model.Id.ToString()) || model.Id != 0)
                {
                    var hasproduct = _boykotDbContext.Uruns
                        .Where(x => x.Id == model.Id)
                        .FirstOrDefault();

                    hasproduct.Kodu = model.Kodu;
                    hasproduct.Not1 = model.Not1;
                    hasproduct.Barkod = model.Barkod;
                    hasproduct.Firma = model.Firma;
                    hasproduct.Adi = model.Adi;
                    hasproduct.Marka = model.Marka;
                    hasproduct.Aciklama = model.Aciklama;
                    hasproduct.Not2 = model.Not2;
                    hasproduct.Resim = file.FileName == null ? (model.Resim == null ? "/images/no_product.png"
                                                                                    : model.Resim)
                                                            : $"/images/{ImageUpload(file).Result ?? "/images/no_product.png"}";
                }
                else
                {
                    await _boykotDbContext.Uruns.AddAsync(product);
                }

                await _boykotDbContext.SaveChangesAsync();

                ViewBag.Message = "islem basarili!";
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message.ToString();
            }

            return View();
        }
        /// <summary>
        /// Delete Product
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> Delete(int? Id)
        {
            if (!Id.HasValue)
                return Json(null);

            var urun = await _boykotDbContext.Uruns
                .Where(x => x.Id == Id && x.Aktifmi == false)
                .FirstAsync();

            if (urun == null)
                return Json(null);

            urun.Aktifmi = true;
            var result = await _boykotDbContext.SaveChangesAsync();
            return Json(result);
        }

        #region private
        /// <summary>
        /// Image Upload
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
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
