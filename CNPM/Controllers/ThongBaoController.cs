using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using CNPM.Models;
using CNPM.Utilities;

public class ThongBaoController : Controller
{
    private readonly CnpmContext _context;
    private readonly EmailService _emailService;

    public ThongBaoController(CnpmContext context, EmailService emailService)
    {
        _context = context;
        _emailService = emailService;
    }

    // Hiển thị danh sách thông báo
    public IActionResult Index()
    {
        if (!Function.IsLogin())
            return RedirectToAction("Index", "Login");
        var thongBaos = _context.TbThongBaos
            .Include(tb => tb.MaSinhVienNavigation) // Lấy thông tin sinh viên
            .ToList();
        return View(thongBaos);
    }

    // Tạo thông báo (GET)
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    // Tạo thông báo (POST)
    [HttpPost]
    public async Task<IActionResult> Create(TbThongBao thongBao)
    {
        if (ModelState.IsValid)
        {
            // Kiểm tra nếu MaSinhVien trống
            if (string.IsNullOrWhiteSpace(thongBao.MaSinhVien))
            {
                ModelState.AddModelError("MaSinhVien", "Mã sinh viên không được để trống.");
                return View(thongBao); // Trả lại View với thông báo lỗi
            }

            // Kiểm tra xem Mã Sinh Viên có tồn tại trong cơ sở dữ liệu không
            var sinhVien = await _context.TbSinhViens
                                         .FirstOrDefaultAsync(sv => sv.MaSinhVien == thongBao.MaSinhVien);
            if (sinhVien == null)
            {
                ModelState.AddModelError("MaSinhVien", "Mã sinh viên không tồn tại.");
                return View(thongBao); // Trả lại View với thông báo lỗi
            }

            // Kiểm tra nếu NoiDung trống
            if (string.IsNullOrWhiteSpace(thongBao.NoiDung))
            {
                ModelState.AddModelError("NoiDung", "Nội dung không được để trống.");
                return View(thongBao); // Trả lại View với thông báo lỗi
            }

            thongBao.NgayTao = DateTime.Now;

            // Lưu thông báo vào cơ sở dữ liệu
            _context.TbThongBaos.Add(thongBao);
            await _context.SaveChangesAsync();

            // Gửi email thông báo nếu có email của sinh viên
            if (sinhVien != null && !string.IsNullOrEmpty(sinhVien.Email))
            {
                try
                {
                    await _emailService.SendEmailAsync(
                        sinhVien.Email,
                        "Thông báo từ ký túc xá",
                        thongBao.NoiDung
                    );
                    Console.WriteLine($"Email đã gửi tới: {sinhVien.Email}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Không thể gửi email: {ex.Message}");
                }
            }

            return RedirectToAction(nameof(Index));
        }

        // Nếu model không hợp lệ hoặc có lỗi, quay lại View Create
        return View(thongBao);
    }
    public IActionResult Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var mn = _context.TbThongBaos.Find(id);
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
        var nv = _context.TbThongBaos.Find(id);
        if (nv == null)
        {
            return NotFound();
        }
        // Tìm tất cả các bản ghi 
        var tk = _context.TbThongBaos.Where(p => p.IdThongBao == id).ToList();
        if (tk.Any())
        {
            _context.TbThongBaos.RemoveRange(tk);
        }

        _context.TbThongBaos.Remove(nv);
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

}
