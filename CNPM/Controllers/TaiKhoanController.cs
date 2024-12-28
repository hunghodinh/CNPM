using CNPM.Models;
using CNPM.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CNPM.Controllers
{
    public class TaiKhoanController : Controller
    {
        private readonly CnpmContext _context;

        public TaiKhoanController(CnpmContext context)
        {
            _context = context;
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(TbTaiKhoan account)
        {
            if (account == null) { return NotFound(); }

            var check = _context.TbTaiKhoans.Where(m => m.TenTk == account.TenTk).FirstOrDefault();
            if (check != null)
            {
                Function._Message = "Trùng tài khoản";
                return RedirectToAction("Index", "Register");
            }
            Function._Message = string.Empty;
            account.MatKhau = HashMD5.GetHash(account.MatKhau != null ? account.MatKhau : "");
            _context.Add(account);
            _context.SaveChanges();
            return RedirectToAction("Index", "TaiKhoan");
        }
    }
}
