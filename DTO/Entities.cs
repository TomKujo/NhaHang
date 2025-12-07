using System;

namespace DTO
{
    public class NguoiDungDTO
    {
        public string MaNguoiDung { get; set; }
        public string TenDN { get; set; }
        public string MatKhau { get; set; }
        public string TrangThai { get; set; }
        public string VaiTro { get; set; }
        public int SoLanSai { get; set; }
    }

    public class KhachHangDTO
    {
        public string MaKH { get; set; }
        public string Ten { get; set; }
        public string SDT { get; set; }
        public string Email { get; set; }
        public int DiemTichLuy { get; set; }
    }

    public class NhanVienDTO
    {
        public string MaNV { get; set; }
        public string Ten { get; set; }
        public string DiaChi { get; set; }
        public string SDT { get; set; }
        public string Email { get; set; }
        public string TrangThaiTK { get; set; }
        public string VaiTro { get; set; }
        public decimal Luong { get; set; }
    }

    public class QuanLyDTO
    {
        public string MaQL { get; set; }
        public string Ten { get; set; }
        public string SDT { get; set; }
    }

    public class BanDTO
    {
        public string MaBan { get; set; }
        public string Loai { get; set; }
        public string Ten { get; set; }
        public decimal TienCoc { get; set; }
        public string TrangThai { get; set; }
    }

    public class DatBanDTO
    {
        public string MaDatBan { get; set; }
        public string MaKH { get; set; }
        public string MaBan { get; set; }
        public DateTime ThoiGian { get; set; }
        public string TrangThai { get; set; }

        // Thuộc tính hiển thị (Helper)
        public string TenKhachHang { get; set; }
        public string SDT { get; set; }
    }

    public class ThucDonDTO
    {
        public string MaThucDon { get; set; }
        public string Ten { get; set; }
        public string MoTa { get; set; }
    }

    public class MonDTO
    {
        public string MaMon { get; set; }
        public string MaThucDon { get; set; }
        public string Ten { get; set; }
        public decimal Gia { get; set; }
        public int DiemTichLuy { get; set; }
    }

    public class HoaDonDTO
    {
        public string MaHD { get; set; }
        public string MaBan { get; set; }
        public string MaKH { get; set; }
        public string MaKM { get; set; }
        public DateTime ThoiGianLap { get; set; }
        public decimal TongTien { get; set; }
        public string TrangThai { get; set; }
        public string HinhThucThanhToan { get; set; }
    }

    public class ChiTietHDDTO
    {
        public int ID { get; set; }
        public string MaHD { get; set; }
        public string MaMon { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public string GhiChu { get; set; }

        // Thuộc tính hiển thị (Helper)
        public string TenMon { get; set; }
    }

    public class NguyenLieuDTO
    {
        public string MaNguyenLieu { get; set; }
        public string Ten { get; set; }
        public decimal SoLuongTon { get; set; }
        public string DonVi { get; set; }
    }

    public class CongThucDTO
    {
        public string MaMon { get; set; }
        public string MaNguyenLieu { get; set; }
        public decimal LuongTieuHao { get; set; }

        // Thuộc tính hiển thị (Helper)
        public string TenNguyenLieu { get; set; }
        public string DonVi { get; set; }
    }

    public class NhaCungCapDTO
    {
        public string MaNCC { get; set; }
        public string Ten { get; set; }
        public string DiaChi { get; set; }
        public string SDT { get; set; }
    }

    public class PhieuNhapHangDTO
    {
        public string MaPhieu { get; set; }
        public string MaNCC { get; set; }
        public string MaNV { get; set; }
        public DateTime ThoiGian { get; set; }
        public decimal TongTien { get; set; }

        // Đã xóa 'TinhTrang' vì bảng PhieuNhapHang trong SQL không có cột này

        // Thuộc tính hiển thị (Helper)
        public string TenNCC { get; set; }
        public string TenNV { get; set; }
    }

    public class ChiTietNhapDTO
    {
        public string MaPhieu { get; set; }
        public string MaNguyenLieu { get; set; }

        // Cập nhật theo SQL: Thay 'SoLuong' bằng các trường chi tiết
        public decimal LuongYeuCau { get; set; }
        public decimal LuongThucTe { get; set; }
        public decimal DonGia { get; set; }
        public string TinhTrang { get; set; } // SQL: NVARCHAR(10) DEFAULT N'Đủ'

        // Thuộc tính hiển thị (Helper)
        public string TenNguyenLieu { get; set; }
    }

    public class KhuyenMaiDTO
    {
        public string MaKM { get; set; }
        public string TenKM { get; set; }
        public string MoTa { get; set; }
        public int DiemCanThiet { get; set; }
        public decimal GiaTriGiam { get; set; }
        public string LoaiGiam { get; set; }
        public DateTime NgayBatDau { get; set; }
        public DateTime? NgayKetThuc { get; set; } // Dùng DateTime? vì SQL cho phép NULL
        public string TrangThai { get; set; }
    }

    public class DoiDiemDTO
    {
        public string MaGiaoDich { get; set; }
        public string MaKH { get; set; }
        public string MaKM { get; set; }
        public DateTime ThoiGianDoi { get; set; }
        public int DiemDaDung { get; set; }
        public string TrangThai { get; set; }

        // Thuộc tính hiển thị (Helper)
        public string TenKhuyenMai { get; set; }
        public string TenKhachHang { get; set; }
    }

    public class BaoCaoDTO
    {
        public int Thang { get; set; }
        public decimal DoanhThu { get; set; }
        public decimal ChiPhiNhap { get; set; }
        public decimal LuongNhanVien { get; set; }
        public decimal TongChiPhi { get; set; }
        public decimal LoiNhuan { get; set; }
    }
}