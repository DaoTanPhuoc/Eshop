using Eshop.Data;
using Microsoft.AspNetCore.Mvc;

namespace Eshop.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    public class HomeController : Controller
    {
        private readonly EshopContext _context;

        public HomeController(EshopContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            int totalInMonth = _context.Invoices.Where(c => c.IssuedDate.Month == DateTime.Now.Month).Sum(c => c.Total);
            ViewData["totalInMonth"] = totalInMonth.ToString("#,##0.###");
            int totalInYear = _context.Invoices.Where(c => c.IssuedDate.Year == DateTime.Now.Year).Sum(c => c.Total);
            ViewData["totalInYear"] = totalInYear.ToString("#,##0.###");
            return View();
        }
    }
}
