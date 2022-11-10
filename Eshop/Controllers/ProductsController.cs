using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using Eshop.Data;
using Eshop.Models;

namespace Eshop.Controllers
{
    public class ProductsController : Controller
    {
        private readonly EshopContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly EshopContext db=new EshopContext();
        public ProductsController(EshopContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }
        // GET: Products
         
        public ActionResult Index(int ?page,string searchString, int producttytleID = 0,string sortproperType="",string sortOrder="")
        {
            var books = _context.Products.Include(p=>p.ProductType);
            var skipbooks = from a in books select(a);
            //luu ket qua search
            ViewBag.Keyword = searchString;
            //search
            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                skipbooks = skipbooks.Where(b => b.Name.Contains(searchString));
            }
            //search id cate
            else if (producttytleID != 0) {
                skipbooks = skipbooks.Include(p => p.ProductType).Where(b => b.ProductTypeId== producttytleID);
            }

            // show cate and id cate
            if (producttytleID != 0)
            {
                skipbooks = skipbooks.Where(c => c.ProductTypeId == producttytleID);
            }
            ViewBag.producttytleID = new SelectList(_context.ProductTypes,"Id","Name");
            // sort lever


            // phan trang+search
            var countitem = skipbooks.Count();
            if (page == null)
            {
                page = 1;
               

                    skipbooks = skipbooks.Skip((int)(page - 1) * 6).Take(6);
                    ViewBag.tab = countitem / 6;
                
            }
            if(countitem<=0)
			{
                ViewBag.tab = 0;
            }
            else if (countitem < 6)
			{
                skipbooks = skipbooks.Skip((int)(page - 1) * countitem).Take(countitem);
                ViewBag.tab = 1;
            }
			else
			{
                skipbooks = skipbooks.Skip((int)(page - 1) * 6).Take(6);
                ViewBag.tab = ViewBag.tab = countitem / 6;
            }

            // sap xep nang cao
                // tao gia tri sap xep
            if (sortOrder == "asc") ViewBag.SortOrder = "desc";
            if (sortOrder == "desc") ViewBag.SortOrder = "";
            if (sortOrder == "") ViewBag.SortOrder = "asc";


			// phuong thuc sap sep dinh cao
			if (sortproperType == "Name")
			{
             
                skipbooks=skipbooks.OrderByDescending(x => x.Name);
			}
            return View(skipbooks.ToList());
        }
        

        // GET: Products/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null || _context.Products == null)
        //    {
        //        return NotFound();
        //    }

        //    var product = await _context.Products
        //        .Include(p => p.ProductType)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(product);
        //}

        // GET: Products/Create
        //public IActionResult Create()
        //{
        //    ViewData["ProductTypeId"] = new SelectList(_context.ProductTypes, "Id", "Name");
        //    return View();
        //}

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SKU,Name,Description,Price,Stock,ProductTypeId,ImageFile,Status")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                if (product.ImageFile != null)
                {
                    var filename = product.Id.ToString() + Path.GetExtension(product.ImageFile.FileName);
                    var uploadPath = Path.Combine(_environment.WebRootPath, "img", "product");
                    var filePath = Path.Combine(uploadPath, filename);
                    using (FileStream fs = System.IO.File.Create(filePath))
                    {
                        product.ImageFile.CopyTo(fs);
                        fs.Flush();
                    }
                    product.Image = filename;
                    _context.Products.Update(product);
                    _context.SaveChanges();
                }
                return RedirectToAction("Products", "Dashboard");
            }
            ViewData["ProductTypeId"] = new SelectList(_context.ProductTypes, "Id", "Name", product.ProductTypeId);
            return RedirectToAction("Products", "Dashboard");
        }

        // GET: Products/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null || _context.Products == null)
        //    {
        //        return NotFound();
        //    }

        //    var product = await _context.Products.FindAsync(id);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["ProductTypeId"] = new SelectList(_context.ProductTypes, "Id", "Name", product.ProductTypeId);
        //    return View(product);
        //}

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SKU,Name,Description,Price,Stock,ProductTypeId,Image,Status")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Products", "Dashboard");
            }
            ViewData["ProductTypeId"] = new SelectList(_context.ProductTypes, "Id", "Name", product.ProductTypeId);
            return RedirectToAction("Products", "Dashboard");
        }

        // GET: Products/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null || _context.Products == null)
        //    {
        //        return NotFound();
        //    }

        //    var product = await _context.Products
        //        .Include(p => p.ProductType)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(product);
        //}

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'EshopContext.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction("Products", "Dashboard");
        }

        private bool ProductExists(int id)
        {
          return _context.Products.Any(e => e.Id == id);
        }

        /// view product customer
        public IActionResult Search(int? p, string? searchString)
        {
            if (p == null)
            {
                p = 1;
            }
            var productskip = _context.Products.Include(p => p.ProductType).Skip((int)(p - 1) * 6).Take(6).ToList();
            var tabproduct = _context.Products.Count();

            if (searchString != null)
            {
                productskip = _context.Products.Include(p => p.ProductType).Where(p => p.Name.Contains(searchString)).Skip((int)(p - 1) * 6).Take(6).ToList();
            }
            var brand = _context.ProductTypes.ToList();
            ViewBag.nameBrand = brand;
            ViewBag.tab = tabproduct / 6;

            return View("Index", productskip);
        }
        public IActionResult Brand(int? b)
        {
            var productskip = _context.Products.Where(p => p.ProductTypeId == b).ToList();
            if (productskip != null)
            {

                return RedirectToAction("Index","Produts", productskip);
            }
            else
            {
                return View("Error");
            }
            
        }
        //public async Task<IActionResult> Index()
        //{
        //    var eshopContext = _context.Products.Include(p => p.ProductType);
        //    return View(await eshopContext.ToListAsync());
        //}

    }
}

