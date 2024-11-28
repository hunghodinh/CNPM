using CNPM.Models;
using System;
using System.Collections.Generic;

namespace CNPM.Models;

public partial class TbNhanVien
{
    public string MaNhanVien { get; set; } = null!;

    public string? TenNhanVien { get; set; }

    public string? SoDt { get; set; }

    public string? Cccd { get; set; }

    public string? Email { get; set; }

    public DateTime? NgaySinh { get; set; }

    public bool? GioiTinh { get; set; }

    public string? ChucVu { get; set; }

    public virtual ICollection<TbHopDong> TbHopDongs { get; set; } = new List<TbHopDong>();

    public virtual ICollection<TbTaiKhoan> TbTaiKhoans { get; set; } = new List<TbTaiKhoan>();
}
