using System;
using System.Collections.Generic;

namespace CNPM.Models;

public partial class TbPhong
{
    public int MaSoPhong { get; set; }

    public decimal? DonGia { get; set; }

    public int? SoNguoiMax { get; set; }

    public string? LoaiPhong { get; set; }

    public bool? TrangThai { get; set; }

    public string? MoTa { get; set; }

    public virtual ICollection<TbHopDong> TbHopDongs { get; set; } = new List<TbHopDong>();
}
