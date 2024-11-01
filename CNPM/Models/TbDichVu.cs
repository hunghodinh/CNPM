using System;
using System.Collections.Generic;

namespace CNPM.Models;

public partial class TbDichVu
{
    public string IdDichVu { get; set; } = null!;

    public string? TenDichVu { get; set; }

    public decimal? DonGia { get; set; }

    public virtual ICollection<TbHoaDon> TbHoaDons { get; set; } = new List<TbHoaDon>();
}
