using CNPM.Models;
using System;
using System.Collections.Generic;

namespace CNPM.Models;

public partial class TbThongBao
{
    public int IdThongBao { get; set; }

    public string? MaSinhVien { get; set; }

    public string? NoiDung { get; set; }

    public DateTime? NgayTao { get; set; }

    public virtual TbSinhVien? MaSinhVienNavigation { get; set; }
}
