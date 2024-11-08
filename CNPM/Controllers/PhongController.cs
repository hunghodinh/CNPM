﻿using CNPM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CNPM.Controllers
{
	public class PhongController : Controller
	{
		private readonly CnpmContext _context;

		public PhongController(CnpmContext context)
		{
			_context = context;
		}
		public IActionResult Index()
		{
			List<TbPhong> rooms = _context.TbPhongs.ToList();
			return View(rooms);
		}
        public async Task<IActionResult> ChiTiet(int maSoPhong)
        {
            var danhSachSinhVien = await _context.TbHopDongs
                .Where(hd => hd.MaSoPhong == maSoPhong && hd.TrangThai == true) // Kiểm tra trạng thái hợp đồng còn hiệu lực
                .Include(hd => hd.MaSinhVien1)  // Bao gồm thông tin sinh viên
                .Select(hd => hd.MaSinhVien1)   // Chọn đối tượng sinh viên
                .ToListAsync();

            return View(danhSachSinhVien);
        }

        public IActionResult Create()
		{
			var mn = _context.TbPhongs.OrderBy(m => m.MaSoPhong).ToList();
			ViewBag.mn = mn;
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Create(TbPhong mn)
		{
			if (ModelState.IsValid)
			{
				_context.TbPhongs.Add(mn);
				_context.SaveChanges();

				return RedirectToAction("Index");
			}

			return View(mn);
		}

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var po = _context.TbPhongs.Find(id);
            if (po == null)
            {
                return NotFound();
            }

            var polist = _context.TbPhongs.OrderBy(m => m.MaSoPhong).ToList();
            ViewBag.poList = polist;
            return View(po);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(TbPhong po)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem thực thể có tồn tại hay không trước khi cập nhật
                var room = _context.TbSinhViens.Find(po.MaSoPhong);
                if (room != null)
                {
                    _context.Entry(room).CurrentValues.SetValues(po);
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
