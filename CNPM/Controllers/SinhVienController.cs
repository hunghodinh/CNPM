using CNPM.Models;
using Microsoft.AspNetCore.Mvc;
using CNPM.Utilities;

namespace CNPM.Controllers
{
    public class SinhVienController : Controller
    {
        private readonly CnpmContext _context;

        public SinhVienController(CnpmContext context)
        {
            _context = context;
        }
        public IActionResult Index(string? MaSinhVien)
        {
            if (!Function.IsLogin())
                return RedirectToAction("Index", "Login");

            // Lấy danh sách sinh viên
            var sinhViens = _context.TbSinhViens.AsQueryable();

            // Lọc theo MaSinhVien nếu có
            if (!string.IsNullOrEmpty(MaSinhVien))
            {
                sinhViens = sinhViens.Where(sv => sv.MaSinhVien.Contains(MaSinhVien));
            }

            // Truyền giá trị tìm kiếm qua ViewBag để hiển thị lại trên giao diện
            ViewBag.MaSinhVien = MaSinhVien;

            return View(sinhViens.ToList());
        }

        public IActionResult Create()
        {
            var mn = _context.TbSinhViens.OrderBy(m => m.MaSinhVien).ToList();
            ViewBag.mn = mn;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TbSinhVien mn)
        {
            var check = _context.TbSinhViens.Where(m => m.MaSinhVien == mn.MaSinhVien).FirstOrDefault();
            if (check != null)
            {
                TempData["Message"] = "Mã sinh viên đã tồn tại";
                return RedirectToAction("Create");
            }

            if (ModelState.IsValid)
            {
                _context.TbSinhViens.Add(mn);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            // Nếu có lỗi, trả lại View kèm dữ liệu
            return View(mn);
        }

        public IActionResult Delete(String? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var mn = _context.TbSinhViens.Find(id);
            if (mn == null)
            {
                return NotFound();
            }
            return View(mn);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string id)
        {
            var nv = _context.TbSinhViens.Find(id);
            if (nv == null)
            {
                return NotFound();
            }
            // Tìm tất cả các bản ghi 
            var tk = _context.TbSinhViens.Where(p => p.MaSinhVien == id).ToList();
            if (tk.Any())
            {
                _context.TbSinhViens.RemoveRange(tk);
            }

            _context.TbSinhViens.Remove(nv);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Edit(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var po = _context.TbSinhViens.Find(id);
            if (po == null)
            {
                return NotFound();
            }

            var polist = _context.TbSinhViens.OrderBy(m => m.MaSinhVien).ToList();
            ViewBag.poList = polist;
            return View(po);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(TbSinhVien po)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem thực thể có tồn tại hay không trước khi cập nhật
                var existingStudent = _context.TbSinhViens.Find(po.MaSinhVien);
                if (existingStudent != null)
                {
                    _context.Entry(existingStudent).CurrentValues.SetValues(po);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    return NotFound(); // Xử lý nếu thực thể không tồn tại
                }
            }
            return View(po);
        }
    }
}
