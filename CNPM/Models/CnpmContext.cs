using System;
using System.Collections.Generic;
using CNPM.Models;
using Microsoft.EntityFrameworkCore;

namespace CNPM.Models;

public partial class CnpmContext : DbContext
{
    public CnpmContext()
    {
    }

    public CnpmContext(DbContextOptions<CnpmContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TbDichVu> TbDichVus { get; set; }

    public virtual DbSet<TbHoaDon> TbHoaDons { get; set; }

    public virtual DbSet<TbHopDong> TbHopDongs { get; set; }

    public virtual DbSet<TbNhanVien> TbNhanViens { get; set; }

    public virtual DbSet<TbPhong> TbPhongs { get; set; }

    public virtual DbSet<TbSinhVien> TbSinhViens { get; set; }

    public virtual DbSet<TbTaiKhoan> TbTaiKhoans { get; set; }

    public virtual DbSet<TbThongBao> TbThongBaos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("data source= LAPTOP-V7VL83QP; initial catalog=CNPM; integrated security=True; TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TbDichVu>(entity =>
        {
            entity.HasKey(e => e.IdDichVu);

            entity.ToTable("TbDichVu");

            entity.Property(e => e.IdDichVu).HasMaxLength(50);
            entity.Property(e => e.DonGia).HasColumnType("money");
            entity.Property(e => e.TenDichVu).HasMaxLength(50);
        });

        modelBuilder.Entity<TbHoaDon>(entity =>
        {
            entity.HasKey(e => e.IdHoaDon).HasName("PK_TbHoaDon_1");

            entity.ToTable("TbHoaDon");

            entity.Property(e => e.IdHoaDon).HasMaxLength(50);
            entity.Property(e => e.GhiChu).HasColumnType("text");
            entity.Property(e => e.IdDichVu).HasMaxLength(50);
            entity.Property(e => e.IdHopDong).HasMaxLength(50);
            entity.Property(e => e.NgayDong).HasColumnType("datetime");
            entity.Property(e => e.NguoiDong).HasMaxLength(50);
            entity.Property(e => e.TienPhong).HasColumnType("money");
            entity.Property(e => e.TongTien).HasColumnType("money");

            entity.HasOne(d => d.IdDichVuNavigation).WithMany(p => p.TbHoaDons)
                .HasForeignKey(d => d.IdDichVu)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TbHoaDon_TbDichVu");

            entity.HasOne(d => d.IdHopDongNavigation).WithMany(p => p.TbHoaDons)
                .HasForeignKey(d => d.IdHopDong)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TbHoaDon_TbHopDong");
        });

        modelBuilder.Entity<TbHopDong>(entity =>
        {
            entity.HasKey(e => e.IdHopDong).HasName("PK_TbHopDong_1");

            entity.ToTable("TbHopDong");

            entity.Property(e => e.IdHopDong).HasMaxLength(50);
            entity.Property(e => e.GhiChu).HasColumnType("text");
            entity.Property(e => e.MaNhanVien).HasMaxLength(50);
            entity.Property(e => e.MaSinhVien).HasMaxLength(50);
            entity.Property(e => e.NgayBatDau).HasColumnType("datetime");
            entity.Property(e => e.NgayKetThuc).HasColumnType("datetime");
            entity.Property(e => e.TienCoc).HasColumnType("money");

            entity.HasOne(d => d.MaNhanVienNavigation).WithMany(p => p.TbHopDongs)
                .HasForeignKey(d => d.MaNhanVien)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TbHopDong_TbNhanVien");

            entity.HasOne(d => d.MaSinhVienNavigation).WithMany(p => p.TbHopDongs)
                .HasForeignKey(d => d.MaSinhVien)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TbHopDong_TbSinhVien");

            entity.HasOne(d => d.MaSoPhongNavigation).WithMany(p => p.TbHopDongs)
                .HasForeignKey(d => d.MaSoPhong)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TbHopDong_TbPhong");
        });

        modelBuilder.Entity<TbNhanVien>(entity =>
        {
            entity.HasKey(e => e.MaNhanVien).HasName("PK_TbNhanVien_1");

            entity.ToTable("TbNhanVien");

            entity.Property(e => e.MaNhanVien).HasMaxLength(50);
            entity.Property(e => e.Cccd)
                .HasMaxLength(50)
                .HasColumnName("CCCD");
            entity.Property(e => e.ChucVu).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.NgaySinh).HasColumnType("datetime");
            entity.Property(e => e.SoDt)
                .HasMaxLength(50)
                .HasColumnName("SoDT");
            entity.Property(e => e.TenNhanVien).HasMaxLength(50);
        });

        modelBuilder.Entity<TbPhong>(entity =>
        {
            entity.HasKey(e => e.MaSoPhong).HasName("PK_TbPhong_1");

            entity.ToTable("TbPhong");

            entity.Property(e => e.MaSoPhong).ValueGeneratedNever();
            entity.Property(e => e.DonGia).HasColumnType("money");
            entity.Property(e => e.LoaiPhong).HasMaxLength(50);
            entity.Property(e => e.MoTa).HasColumnType("text");
        });

        modelBuilder.Entity<TbSinhVien>(entity =>
        {
            entity.HasKey(e => e.MaSinhVien).HasName("PK_TbSinhVien_1");

            entity.ToTable("TbSinhVien");

            entity.Property(e => e.MaSinhVien).HasMaxLength(50);
            entity.Property(e => e.Cccd)
                .HasMaxLength(50)
                .HasColumnName("CCCD");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.NgaySinh).HasColumnType("datetime");
            entity.Property(e => e.SoDt)
                .HasMaxLength(50)
                .HasColumnName("SoDT");
            entity.Property(e => e.TenSinhVien).HasMaxLength(50);
        });

        modelBuilder.Entity<TbTaiKhoan>(entity =>
        {
            entity.HasKey(e => e.IdTaiKhoan).HasName("PK_TbTaiKhoan_1");

            entity.ToTable("TbTaiKhoan");

            entity.Property(e => e.MaNhanVien).HasMaxLength(50);
            entity.Property(e => e.MatKhau).HasMaxLength(50);
            entity.Property(e => e.TenTk)
                .HasMaxLength(50)
                .HasColumnName("TenTK");

            entity.HasOne(d => d.MaNhanVienNavigation).WithMany(p => p.TbTaiKhoans)
                .HasForeignKey(d => d.MaNhanVien)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TbTaiKhoan_TbNhanVien");
        });

        modelBuilder.Entity<TbThongBao>(entity =>
        {
            entity.HasKey(e => e.IdThongBao).HasName("PK_Table_1");

            entity.ToTable("TbThongBao");

            entity.Property(e => e.MaSinhVien).HasMaxLength(50);
            entity.Property(e => e.NgayTao).HasColumnType("datetime");

            entity.HasOne(d => d.MaSinhVienNavigation).WithMany(p => p.TbThongBaos)
                .HasForeignKey(d => d.MaSinhVien)
                .HasConstraintName("FK_TbThongBao_TbSinhVien");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
