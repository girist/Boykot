using Boykot.WebApp.Enums;
using Boykot.WebApp.Models;
using Boykot.WebApp.Models.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

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
            //if (page != null && page < 1)
            //    page = 1;

            //var pageSize = 10;

            //var products = _boykotDbContext.Uruns
            //    .OrderByDescending(s => s.Id)
            //    .ToPagedList(page ?? 1, pageSize);

            //return View(products);
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
                .Select(x => new
                {
                    x.Adi,
                    x.Barkod,
                    x.Marka,
                    x.Kodu,
                    x.Ulke,
                    x.Id,
                    KategoriAdi = x.Kategori.Adi
                }).ToListAsync();

            return Json(result);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
