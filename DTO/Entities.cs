using System;

namespace DTO
{
    // 1. Bảng Người Dùng (Chứa thông tin đăng nhập chung)
    public class NguoiDungDTO
    {
        public string MaNguoiDung { get; set; }
        public string TenDN { get; set; }
        public string MatKhau { get; set; }
        public string TrangThai { get; set; }
        public string VaiTro { get; set; }
        public int SoLanSai { get; set; }
    }

    // 2. Bảng Khách Hàng
    public class KhachHangDTO
    {
        public string MaKH { get; set; } // FK liên kết với MaNguoiDung
        public string HoVaTen { get; set; }
        public string SDT { get; set; }
        public string Email { get; set; }
        public int DiemTichLuy { get; set; }
    }

    // 3. Bảng Quản Lý
    public class QuanLyDTO
    {
        public string MaQL { get; set; } // FK liên kết với MaNguoiDung
        public string TenQL { get; set; }
        public string DiaChi { get; set; }
        public string SDT { get; set; }
    }

    // 4. Bảng Bàn
    public class BanDTO
    {
        public string MaBan { get; set; }
        public string Loai { get; set; }      // 'VIP', 'Thường', 'Gia đình'
        public string TenBan { get; set; }
        public string TrangThai { get; set; } // 'Trống', 'Có khách'
        public decimal TienCoc { get; set; }
    }

    // 5. Bảng Đặt Bàn
    public class DatBanDTO
    {
        public string MaDatBan { get; set; }
        public string MaKH { get; set; }
        public string MaBan { get; set; }
        public TimeSpan GioDen { get; set; }  // Kiểu TIME trong SQL -> TimeSpan trong C#
        public TimeSpan GioDi { get; set; }
        public DateTime NgayDat { get; set; }
        public string TrangThai { get; set; } // 'Chờ phản hồi', 'Đã cọc', 'Hủy'
    }

    // 6. Bảng Thực Đơn (Nhóm món ăn)
    public class ThucDonDTO
    {
        public string MaThucDon { get; set; }
        public string TenThucDon { get; set; }
        public string MoTa { get; set; }
    }

    // 7. Bảng Món Ăn
    public class MonAnDTO
    {
        public string MaMonAn { get; set; }
        public string MaThucDon { get; set; }
        public string TenMonAn { get; set; }
        public decimal Gia { get; set; }
        public int DiemThuong { get; set; }
    }

    // 8. Bảng Voucher
    public class VoucherDTO
    {
        public string MaVoucher { get; set; }
        public string MaMonAn { get; set; }
        public string MoTa { get; set; }
        public string LoaiGiamGia { get; set; } // 'Phần trăm', 'Trừ thẳng'
        public DateTime HanSuDung { get; set; }
    }

    // 9. Bảng Khách Hàng sở hữu Voucher
    public class KH_VoucherDTO
    {
        public string MaKH { get; set; }
        public string MaVoucher { get; set; }
        public int SoLuong { get; set; }
        public DateTime NgayNhan { get; set; }
        public string TrangThai { get; set; } // 'Còn hạn', 'Đã dùng', 'Hết hạn'
    }

    // 10. Bảng Hóa Đơn
    public class HoaDonDTO
    {
        public string MaHoaDon { get; set; }
        public string MaBan { get; set; }
        public string MaVoucher { get; set; } // Có thể null
        public string MaKH { get; set; }
        public DateTime ThoiGianLap { get; set; }
        public string MoTa { get; set; }
        public decimal ThanhTien { get; set; }
        public decimal TienGiam { get; set; }
        public decimal TongTien { get; set; }
        public string TrangThai { get; set; } // 'Đã thanh toán', 'Chưa thanh toán', 'Hủy'
        public string ThanhToan { get; set; } // 'Tiền mặt', 'Chuyển khoản'
    }

    // 11. Bảng Chi Tiết Hóa Đơn
    public class ChiTietHoaDonDTO
    {
        public string MaHoaDon { get; set; }
        public string MaMonAn { get; set; }
        public int SoLuong { get; set; }
        public decimal ThanhTien { get; set; }

        // Thuộc tính phụ để hiển thị lên lưới (Grid)
        public string TenMonAn { get; set; }
    }

    // 12. Bảng Thực Phẩm (Nguyên liệu kho)
    public class ThucPhamDTO
    {
        public string MaThucPham { get; set; }
        public string TenTP { get; set; }
        public int SoLuongTonKho { get; set; }
        public string DonViTinh { get; set; }
    }

    // 13. Bảng Công Thức (Định lượng món ăn)
    public class CongThucDTO
    {
        public string MaMonAn { get; set; }
        public string MaThucPham { get; set; }
        public decimal SoLuong { get; set; } // Số lượng nguyên liệu cần cho 1 món
    }

    // 14. Bảng Nhà Cung Cấp
    public class NhaCungCapDTO
    {
        public string MaNCC { get; set; }
        public string TenNCC { get; set; }
        public string DiaChi { get; set; }
        public string SDT { get; set; }
    }

    // 15. Bảng Nhân Viên
    public class NhanVienDTO
    {
        public string MaNV { get; set; }
        public string TenNV { get; set; }
        public string DiaChi { get; set; }
        public string SDT { get; set; }
    }

    // 16. Bảng Phiếu Nhập Hàng
    public class PhieuNhapHangDTO
    {
        public string MaPNH { get; set; }
        public string MaNCC { get; set; }
        public string MaNguoiNhap { get; set; } // FK tới NguoiDung
        public string TinhTrang { get; set; }
        public DateTime NgayNhapHang { get; set; }
        public decimal TongTien { get; set; }
    }

    // 17. Bảng Chi Tiết Nhập Hàng
    public class ChiTietNhapHangDTO
    {
        public string MaPNH { get; set; }
        public string MaThucPham { get; set; }
        public int SoLuong { get; set; }
        public decimal TongTien { get; set; }
    }

    public class CartItemDTO
    {
        public string MaThucPham { get; set; }
        public string TenTP { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public decimal ThanhTien => SoLuong * DonGia;
    }
}