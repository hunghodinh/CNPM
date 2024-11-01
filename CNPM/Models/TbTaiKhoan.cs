using System;
using System.Collections.Generic;

namespace CNPM.Models;

public partial class TbTaiKhoan
{
    public int IdTaiKhoan { get; set; }

    public string? TenTk { get; set; }

    public string? MatKhau { get; set; }

    public string MaNhanVien { get; set; } = null!;

    public virtual TbNhanVien MaNhanVienNavigation { get; set; } = null!;
}
