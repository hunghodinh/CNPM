using System;
using System.Collections.Generic;

namespace CNPM.Models;

public partial class TbHopDong
{
    public string IdHopDong { get; set; } = null!;

    public int MaSoPhong { get; set; }

    public string MaSinhVien { get; set; } = null!;

	public string MaNhanVien { get; set; } = null!;

    public DateTime? NgayBatDau { get; set; }

    public DateTime? NgayKetThuc { get; set; }

    public decimal? TienCoc { get; set; }

    public bool? TrangThai { get; set; }

    public string? GhiChu { get; set; }

    public virtual TbSinhVien MaSinhVien1 { get; set; } = null!;

    public virtual TbNhanVien MaSinhVienNavigation { get; set; } = null!;

    public virtual TbPhong MaSoPhongNavigation { get; set; } = null!;

    public virtual ICollection<TbHoaDon> TbHoaDons { get; set; } = new List<TbHoaDon>();
}
