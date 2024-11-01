using CNPM.Models;
using Microsoft.AspNetCore.Mvc;

namespace CNPM.Controllers
{
    public class SinhVienController : Controller
    {
        private readonly CnpmContext _context;

        public SinhVienController(CnpmContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<TbSinhVien> rooms = _context.TbSinhViens.ToList();
            return View(rooms);
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
            if (ModelState.IsValid)
            {
                _context.TbSinhViens.Add(mn);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

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
