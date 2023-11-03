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
                    products = products.Where(x => x.Adi.Contains(searchRequest.SearchText.ToUpper()));
                    break;
                //case SearchCriteriaEnum.Kategori:
                //    products = products.Where(x => x.Kategori.Adi.Contains(searchRequest.SearchText.ToUpper()));
                //    break;
                case SearchCriteriaEnum.Barkod:
                    products = products.Where(x => x.Barkod == searchRequest.SearchText);
                    break;
                default:
                    break;
            }
            var result = products.ToArray()
                                .Where(x => !x.Aktifmi)
                                .Select(x => new { x.Id,x.Adi,x.Barkod,x.Kodu,x.Ulke,x.Marka})
                                .GroupBy(x => new { x.Marka })
                                .Select(s => new{ 
                                    Count = s.Count(),
                                    Marka = s.Key.Marka,
                                    Uruns = s.Take(1)
                                }).Select(t => new UrunResponseModel
                                {
                                    Adi = t.Uruns.Select(s => s.Adi).FirstOrDefault(),
                                    Kodu = t.Uruns.Select(s => s.Kodu).FirstOrDefault(),
                                    Marka = t.Uruns.Select(s => s.Marka).FirstOrDefault(),
                                    Barkod = t.Uruns.Select(s => s.Barkod).FirstOrDefault(),
                                    Ulke = t.Uruns.Select(s => s.Ulke).FirstOrDefault(),
                                }).ToList();

            return Json(result);
        }
    }
}
