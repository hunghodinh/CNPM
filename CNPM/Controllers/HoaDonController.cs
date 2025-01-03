using CNPM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CNPM.Utilities;

namespace CNPM.Controllers
{
	public class HoaDonController : Controller
	{
		private readonly CnpmContext _context;

		public HoaDonController(CnpmContext context)
		{
			_context = context;
		}
        public IActionResult Index(DateTime? NgayDong)
        {
            if (!Function.IsLogin())
                return RedirectToAction("Index", "Login");

            var hoaDons = _context.TbHoaDons.AsQueryable();

            if (NgayDong.HasValue)
            {
                hoaDons = hoaDons.Where(hd => hd.NgayDong.HasValue && hd.NgayDong.Value.Date == NgayDong.Value.Date);
            }

            // Lưu giá trị NgayDong vào ViewBag để sử dụng trong view
            ViewBag.NgayDong = NgayDong?.ToString("dd-MM-yyyy");

            return View(hoaDons.ToList());
        }



        public IActionResult Create()
        {
            // Lọc danh sách hợp đồng chưa có hóa đơn
            var sinhViens = _context.TbHopDongs
                .Include(hd => hd.MaSinhVienNavigation)
                .Where(hd => !_context.TbHoaDons.Any(hdExist => hdExist.IdHopDong == hd.IdHopDong)) // Lọc hợp đồng chưa có hóa đơn
                .Select(hd => new SelectListItem
                {
                    Value = hd.IdHopDong,
                    Text = hd.MaSinhVienNavigation.TenSinhVien
                })
                .ToList();

            // Lấy danh sách TenDichVu từ bảng TbDichVu
            var dichVus = _context.TbDichVus
                .Select(dv => new SelectListItem
                {
                    Value = dv.IdDichVu,
                    Text = dv.TenDichVu
                })
                .ToList();

            ViewBag.SinhViens = sinhViens;
            ViewBag.DichVus = dichVus;

            return View();
        }





        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TbHoaDon hoaDon)
        {
            if (hoaDon == null)
                return NotFound();

            // Kiểm tra nếu IdHoaDon đã tồn tại
            var existingHoaDon = _context.TbHoaDons.FirstOrDefault(hd => hd.IdHoaDon == hoaDon.IdHoaDon);
            if (existingHoaDon != null)
            {
                TempData["Message"] = $"Id hóa đơn '{hoaDon.IdHoaDon}' đã tồn tại, vui lòng nhập lại!";
                return RedirectToAction("Create");
            }

            // Kiểm tra dịch vụ có tồn tại không
            var dichVu = _context.TbDichVus.FirstOrDefault(dv => dv.IdDichVu == hoaDon.IdDichVu);
            if (dichVu == null)
            {
                TempData["Message"] = "Dịch vụ không hợp lệ!";
                return RedirectToAction("Create");
            }

            // Tính toán tổng tiền
            hoaDon.TongTien = hoaDon.TienPhong + dichVu.DonGia;

            // Lưu vào cơ sở dữ liệu
            hoaDon.NgayDong = DateTime.Now;
            _context.TbHoaDons.Add(hoaDon);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }




        public IActionResult Delete(String? id)
		{
			if (id == null)
			{
				return NotFound();
			}
			var mn = _context.TbHoaDons.Find(id);
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
			var nv = _context.TbHoaDons.Find(id);
			if (nv == null)
			{
				return NotFound();
			}
			// Tìm tất cả các bản ghi 
			var tk = _context.TbHoaDons.Where(p => p.IdHoaDon == id).ToList();
			if (tk.Any())
			{
				_context.TbHoaDons.RemoveRange(tk);
			}

			_context.TbHoaDons.Remove(nv);
			_context.SaveChanges();
			return RedirectToAction(nameof(Index));
		}


		public IActionResult Edit(string idHoaDon)
		{
			// Tìm hóa đơn cần cập nhật
			var hoaDon = _context.TbHoaDons.FirstOrDefault(hd => hd.IdHoaDon == idHoaDon);

			if (hoaDon == null)
			{
				return NotFound();
			}

			// Trả về view với thông tin hiện tại của hóa đơn
			return View(hoaDon);
		}

		// POST: Hóa Đơn/Edit/{id}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Edit(string idHoaDon, int trangThai)
		{
			// Tìm hóa đơn cần cập nhật
			var hoaDon = _context.TbHoaDons.FirstOrDefault(hd => hd.IdHoaDon == idHoaDon);

			if (hoaDon == null)
			{
				return NotFound();
			}

			// Cập nhật trạng thái mới cho hóa đơn
			hoaDon.TrangThai = trangThai;

			// Lưu thay đổi vào cơ sở dữ liệu
			_context.SaveChanges();

			// Sau khi cập nhật thành công, chuyển hướng về trang danh sách hóa đơn
			return RedirectToAction("Index");
		}

        public IActionResult InHD(string? idHd)
        {
            if (string.IsNullOrEmpty(idHd))
            {
                return BadRequest("Mã hóa đơn không hợp lệ.");
            }

            var invoice = _context.TbHoaDons
                         .Include(hd => hd.IdDichVuNavigation) // Load thông tin dịch vụ nếu có
                         .FirstOrDefault(hd => hd.IdHoaDon == idHd);

            if (invoice == null)
            {
                return NotFound($"Không tìm thấy hóa đơn với mã: {idHd}");
            }

            return View(invoice);
        }

    }
}
