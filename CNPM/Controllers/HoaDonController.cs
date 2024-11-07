using CNPM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CNPM.Controllers
{
	public class HoaDonController : Controller
	{
		private readonly CnpmContext _context;

		public HoaDonController(CnpmContext context)
		{
			_context = context;
		}
		public IActionResult Index()
		{
			List<TbHoaDon> r = _context.TbHoaDons.ToList();
			return View(r);
		}

        public IActionResult Create()
        {
			// Lấy danh sách sinh viên và hợp đồng chưa có trong bảng TbHoaDon
			var sinhViens = _context.TbHopDongs
									.Where(h => !_context.TbHoaDons.Any(hd => hd.IdHopDong == h.IdHopDong))
									.Select(h => new {
										MaSinhVien = h.MaSinhVien,
										TenSinhVien = h.MaSinhVien1.TenSinhVien,
										IdHopDong = h.IdHopDong
									})
									.ToList();

			ViewBag.SinhViens = sinhViens;
			return View();
        }
        [HttpPost]
		public IActionResult Create(string idHoaDon, string idHopDong, string idDichVu, DateTime? ngayDong, string? nguoiDong, decimal? tienPhong, int? trangThai, string? ghiChu)
		{
			// Lấy thông tin hợp đồng và dịch vụ
			var hopDong = _context.TbHopDongs.FirstOrDefault(h => h.IdHopDong == idHopDong);
			var dichVu = _context.TbDichVus.FirstOrDefault(d => d.IdDichVu == idDichVu);

			// Kiểm tra tồn tại của hợp đồng và dịch vụ
			if (hopDong == null || dichVu == null)
			{
				return BadRequest("Hợp đồng hoặc dịch vụ không tồn tại.");
			}

			// Tính tổng tiền từ tiền phòng và đơn giá dịch vụ
			decimal tongTien = (tienPhong ?? 0) + (dichVu.DonGia ?? 0);

			// Tạo mới hóa đơn
			TbHoaDon hoaDon = new TbHoaDon
			{
				IdHoaDon = idHoaDon,
				IdHopDong = idHopDong,
				IdDichVu = idDichVu,
				NgayDong = ngayDong,
				NguoiDong = nguoiDong,
				TienPhong = tienPhong,
				TongTien = tongTien,
				TrangThai = trangThai,
				GhiChu = ghiChu
			};

			// Thêm hóa đơn vào database
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



	}
}
