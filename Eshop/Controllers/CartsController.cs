using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Eshop.Data;
using Eshop.Models;
using Microsoft.AspNetCore.Http;
 

namespace Eshop.Controllers
{
    public class CartsController : Controller
    {
        private readonly EshopContext _context;

        public CartsController(EshopContext context)
        {
            _context = context;
        }

        // GET: Carts
        public async Task<IActionResult> Index()
        {
            var eshopContext = _context.Carts.Include(c => c.Account).Include(c => c.Product);
            return View(await eshopContext.ToListAsync());
        }

        // GET: Carts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Carts == null)
            {
                return NotFound();
            }

            var cart = await _context.Carts
                .Include(c => c.Account)
                .Include(c => c.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // GET: Carts/Create
        public IActionResult Create()
        {
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Username");
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name");
            return View();
        }

        // POST: Carts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AccountId,ProductId,Quantity")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cart);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Username", cart.AccountId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", cart.ProductId);
            return View(cart);
        }

        // GET: Carts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Carts == null)
            {
                return NotFound();
            }

            var cart = await _context.Carts.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Username", cart.AccountId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", cart.ProductId);
            return View(cart);
        }

        // POST: Carts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AccountId,ProductId,Quantity")] Cart cart)
        {
            if (id != cart.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cart);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartExists(cart.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Username", cart.AccountId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", cart.ProductId);
            return View("IndexUser",id);
        }

        // GET: Carts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Carts == null)
            {
                return NotFound();
            }

            var cart = await _context.Carts
                .Include(c => c.Account)
                .Include(c => c.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // POST: Carts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Carts == null)
            {
                return Problem("Entity set 'EshopContext.Carts'  is null.");
            }
            var cart = await _context.Carts.FindAsync(id);
            if (cart != null)
            {
                _context.Carts.Remove(cart);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CartExists(int id)
        {
          return _context.Carts.Any(e => e.Id == id);
        }

        [HttpGet]
        public IActionResult addCreate (int AccountId, int ProductId, int soluong,string Actionk)
        {
            var itemcarts=_context.Carts.Where(p=>p.AccountId== AccountId && p.ProductId == ProductId).FirstOrDefault();
            var stockcart = _context.Products.Where(p=>p.Id==ProductId).FirstOrDefault();
             
            if (AccountId == -1)
            {
                return RedirectToAction("login","Accounts");
            }
            else
            {
                var item = _context.Carts.Where(p => p.ProductId == ProductId && p.AccountId == AccountId).ToList();
                var itemcart = new Cart
                {
                    AccountId = AccountId,
                    ProductId = ProductId,
                    Quantity = 1,
                };
                // new như chua co thi add và và so luong dúng 
                if (itemcarts == null)
                {
                    _context.Add(itemcart);
                    _context.SaveChanges();
                    soluong = _context.Carts.Where(a => a.AccountId == AccountId).Sum(p => p.Quantity);
                    HttpContext.Session.SetInt32("cartsAccount", soluong);
                    HttpContext.Response.Cookies.Append("cartadd", "1", new CookieOptions { Expires = DateTime.Now.AddSeconds(3) });


                }
                else
                {
                    if (itemcarts.Quantity >= stockcart.Stock)
                    {
                        HttpContext.Response.Cookies.Append("cartadd", "-1", new CookieOptions { Expires = DateTime.Now.AddSeconds(1) });
                        return RedirectToAction("Index", "Products");
                    }
                    else
                    {
                        if (item.Count == 0)
                        {
                            _context.Add(itemcart);
                            _context.SaveChanges();
                            soluong = _context.Carts.Where(a => a.AccountId == AccountId).Sum(p => p.Quantity);
                            HttpContext.Session.SetInt32("cartsAccount", soluong);
                            HttpContext.Response.Cookies.Append("cartadd", "1", new CookieOptions { Expires = DateTime.Now.AddSeconds(1) });

                        }
                        else
                        {
                            // lay ra id san pham de update
                            var idupdate = _context.Carts.Where(p => p.ProductId == ProductId && p.AccountId == AccountId).FirstOrDefault();

                            // gan id sam pham            
                            //cap nhat lai jso luon san pham
                            idupdate.Quantity += 1;
                            //update
                            _context.Update(idupdate);
                            _context.SaveChanges();
                            // dem so luon 
                            soluong = _context.Carts.Where(a => a.AccountId == AccountId).Sum(p => p.Quantity);
                            HttpContext.Session.SetInt32("cartsAccount", soluong);
                            HttpContext.Response.Cookies.Append("cartadd", "1", new CookieOptions { Expires = DateTime.Now.AddSeconds(1) });

                        }
                    }
                }
              
            }
            // cookie
            
            // trả về vị trí đang đứng 
            if (Actionk == "IndexProducts")
                return RedirectToAction("Index","Products");
            if(Actionk == "Detailitem")
                return RedirectToAction("Detailitem", "Products", new {id=ProductId});
            if (Actionk == "Homeindex")
                return RedirectToAction("Index", "Home");
            return RedirectToAction("Index", "Products");
        }
        public IActionResult Save([Bind("Id,AccountId,ProductId,Quantity")] Cart cartitem)
        {
            // goi so luong len
            var soluong = HttpContext.Session.GetInt32("cartsAccount");
             
            //
            if (cartitem.Quantity == 0)
            {
                _context.Carts.Remove(cartitem);
                _context.SaveChanges();
            }
            else
            {
                _context.Update(cartitem);
                _context.SaveChanges();
            }
            soluong = _context.Carts.Where(p => p.AccountId == cartitem.AccountId).Sum(p=>p.Quantity);
            HttpContext.Session.SetInt32("cartsAccount", (int)soluong);
        

            return RedirectToAction("IndexUser", "Carts", new {id=cartitem.AccountId});
        }

        public IActionResult IndexUser(int id)
		{
           
			if (id == 0)
			{
                return RedirectToAction("login", "Accounts");
            }
			else
			{
                var listcartitem = _context.Carts.Include(c => c.Product).Where(p => p.AccountId==id).ToList();
                ViewBag.items=listcartitem;
			}
            return View();
		}
    }
}
