using System;
using System.Collections.Generic;

namespace CNPM.Models;

public partial class TbHoaDon
{
    public string IdHoaDon { get; set; } = null!;

    public string IdHopDong { get; set; } = null!;

    public string IdDichVu { get; set; } = null!;

    public DateTime? NgayDong { get; set; }

    public string? NguoiDong { get; set; }

    public decimal? TienPhong { get; set; }

    public decimal? TongTien { get; set; }

    public int? TrangThai { get; set; }

    public string? GhiChu { get; set; }

    public virtual TbDichVu IdDichVuNavigation { get; set; } = null!;

    public virtual TbHopDong IdHopDongNavigation { get; set; } = null!;
}
