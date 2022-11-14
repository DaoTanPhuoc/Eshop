using Eshop.Data;
using Eshop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Eshop.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ILogger<DashboardController> _logger;
        private readonly EshopContext _context;

        public DashboardController(ILogger<DashboardController> logger, EshopContext context)
        {
			_logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Accounts()
        {
            return View("Accounts",await _context.Accounts.ToListAsync());
        }
        public async Task<IActionResult> DetailsAccount(int? id)
        {
            if (id == null || _context.Accounts == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (account == null)
            {
                return NotFound();
            }

            return View("DetailsAccount",account);
        }
        public IActionResult CreateAccount()
        {
            return View("CreateAccount");
        }
        public async Task<IActionResult> EditAccount(int? id)
        {
            if (id == null || _context.Accounts == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            return View("EditAccount",account);
        }
        public async Task<IActionResult> DeleteAccount(int? id)
        {
            if (id == null || _context.Accounts == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (account == null)
            {
                return NotFound();
            }

            return View("DeleteAccount",account);
        }
        public async Task<IActionResult> Invoices()
        {
            return View(await _context.Invoices.ToListAsync());
        }
        public async Task<IActionResult> DetailsInvoice(int? id)
        {
            if (id == null || _context.Invoices == null)
            {
                return NotFound();
            }

            var Invoice = await _context.Invoices
                .FirstOrDefaultAsync(m => m.Id == id);
            if (Invoice == null)
            {
                return NotFound();
            }

            return View("DetailsInvoice", Invoice);
        }
        public IActionResult CreateInvoice()
        {
            return View("CreateInvoice");
        }
        public async Task<IActionResult> EditInvoice(int? id)
        {
            if (id == null || _context.Invoices == null)
            {
                return NotFound();
            }

            var Invoice = await _context.Invoices.FindAsync(id);
            if (Invoice == null)
            {
                return NotFound();
            }
            return View("EditInvoice", Invoice);
        }
        public async Task<IActionResult> DeleteInvoice(int? id)
        {
            if (id == null || _context.Invoices == null)
            {
                return NotFound();
            }

            var Invoice = await _context.Invoices
                .FirstOrDefaultAsync(m => m.Id == id);
            if (Invoice == null)
            {
                return NotFound();
            }

            return View("DeleteInvoice", Invoice);
        }
        public async Task<IActionResult> Products()
        {
            return View(await _context.Products.ToListAsync());
        }
        public async Task<IActionResult> DetailsProduct(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var Product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (Product == null)
            {
                return NotFound();
            }

            return View("DetailsProduct", Product);
        }
        public IActionResult CreateProduct()
        {
            return View("CreateProduct");
        }
        public async Task<IActionResult> EditProduct(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var Product = await _context.Products.FindAsync(id);
            if (Product == null)
            {
                return NotFound();
            }
            return View("EditProduct", Product);
        }
        public async Task<IActionResult> DeleteProduct(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var Product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (Product == null)
            {
                return NotFound();
            }

            return View("DeleteProduct", Product);
        }
    }
}