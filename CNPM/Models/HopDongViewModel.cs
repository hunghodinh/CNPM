namespace CNPM.Models
{
	public class HopDongViewModel
{
    public string IdHopDong { get; set; } = null!;
    public int MaSoPhong { get; set; }
    public string? TenSinhVien { get; set; } // Đảm bảo kiểu dữ liệu phù hợp với trường TenSinhVien trong bảng TbSinhVien

	public string? TenNhanVien { get; set; }
	public string? MaNhanVien { get; set; }
	public DateTime? NgayBatDau { get; set; }
    public DateTime? NgayKetThuc { get; set; }
    public decimal? TienCoc { get; set; }
    public bool? TrangThai { get; set; }
}

}
