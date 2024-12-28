using CNPM.Models;
using CNPM.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace CNPM.Controllers
{
    public class LoginController : Controller
    {
        private readonly CnpmContext _context;

        public LoginController(CnpmContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(TbTaiKhoan account)
        {
            if (account == null)
            {
                return NotFound();
            }
            string password = HashMD5.GetHash(account.MatKhau);
            var check = _context.TbTaiKhoans.Where(m => m.TenTk == account.TenTk && m.MatKhau == password).FirstOrDefault();
            if (check == null)
            {
                Function._Message = "Tên người dùng hoặc mật khẩu không hợp lệ";
                return RedirectToAction("Index", "Login");
            }
            Function._Message = string.Empty;
            Function._AccountId = check.IdTaiKhoan;
            Function._Username = check.TenTk;
            return RedirectToAction("Index", "Home");
        }
    }
}
