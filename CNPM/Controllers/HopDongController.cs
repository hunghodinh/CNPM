using CNPM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CNPM.Controllers
{
	public class HopDongController : Controller
	{
		private readonly CnpmContext _context;

		public HopDongController(CnpmContext context)
		{
			_context = context;
		}
		public IActionResult Index()
		{
			var hopDongVMs = _context.TbHopDongs
				.Include(hd => hd.MaSinhVienNavigation)
				.Include(hd => hd.MaSinhVienNavigation)
				.Select(hd => new HopDongViewModel
				{
					IdHopDong = hd.IdHopDong,
					MaSoPhong = hd.MaSoPhong,
					TenSinhVien = hd.MaSinhVienNavigation.TenSinhVien,
					
					MaNhanVien = hd.MaNhanVien,
					NgayBatDau = hd.NgayBatDau,
					NgayKetThuc = hd.NgayKetThuc,
					TienCoc = hd.TienCoc,
					TrangThai = hd.TrangThai
				})
				.ToList();

			return View(hopDongVMs);

		}

		/////////////////////////////
		public IActionResult Create()
		{
			// Lấy danh sách các phòng trống
			var phongs = _context.TbPhongs
				.Where(p => p.TrangThai == false)
				.Select(p => new { p.MaSoPhong })
				.ToList();
			ViewBag.PhongList = new SelectList(phongs, "MaSoPhong", "MaSoPhong");

			var sinhViens = _context.TbSinhViens
				.Where(sv => !_context.TbHopDongs.Select(hd => hd.MaSinhVien).Contains(sv.MaSinhVien))
				.Select(sv => new { sv.MaSinhVien, sv.TenSinhVien })
				.ToList();
			ViewBag.SinhVienList = new SelectList(sinhViens, "MaSinhVien", "TenSinhVien");

			var nhanViens = _context.TbNhanViens
				.Select(nv => new { nv.MaNhanVien, nv.TenNhanVien })
				.ToList();
			ViewBag.NhanVienList = new SelectList(nhanViens, "MaNhanVien", "TenNhanVien");

			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Create(TbHopDong mn)
		{
			if (ModelState.IsValid)
			{
				_context.TbHopDongs.Add(mn);
				_context.SaveChanges();
				return RedirectToAction("Index");
			}
			

			return View(mn);
		}

		////////////////////////////////////////////
		public IActionResult Delete(String? id)
		{
			if (id == null)
			{
				return NotFound();
			}
			var mn = _context.TbHopDongs.Find(id);
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
			var nv = _context.TbHopDongs.Find(id);
			if (nv == null)
			{
				return NotFound();
			}
			// Tìm tất cả các bản ghi 
			var tk = _context.TbHopDongs.Where(p => p.IdHopDong == id).ToList();
			if (tk.Any())
			{
				_context.TbHopDongs.RemoveRange(tk);
			}

			_context.TbHopDongs.Remove(nv);
			_context.SaveChanges();
			return RedirectToAction(nameof(Index));
		}
		/////////////////////////////////////
		public IActionResult Edit(String? id)
		{
			if (id == null)
			{
				return NotFound();
			}
			var po = _context.TbHopDongs.Find(id);
			if (po == null)
			{
				return NotFound();
			}

			var polist = _context.TbHopDongs.OrderBy(m => m.IdHopDong).ToList();
			ViewBag.poList = polist;
			return View(po);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Edit(TbHopDong po)
		{
			if (ModelState.IsValid)
			{
				// Kiểm tra xem thực thể có tồn tại hay không trước khi cập nhật
				var existing = _context.TbHopDongs.Find(po.IdHopDong);
				if (existing != null)
				{
					_context.Entry(existing).CurrentValues.SetValues(po);
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
