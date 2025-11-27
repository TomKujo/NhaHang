using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO; // Thư viện để ghi file báo cáo

namespace BLL
{
    // Enum kết quả đăng nhập (Đặt ngoài class để dễ gọi)
    public enum LoginResult
    {
        Success,
        InvalidCredentials,
        UserNotFound,
        Locked,
        CustomerDenied
    }

    public class ServiceBLL
    {
        private RepositoryDAL dal = new RepositoryDAL();

        // ==========================================================
        // 1. MODULE ĐĂNG NHẬP & QUẢN TRỊ
        // ==========================================================
        public LoginResult LoginAdvanced(string username, string password, out NguoiDungDTO userOut)
        {
            userOut = null;
            NguoiDungDTO user = dal.GetUserByUsername(username);

            if (user == null) return LoginResult.UserNotFound;
            if (user.VaiTro == "Khách hàng") return LoginResult.CustomerDenied;
            if (user.TrangThai == "Khóa") return LoginResult.Locked;

            if (user.MatKhau == password)
            {
                dal.ResetLoginFail(username);
                userOut = user;
                return LoginResult.Success;
            }
            else
            {
                int newCount = user.SoLanSai + 1;
                dal.UpdateLoginFail(username, newCount);
                if (newCount >= 3)
                {
                    dal.LockAccount(username);
                    return LoginResult.Locked;
                }
                return LoginResult.InvalidCredentials;
            }
        }

        public string ForgotPassword(string username)
        {
            NguoiDungDTO user = dal.GetUserByUsername(username);
            if (user != null)
            {
                dal.ResetPasswordByUsername(username, "123456");
                return "Mật khẩu đã được reset về mặc định: 123456";
            }
            return "Tài khoản không tồn tại";
        }

        // ==========================================================
        // 2. MODULE QUẢN LÝ BÀN & ĐẶT LỊCH
        // ==========================================================
        public List<BanDTO> GetListBan() => dal.GetListBan();

        public string AddTable(string t, string l) => dal.ThemBan(t, l) ? "Thêm thành công" : "Lỗi thêm bàn";
        public string UpdateTable(string m, string t, string l) => dal.SuaBan(m, t, l) ? "Sửa thành công" : "Lỗi sửa bàn";
        public string DeleteTable(string m) => dal.XoaBan(m) ? "Xóa thành công" : "Không thể xóa bàn đang có khách/đặt trước!";

        public string BookTableAdvanced(string maBan, string tenKhach, string sdt, DateTime ngayDat, TimeSpan gioDen, TimeSpan gioDi)
        {
            if (ngayDat.Date <= DateTime.Now.Date) return "Quy định: Phải đặt trước ít nhất 1 ngày!";
            if (dal.CheckTrungLich(maBan, ngayDat, gioDen, gioDi)) return "Lỗi: Khung giờ này đã có người đặt!";

            return dal.InsertDatBan(maBan, "khach1", ngayDat, gioDen, gioDi) ? "Đặt bàn thành công!" : "Lỗi hệ thống.";
        }

        public bool CanSitAtTable(string maBan, out string reason)
        {
            DataTable dt = dal.GetUpcomingBooking(maBan);
            if (dt.Rows.Count > 0)
            {
                TimeSpan gioBooking = (TimeSpan)dt.Rows[0]["GioDen"];
                TimeSpan now = DateTime.Now.TimeOfDay;
                if (gioBooking.Subtract(now).TotalHours < 2 && gioBooking > now)
                {
                    reason = $"Bàn có lịch đặt lúc {gioBooking:hh\\:mm}. Không nhận khách trước 2 tiếng.";
                    return false;
                }
            }
            reason = "";
            return true;
        }

        public void RefreshTableStatus() => dal.AutoUpdateTableStatus();

        // ==========================================================
        // 3. MODULE ORDER & THANH TOÁN
        // ==========================================================
        public DataTable GetOrderDetails(string maBan)
        {
            string maHD = dal.GetUnpaidBillID(maBan);
            return maHD == null ? null : dal.GetChiTietHoaDonInfo(maHD);
        }

        public string OrderMon(string maBan, string maMon, int soLuong)
        {
            try
            {
                string maHD = dal.GetUnpaidBillID(maBan);
                if (maHD == null)
                {
                    dal.InsertHoaDon(maBan, "khach1");
                    maHD = dal.GetUnpaidBillID(maBan);
                }

                dal.InsertChiTietHoaDon(maHD, maMon, soLuong);

                // Tích điểm thưởng
                string maKH = dal.GetKhachHangByHoaDon(maHD);
                int diemMon = dal.GetDiemThuongMon(maMon);
                int tongDiem = diemMon * soLuong;

                if (!string.IsNullOrEmpty(maKH) && tongDiem > 0)
                    dal.CongDiemTichLuy(maKH, tongDiem);

                return $"Gọi món thành công! (Cộng {tongDiem} điểm)";
            }
            catch (Exception ex) { return "Lỗi: " + ex.Message; }
        }

        public bool Checkout(string maBan, decimal tong)
        {
            string maHD = dal.GetUnpaidBillID(maBan);
            if (maHD != null)
            {
                dal.Checkout(maHD, maBan, tong);
                return true;
            }
            return false;
        }

        // ==========================================================
        // 4. MODULE THỰC ĐƠN
        // ==========================================================
        public DataTable GetListThucDon() => dal.GetListThucDon();
        public List<MonAnDTO> GetListMonAn(string maTD) => dal.GetMonAnByThucDon(maTD);

        public string AddCategory(string ten, string mt) => dal.ThemThucDon(ten, mt) ? "Thêm thành công" : "Lỗi";
        public string UpdateCategory(string id, string ten, string mt) => dal.SuaThucDon(id, ten, mt) ? "Sửa thành công" : "Lỗi";
        public string DeleteCategory(string id) => dal.XoaThucDon(id) ? "Xóa thành công" : "Không thể xóa (còn món ăn)";

        public string AddDish(MonAnDTO m) => dal.ThemMonAn(m) ? "Thêm món thành công" : "Lỗi";
        public string UpdateDish(MonAnDTO m) => dal.SuaMonAn(m) ? "Sửa món thành công" : "Lỗi";
        public string DeleteDish(string id) => dal.XoaMonAn(id) ? "Xóa món thành công" : "Lỗi";

        // ==========================================================
        // 5. MODULE KHO & NHÀ CUNG CẤP
        // ==========================================================
        public DataTable GetInventory() => dal.GetKhoHang();
        public string AddIngredient(string t, string d) => dal.ThemThucPhamMoi(t, d) ? "Thêm thành công" : "Lỗi";

        public DataTable GetListNCC() => dal.GetNhaCungCap();
        public string AddSupplier(string t, string d, string s) => dal.ThemNCC(t, d, s) ? "Thêm NCC thành công" : "Lỗi";
        public string UpdateSupplier(string m, string t, string d, string s) => dal.SuaNCC(m, t, d, s) ? "Cập nhật thành công" : "Lỗi";
        public string DeleteSupplier(string m) => dal.XoaNCC(m) ? "Xóa thành công" : "NCC đã có giao dịch, không thể xóa!";

        public string ImportStock(string maNCC, string maNV, List<CartItemDTO> items, string tenNCC)
        {
            if (items.Count == 0) return "Giỏ hàng trống!";

            string maPNH;
            if (dal.TaoPhieuNhap(maNCC, maNV, items, out maPNH))
            {
                ExportImportBill(maPNH, tenNCC, maNV, items);
                return "Nhập hàng thành công! Đã xuất file.";
            }
            return "Lỗi nhập hàng.";
        }

        private void ExportImportBill(string maPNH, string tenNCC, string maNV, List<CartItemDTO> items)
        {
            try
            {
                string fileName = $"PhieuNhap_{maPNH}.txt";
                using (StreamWriter sw = new StreamWriter(fileName))
                {
                    sw.WriteLine("========== PHIẾU NHẬP KHO ==========");
                    sw.WriteLine($"Mã: {maPNH} - Ngày: {DateTime.Now:dd/MM/yyyy HH:mm}");
                    sw.WriteLine($"NCC: {tenNCC} - Người nhập: {maNV}");
                    sw.WriteLine("------------------------------------------------");
                    sw.WriteLine(string.Format("{0,-20} | {1,5} | {2,12}", "Tên hàng", "SL", "Thành tiền"));
                    sw.WriteLine("------------------------------------------------");
                    decimal tong = 0;
                    foreach (var item in items)
                    {
                        sw.WriteLine(string.Format("{0,-20} | {1,5} | {2,12:N0}",
                            item.TenTP.Length > 20 ? item.TenTP.Substring(0, 17) + "..." : item.TenTP,
                            item.SoLuong, item.ThanhTien));
                        tong += item.ThanhTien;
                    }
                    sw.WriteLine("------------------------------------------------");
                    sw.WriteLine($"TỔNG CỘNG: {tong:N0} VNĐ");
                }
                System.Diagnostics.Process.Start("notepad.exe", fileName);
            }
            catch { }
        }

        // ==========================================================
        // 6. MODULE NHÂN SỰ
        // ==========================================================
        public DataTable GetStaff() => dal.GetNhanVien();
        public string AddStaff(NhanVienDTO nv) => dal.ThemNhanVien(nv.TenNV, nv.SDT, nv.DiaChi) ? "Thêm thành công" : "Lỗi";
        public string UpdateStaff(NhanVienDTO nv) => dal.SuaNhanVien(nv.MaNV, nv.TenNV, nv.SDT, nv.DiaChi) ? "Cập nhật thành công" : "Lỗi";
        public string DeleteStaff(string id) => dal.XoaNhanVien(id) ? "Xóa thành công" : "Lỗi";
    }
}