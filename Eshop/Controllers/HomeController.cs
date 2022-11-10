using Eshop.Data;
using Eshop.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Eshop.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ILogger<HomeController> _logger;
        private EshopContext _context;
        public HomeController(ILogger<HomeController> logger, EshopContext context, IHttpContextAccessor contextAccessor)
        {
            _logger = logger;
            _context = context;
            _contextAccessor = contextAccessor;
        }

        public IActionResult Index()
        {
            var lstProduct = _context.Products.Take(3).ToList();
            return View(lstProduct);
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
    }
}