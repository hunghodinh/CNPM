﻿using CNPM.Models;
using System;
using System.Collections.Generic;

namespace CNPM.Models;

public partial class TbSinhVien
{
    public string MaSinhVien { get; set; } = null!;

    public string? TenSinhVien { get; set; }

    public string? Cccd { get; set; }

    public string? SoDt { get; set; }

    public string? Email { get; set; }

    public DateTime? NgaySinh { get; set; }

    public string? DiaChi { get; set; }

    public bool? GioiTinh { get; set; }

    public virtual ICollection<TbHopDong> TbHopDongs { get; set; } = new List<TbHopDong>();

    public virtual ICollection<TbThongBao> TbThongBaos { get; set; } = new List<TbThongBao>();
}
