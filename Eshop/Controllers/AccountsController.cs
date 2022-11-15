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
    public class AccountsController : Controller
    {
        private readonly EshopContext _context;
        private readonly IWebHostEnvironment _environment;

        public AccountsController(EshopContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: Accounts
        public async Task<IActionResult> Index()
        {
            return View(await _context.Accounts.ToListAsync());
        }

        // GET: Accounts/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null || _context.Accounts == null)
        //    {
        //        return NotFound();
        //    }

        //    var account = await _context.Accounts
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (account == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(account);
        //}

        // GET: Accounts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Username,Password,Email,Phone,Address,FullName,IsAdmin,Avatar,Status")] Account account)
        {
            if (ModelState.IsValid)
            {
                _context.Add(account);
                await _context.SaveChangesAsync();
                return RedirectToAction("Accounts", "Dashboard");
            }
            return RedirectToAction("Accounts", "Dashboard");
        }

        // GET: Accounts/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null || _context.Accounts == null)
        //    {
        //        return NotFound();
        //    }

        //    var account = await _context.Accounts.FindAsync(id);
        //    if (account == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(account);
        //}

        // POST: Accounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Username,Password,Email,Phone,Address,FullName,IsAdmin,ImageFile,Status")] Account account)
        {
            if (id != account.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(account);
                    await _context.SaveChangesAsync();
                    if (account.ImageFile != null)
                    {
                        var filename = account.Id.ToString() + Path.GetExtension(account.ImageFile.FileName);
                        var uploadPath = Path.Combine(_environment.WebRootPath, "img", "avatar");
                        var filePath = Path.Combine(uploadPath, filename);
                        using (FileStream fs = System.IO.File.Create(filePath))
                        {
                            account.ImageFile.CopyTo(fs);
                            fs.Flush();
                        }
                        account.Avatar = filename;
                        _context.Accounts.Update(account);
                        _context.SaveChanges();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountExists(account.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("EditAccount", "Dashboard");
            }
            return RedirectToAction("EditAccount", "Dashboard");
        }

        // GET: Accounts/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null || _context.Accounts == null)
        //    {
        //        return NotFound();
        //    }

        //    var account = await _context.Accounts
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (account == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(account);
        //}

        // POST: Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Accounts == null)
            {
                return Problem("Entity set 'EshopContext.Account'  is null.");
            }
            var account = await _context.Accounts.FindAsync(id);
            if (account != null)
            {
                _context.Accounts.Remove(account);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Accounts", "Dashboard");
        }

        private bool AccountExists(int id)
        {
            return _context.Accounts.Any(e => e.Id == id);
        }



        // create (dang ky phuoc)
        public IActionResult create_Account()
        {
            return View();
        }

        [HttpPost]


        public IActionResult create_Account([Bind("Id,Username,Password,Email,Phone,Address,FullName,IsAdmin,ImageFile,Status")] Account account)
        {

            var accounts = _context.Accounts.FirstOrDefault(x => x.Username == account.Username);
            if (accounts == null)
            {
                _context.Accounts.Add(account);
                _context.SaveChanges();

                if (account.ImageFile != null)
                {
                    var fileName = account.Id.ToString() + Path.GetExtension(account.ImageFile.FileName);
                    var uploadPath = Path.Combine(_environment.WebRootPath, "img", "avatar");
                    var filePath = Path.Combine(uploadPath, fileName);
                    using (FileStream fs = System.IO.File.Create(filePath))
                    {
                        account.ImageFile.CopyTo(fs);
                        fs.Flush();
                    }
                    account.Avatar = fileName;
                    _context.Accounts.Update(account);
                    _context.SaveChanges();
                }
                
            }
            
            else
            {
                //ViewBag.Error = "Tai khoan da ton tai";
               
                return View();
            }
            //return RedirectToAction("Index", "Home");
            return View("login");
        }



        // chức năng đăng nhập (phuoc)
        public IActionResult login(Account account, int? check)
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult login([Bind("Username,Password")] Account account)
        {

            var obj = _context.Accounts.Where(u => u.Username == account.Username &&
                u.Password == account.Password).FirstOrDefault();
            if (obj == null)
            {
                var countError = 0;
                HttpContext.Session.SetInt32("Error", value: countError);

                ViewBag.ErrorCount = countError;

                return View();

                // neu đăng nhập tài khoản k có trở về trang đăng ký

            }
            if (obj.IsAdmin == false)// trang user 
            {
                HttpContext.Session.SetInt32("id", obj.Id);
                HttpContext.Session.SetString("Name", obj.FullName);
                // khi dang nhap thanh cong thi dua san pham tu gio hang cua nguoi do ra ngoai
                var soluong = _context.Carts.Where(a => a.AccountId == obj.Id).Sum(p => p.Quantity);
                HttpContext.Session.SetInt32("cartsAccount", soluong);
                return RedirectToAction("Index", "Home");

            }
            else 
            {
                // khi dang nhap thanh cong thi dua san pham tu gio hang cua nguoi do ra ngoai
                var soluong = _context.Carts.Where(a => a.AccountId == obj.Id).Sum(p => p.Quantity);
                HttpContext.Session.SetInt32("cartsAccount", soluong);
                //
                HttpContext.Session.SetInt32("id", obj.Id);
                HttpContext.Session.SetString("Name", "Admin");
                return RedirectToAction("Index","Home");// day la trang cua admin
            }
            
        }



        public IActionResult logout()
        {
            //var session = HttpContext.Session;

            if (HttpContext.Session.GetString("Name") != null)
            {
                HttpContext.Session.Remove("id");
                HttpContext.Session.Remove("Name");
                HttpContext.Session.Remove("cartsAccount");
            }
            return RedirectToAction("Index", "Home");
        }


        // xem thông tin người dùng đăng nhập 
        public IActionResult detail(int? Id)
        {
            if (Id == null)
            {
                return RedirectToAction("login", "Accounts");
            }
            var obj = _context.Accounts.Where(u => u.Id == Id).FirstOrDefault();
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }



        public IActionResult update(int? Id)
        {
            if (Id == null)
            {
                return RedirectToAction("Index");
            }
            var account = _context.Accounts.FirstOrDefault(x => x.Id == Id);
            if (account == null)
            {
                return NotFound();
            }
            return View(account);
        }
        // update
        [HttpPost]
        public IActionResult update(int id, [Bind("Id,Username,FullName,Email,Password,Phone,Address,Avatar,ImageFile,IsAdmin,Status")] Account account)
        {
            if (account.ImageFile != null)
            {
                var fileName = id.ToString() + Path.GetExtension(account.ImageFile.FileName);
                var uploadPath = Path.Combine(_environment.WebRootPath, "img", "avatar");
                var filePath = Path.Combine(uploadPath, fileName);
                using (FileStream fs = System.IO.File.Create(filePath))
                {
                    account.ImageFile.CopyTo(fs);
                    fs.Flush();
                }
                account.Avatar = fileName;
            }

            _context.Update(account);
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        // đổi mật khẩu
        [HttpPost]
        public IActionResult updatePass(int Id,string oldPass, string newPass)
        {

            var obj = _context.Accounts.Where(x => x.Password == oldPass && x.Id==Id).FirstOrDefault();
            if (obj != null)
            {

                obj.Password = newPass;
                _context.Update(obj);
                _context.SaveChanges();

            }
            else
            {
                ViewBag.ErrorMess = "Mật khẩu không hợp lệ! ";
                
            }
            
            return RedirectToAction("detail", "Accounts", new {Id=Id});

        }
       
    }
}
