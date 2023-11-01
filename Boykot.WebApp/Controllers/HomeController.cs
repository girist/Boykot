using Boykot.WebApp.Enums;
using Boykot.WebApp.Models;
using Boykot.WebApp.Models.Request;
using Boykot.WebApp.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Boykot.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BoykotDbContext _boykotDbContext;

        public HomeController(ILogger<HomeController> logger, BoykotDbContext boykotDbContext = null)
        {
            _logger = logger;
            _boykotDbContext = boykotDbContext;
        }

        public IActionResult Index(int? page = 1)
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> SearchProducts(SearchRequestModel searchRequest)
        {
            var products = _boykotDbContext.Uruns.AsQueryable();

            if (searchRequest.SearchText is null)
                return null;

            switch ((SearchCriteriaEnum)Enum.Parse(typeof(SearchCriteriaEnum), searchRequest.Criteria, false))
            {
                case SearchCriteriaEnum.Urun:
                    products = products.Where(x => x.Adi.Contains(searchRequest.SearchText));
                    break;
                case SearchCriteriaEnum.Kategori:
                    products = products.Where(x => x.Kategori.Adi.Contains(searchRequest.SearchText));
                    break;
                case SearchCriteriaEnum.Barkod:
                    products = products.Where(x => x.Barkod == searchRequest.SearchText);
                    break;
                default:
                    break;
            }

            var result = await products
                .Select(x => new UrunResponseModel
                {
                    Adi = x.Adi,
                    Barkod = x.Barkod,
                    KategoriAdi = x.Kategori.Adi,
                    Kodu = x.Kodu,
                    Ulke = x.Ulke
                }).ToListAsync();

            return Json(result);
        }
    }
}
