using CNPM.Models;
using CNPM.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public IActionResult Index()
        {
            var accounts = _context.TbTaiKhoans
                .Include(tk => tk.MaNhanVienNavigation) 
                .ToList();

            return View(accounts);
        }

        public IActionResult Create()
        {
            // Lấy danh sách nhân viên
            var nhanViens = _context.TbNhanViens
                .Select(nv => new SelectListItem
                {
                    Value = nv.MaNhanVien,
                    Text = nv.TenNhanVien
                })
                .ToList();

            ViewBag.NhanViens = nhanViens;
            return View();
        }

        [HttpPost]
        public IActionResult Create(TbTaiKhoan account)
        {
            if (account == null)
                return NotFound();

            var check = _context.TbTaiKhoans.Where(m => m.TenTk == account.TenTk).FirstOrDefault();
            if (check != null)
            {
                TempData["Message"] = "Tên tài khoản đã tồn tại!";
                return RedirectToAction("Create");
            }

            account.MatKhau = HashMD5.GetHash(account.MatKhau ?? "");
            _context.Add(account);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var mn = _context.TbTaiKhoans.Find(id);
            if (mn == null)
            {
                return NotFound();
            }
            return View(mn);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var nv = _context.TbTaiKhoans.Find(id);
            if (nv == null)
            {
                return NotFound();
            }
            // Tìm tất cả các bản ghi 
            var tk = _context.TbTaiKhoans.Where(p => p.IdTaiKhoan == id).ToList();
            if (tk.Any())
            {
                _context.TbTaiKhoans.RemoveRange(tk);
            }

            _context.TbTaiKhoans.Remove(nv);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        // Hiển thị form chỉnh sửa
        public IActionResult Edit(int id)
        {
            // Tìm tài khoản theo Id
            var account = _context.TbTaiKhoans.FirstOrDefault(t => t.IdTaiKhoan == id);
            if (account == null)
                return NotFound();

            // Lấy danh sách nhân viên
            var nhanViens = _context.TbNhanViens
                .Select(nv => new SelectListItem
                {
                    Value = nv.MaNhanVien,
                    Text = nv.TenNhanVien
                })
                .ToList();

            ViewBag.NhanViens = nhanViens;

            return View(account);
        }

        // Xử lý cập nhật tài khoản
        [HttpPost]
        public IActionResult Edit(int id, TbTaiKhoan updatedAccount)
        {
            if (updatedAccount == null)
                return NotFound();

            var account = _context.TbTaiKhoans.FirstOrDefault(t => t.IdTaiKhoan == id);
            if (account == null)
                return NotFound();

            // Kiểm tra trùng tên tài khoản (trừ chính tài khoản hiện tại)
            var check = _context.TbTaiKhoans
                .Where(m => m.TenTk == updatedAccount.TenTk && m.IdTaiKhoan != id)
                .FirstOrDefault();
            if (check != null)
            {
                TempData["Message"] = "Tên tài khoản đã tồn tại!";
                return RedirectToAction("Edit", new { id });
            }

            // Cập nhật thông tin
            account.TenTk = updatedAccount.TenTk;
            account.MaNhanVien = updatedAccount.MaNhanVien;

            // Chỉ cập nhật mật khẩu nếu người dùng nhập mới
            if (!string.IsNullOrEmpty(updatedAccount.MatKhau))
            {
                account.MatKhau = HashMD5.GetHash(updatedAccount.MatKhau);
            }

            _context.Update(account);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}
