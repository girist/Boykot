using Boykot.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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

        public IActionResult Index(int? page = 1)
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> Delete(int? Id)
        {
            if (!Id.HasValue)
                return Json(null);

            var urun = await _boykotDbContext.Uruns.Where(x => x.Id == Id).FirstAsync();

            if (urun == null)
                return Json(null);

            var result = _boykotDbContext.Uruns.Remove(urun);

            return Json(result.State);
        }
    }
}
