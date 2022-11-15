using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Eshop.Data;
using Eshop.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Eshop.Controllers
{
    public class InvoicesController : Controller
    {
        private readonly EshopContext _context;

        public InvoicesController(EshopContext context)
        {
            _context = context;
        }

        // GET: Invoices
        public async Task<IActionResult> Index()
        {
            var eshopContext = _context.Invoices.Include(i => i.Account);
            return View(await eshopContext.ToListAsync());
        }

        // GET: Invoices/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null || _context.Invoices == null)
        //    {
        //        return NotFound();
        //    }

        //    var invoice = await _context.Invoices
        //        .Include(i => i.Account)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (invoice == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(invoice);
        //}

        // GET: Invoices/Create
        //public IActionResult Create()
        //{
        //    ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Username");
        //    return View();
        //}

        // POST: Invoices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Code,AccountId,IssuedDate,ShippingAddress,ShippingPhone,Total,Status")] Invoice invoice)
        {
            if (ModelState.IsValid)
            {
                _context.Add(invoice);
                await _context.SaveChangesAsync();
                return RedirectToAction("Invoices", "Dashboard");
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Username", invoice.AccountId);
            return RedirectToAction("Invoices", "Dashboard");
        }

        // GET: Invoices/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null || _context.Invoices == null)
        //    {
        //        return NotFound();
        //    }

        //    var invoice = await _context.Invoices.FindAsync(id);
        //    if (invoice == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Username", invoice.AccountId);
        //    return View(invoice);
        //}

        // POST: Invoices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Code,AccountId,IssuedDate,ShippingAddress,ShippingPhone,Total,Status")] Invoice invoice)
        {
            if (id != invoice.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(invoice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvoiceExists(invoice.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Invoices", "Dashboard");
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Username", invoice.AccountId);
            return RedirectToAction("Invoices", "Dashboard");
        }

        // GET: Invoices/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null || _context.Invoices == null)
        //    {
        //        return NotFound();
        //    }

        //    var invoice = await _context.Invoices
        //        .Include(i => i.Account)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (invoice == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(invoice);
        //}

        // POST: Invoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Invoices == null)
            {
                return Problem("Entity set 'EshopContext.Invoices'  is null.");
            }
            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice != null)
            {
                _context.Invoices.Remove(invoice);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction("Invoices", "Dashboard");
        }

        private bool InvoiceExists(int id)
        {
          return _context.Invoices.Any(e => e.Id == id);
        }

        public IActionResult Purchase()
        {
            var id = HttpContext.Session.GetInt32("id");
            if (id == null)
                return RedirectToAction("login", "Accounts");
            return View();
        }
        [HttpPost]
        public IActionResult Purchase(String shipAddress, String shipPhone)
        {
            var id = HttpContext.Session.GetInt32("id");
            if(id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            if(shipAddress == null || shipPhone == null)
            {
                return View();
            }
            var accountId = _context.Accounts.Where(a => a.Id == id).FirstOrDefault().Id;

            var carts = _context.Carts.Include(c => c.Product).Include(c => c.Account);
   

            int total = _context.Carts.Where(c=>c.AccountId == accountId).Sum(c => c.Product.Stock * c.Product.Price);
            Invoice invoice = new Invoice
            {
                Code = DateTime.Now.ToString("ddMMyyyyhhmmss"),
                AccountId = accountId,
                IssuedDate = DateTime.Now,
                ShippingAddress = shipAddress,
                ShippingPhone = shipPhone,
                Total = total,
                Status = true
            };
            _context.Invoices.Add(invoice);
            _context.SaveChanges();
            foreach (var item in carts)
            {
                InvoiceDetail detail = new InvoiceDetail
                {
                    InvoiceId = invoice.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.Product.Price
                };
                item.Product.Stock -= item.Quantity;
                _context.InvoiceDetails.Add(detail);
                _context.Carts.Remove(item);
                _context.Products.Update(item.Product);
                
            }
            
            _context.SaveChanges();
            int soluong = _context.Carts.Where(a => a.AccountId == id).Sum(p => p.Quantity);
            HttpContext.Session.SetInt32("cartsAccount", soluong);
            return RedirectToAction("Index", "Home");
        }
    }
}
