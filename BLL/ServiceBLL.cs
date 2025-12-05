using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BLL
{
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
        RepositoryDAL dal = new RepositoryDAL();

        // ==========================================================
        // 1. MODULE ĐĂNG NHẬP & QUẢN TRỊ
        // ==========================================================
        public LoginResult LoginAdvanced(string username, string password, out NguoiDungDTO userOut)
        {
            userOut = null;
            NguoiDungDTO user = dal.GetUserByUsername(username);

            if (user == null) return LoginResult.UserNotFound;

            // YÊU CẦU: TÀI KHOẢN KHÁCH KHÔNG THỂ ĐĂNG NHẬP VÀO HỆ THỐNG QUẢN TRỊ
            if (user.VaiTro == "Khách hàng") return LoginResult.CustomerDenied;

            if (user.TrangThai == "Khóa") return LoginResult.Locked;

            // Kiểm tra mật khẩu
            if (user.MatKhau == password)
            {
                // Reset số lần sai về 0
                dal.UpdateLoginFail(username, 0);
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
            if (user == null) return "Tài khoản không tồn tại";
            if (user.VaiTro == "Khách hàng") return "Khách hàng vui lòng liên hệ lễ tân để reset mật khẩu!";

            dal.ResetPass(username, "123456");
            return "Mật khẩu mới là 123456";
        }

        public string UnlockAccount(string username)
        {
            dal.UnlockAccount(username);
            return "Đã mở khóa tài khoản!";
        }

        // ==========================================================
        // 2. MODULE QUẢN LÝ BÀN & ĐẶT LỊCH
        // ==========================================================
        public string Login(string u, string p, out NguoiDungDTO user)
        {
            user = dal.GetUserByUsername(u);
            if (user == null) return "User not found";
            if (user.VaiTro == "Khách hàng") return "Khách hàng không được phép đăng nhập!";
            if (user.TrangThai == "Khóa") return "Tài khoản bị khóa!";

            if (user.MatKhau != p)
            {
                dal.UpdateLoginFail(u, user.SoLanSai + 1);
                if (user.SoLanSai + 1 >= 3) dal.LockAccount(u);
                return "Sai mật khẩu!";
            }
            // Reset số lần sai
            dal.UpdateLoginFail(u, 0);
            return "OK";
        }

        public string UnlockStaff(string u) { dal.UnlockAccount(u); return "Đã mở khóa!"; }

        // --- Bàn ---
        public List<BanDTO> GetListBan() => dal.GetListBan();

        public string AddTable(string t, string l) => dal.ThemBan(t, l) ? "OK" : "Fail";
        public string UpdateTable(string m, string t, string l) => dal.SuaBan(m, t, l) ? "Sửa thành công" : "Lỗi sửa bàn";
        public string DeleteTable(string m) => dal.XoaBan(m) ? "Xóa thành công" : "Không thể xóa bàn đang có khách/đặt trước!";

        // Update status bàn thủ công
        public string UpdateTableState(string maBan, string state)
        {
            dal.UpdateTableStatus(maBan, state);
            return "Cập nhật trạng thái thành công";
        }

        // --- Đặt bàn ---
        public string BookTableAdvanced(string maBan, string tenKhach, string sdt, DateTime thoiGian)
        {
            if (thoiGian.Date < DateTime.Now.Date) return "Quy định: Không thể đặt lùi ngày!";

            if (dal.CheckTrungLich(maBan, thoiGian)) return "Lỗi: Khung giờ này đã có người đặt!";

            // Kiểm tra hoặc tạo khách hàng (logic đơn giản hóa cho Advanced)
            string maKH = dal.GetKhachHangBySDT(sdt);
            if (string.IsNullOrEmpty(maKH))
            {
                maKH = dal.InsertKhachHang(tenKhach, sdt);
            }

            return dal.InsertDatBan(maBan, maKH, thoiGian) ? "Đặt bàn thành công!" : "Lỗi hệ thống.";
        }

        public string BookTableSmart(string maBan, string tenKhach, string sdt, DateTime thoiGian)
        {
            if (thoiGian < DateTime.Now) return "Thời gian đặt phải lớn hơn hiện tại!";

            if (dal.CheckTrungLich(maBan, thoiGian)) return "Bàn này đã bị trùng lịch!";

            // Tự động tìm hoặc tạo khách hàng mới
            string maKH = dal.GetKhachHangBySDT(sdt);
            if (maKH == null)
            {
                maKH = dal.InsertKhachHang(tenKhach, sdt);
                if (maKH == null) return "Lỗi tạo thông tin khách hàng!";
            }

            return dal.InsertDatBan(maBan, maKH, thoiGian) ? "Đặt bàn thành công!" : "Lỗi hệ thống.";
        }

        public bool CanSitAtTable(string maBan, out string reason)
        {
            DataTable dt = dal.GetBookings(maBan);

            foreach (DataRow r in dt.Rows)
            {
                DateTime thoiGianDat = Convert.ToDateTime(r["ThoiGian"]);
                string trangThai = r["TrangThai"].ToString();

                if (trangThai == "Chờ phản hồi" || trangThai == "Đã xác nhận")
                {
                    TimeSpan diff = thoiGianDat - DateTime.Now;
                    // Nếu còn dưới 2 tiếng nữa là đến giờ đặt khách khác
                    if (diff.TotalHours > 0 && diff.TotalHours < 2)
                    {
                        reason = $"Bàn có lịch đặt lúc {thoiGianDat:HH:mm}. Không nhận khách trước 2 tiếng.";
                        return false;
                    }
                }
            }
            reason = "";
            return true;
        }

        public DataTable GetBookings(string maBan) => dal.GetBookings(maBan);

        public string CancelBooking(string maDatBan)
        {
            return dal.CancelDatBan(maDatBan) ? "Đã hủy lịch đặt." : "Lỗi không thể hủy.";
        }

        public string UpdateBookingSmart(string maDatBan, string maBanMoi, DateTime ngayGio)
        {
            if (ngayGio < DateTime.Now) return "Không thể dời về quá khứ!";

            if (dal.CheckTrungLich(maBanMoi, ngayGio, maDatBan))
                return "Bàn mới bị trùng lịch trong khung giờ này!";

            return dal.UpdateDatBan(maDatBan, maBanMoi, ngayGio) ? "Cập nhật thành công!" : "Lỗi hệ thống";
        }

        public DataTable GetAvailableTables(DateTime date)
        {
            return dal.GetAvailableTables(date);
        }

        // ==========================================================
        // 3. MODULE ORDER & THANH TOÁN
        // ==========================================================
        public DataTable GetOrderDetails(string maBan)
        {
            string maHD = dal.GetUnpaidBillID(maBan);
            return maHD == null ? null : dal.GetOrderDetails(maHD);
        }

        public string OrderMon(string maBan, string maMon, string strSL, string ghiChu)
        {
            try
            {
                if (!int.TryParse(strSL, out int sl) || sl <= 0) return "Số lượng phải là số nguyên lớn hơn 0!";

                string maHD = dal.GetUnpaidBillID(maBan);
                if (maHD == null)
                {
                    // Tạo hóa đơn mới. Lưu ý: DAL InsertHoaDonWithNote nhận ghiChu nhưng SQL không lưu, 
                    // tuy nhiên ta vẫn truyền vào để tuân thủ interface của DAL.
                    dal.InsertHoaDonWithNote(maBan, "khach1", ghiChu);
                    maHD = dal.GetUnpaidBillID(maBan);
                }

                if (maHD != null)
                {
                    dal.InsertChiTietHD(maHD, maMon, sl, ghiChu);
                    return "Gọi món thành công!";
                }
                return "Lỗi tạo hóa đơn!";
            }
            catch (Exception ex) { return "Lỗi: " + ex.Message; }
        }

        public bool Checkout(string maBan, decimal tong)
        {
            try
            {
                string maHD = dal.GetUnpaidBillID(maBan);
                if (maHD != null)
                {
                    dal.DeductInventoryOnCheckout(maHD); // Trừ tồn kho theo công thức

                    // Tích điểm cho khách
                    string maKH = dal.GetKhachHangByHoaDon(maHD);
                    if (!string.IsNullOrEmpty(maKH))
                    {
                        // Logic tích điểm: 100k = 1 điểm
                        int diem = (int)(tong / 100000);
                        dal.CongDiemTichLuy(maKH, diem);
                    }

                    dal.Checkout(maHD, maBan, tong);
                    return true;
                }
                return false;
            }
            catch { return false; }
        }

        // ==========================================================
        // 4. MODULE THỰC ĐƠN & MÓN
        // ==========================================================
        public DataTable GetListThucDon() => dal.GetListThucDon();
        public List<MonDTO> GetListMonAn(string maTD) => dal.GetMonByThucDon(maTD);
        public DataTable GetFullMenu() => dal.GetFullMenu();

        // --- Danh mục (Thực đơn) ---
        public string AddCategory(string ten, string mt) => dal.ThemThucDon(ten, mt) ? "Thêm thành công" : "Lỗi";
        public string UpdateCategory(string id, string ten, string mt) => dal.SuaThucDon(id, ten, mt) ? "Sửa thành công" : "Lỗi";
        public string DeleteCategory(string id) => dal.XoaThucDon(id) ? "Xóa thành công" : "Không thể xóa (còn món ăn)";

        // --- Món ăn ---
        public string AddDish(string td, string ten, decimal gia, int diem)
        {
            MonDTO mon = new MonDTO { MaThucDon = td, Ten = ten, Gia = gia, DiemTichLuy = diem };
            return dal.ThemMon(mon) ? "OK" : "Lỗi thêm món";
        }

        public string AddDishWithRecipe(MonDTO m, Dictionary<string, decimal> ingredients)
        {
            if (dal.ThemMon(m))
            {
                // Vì SQL dùng Trigger sinh mã (INSTEAD OF INSERT) nên DAL không trả về ID ngay được.
                // Logic workaround: Tìm món vừa thêm bằng cách lấy danh sách món của thực đơn đó,
                // và tìm món có Tên khớp (hoặc ID lớn nhất nếu có thể sắp xếp).

                List<MonDTO> listMon = dal.GetMonByThucDon(m.MaThucDon);
                string newID = null;

                // Lấy món có tên trùng. Nếu có nhiều món cùng tên, lấy cái cuối cùng (giả định là cái mới nhất)
                // Lưu ý: SQL Trigger sinh mã MA-xxx tăng dần, nên string compare có thể đúng tương đối.
                var found = listMon.Where(x => x.Ten == m.Ten).OrderByDescending(x => x.MaMon).FirstOrDefault();

                if (found != null)
                {
                    newID = found.MaMon;
                    dal.UpdateRecipe(newID, ingredients);
                }
                return "Thêm món thành công!";
            }
            return "Lỗi thêm món";
        }

        public string UpdateDishWithRecipe(MonDTO m, Dictionary<string, decimal> ingredients)
        {
            if (dal.SuaMon(m))
            {
                dal.UpdateRecipe(m.MaMon, ingredients);
                return "Cập nhật món thành công!";
            }
            return "Lỗi cập nhật";
        }

        public string DeleteDishSafe(string maMon)
        {
            string check = dal.CheckCanDeleteDish(maMon);
            if (check != "OK") return check; // Trả về lỗi nếu đã có hóa đơn

            return dal.XoaMon(maMon) ? "Xóa món thành công!" : "Lỗi hệ thống khi xóa.";
        }

        public DataTable GetDishRecipe(string maMon) => dal.GetRecipe(maMon);

        // ==========================================================
        // 5. MODULE KHO & NHÀ CUNG CẤP & NHẬP HÀNG
        // ==========================================================
        public DataTable GetInventory() => dal.GetKhoHang();

        // Nguyên liệu
        public string AddNL(string ten, string dv) => dal.ThemNguyenLieu(ten, dv) ? "OK" : "Lỗi (trùng mã?)";
        public string UpdateNL(string ma, string ten, string dv) => dal.UpdateNguyenLieu(ma, ten, dv) ? "OK" : "Lỗi";
        public string DeleteIngredient(string ma) => dal.DeleteNguyenLieu(ma) ? "Xóa thành công" : "Nguyên liệu đang được sử dụng!";

        // Nhà cung cấp
        public DataTable GetListNCC() => dal.GetNhaCungCap();
        public string AddSupplier(string t, string d, string s) => dal.ThemNCC(t, d, s) ? "Thêm NCC thành công" : "Lỗi";
        public string UpdateSupplier(string ma, string t, string d, string s) => dal.SuaNCC(ma, t, d, s) ? "Cập nhật thành công" : "Lỗi";
        public string DeleteSupplier(string ma) => dal.XoaNCC(ma) ? "Xóa thành công" : "NCC đã có giao dịch, không thể xóa!";

        // Phiếu nhập
        public DataTable GetAllImportSlips() => dal.GetAllPhieuNhap();
        public string DeleteImportSlip(string id) => dal.DeleteImportSlip(id) ? "Xóa phiếu thành công" : "Lỗi xóa phiếu";

        public string ImportStock(string maNCC, string maNV, List<ChiTietNhapDTO> items, string tenNCC)
        {
            if (items == null || items.Count == 0) return "Giỏ hàng trống!";

            decimal tongTien = 0;

            // Tính tổng tiền dựa trên DTO mới (LuongThucTe * DonGia)
            foreach (var item in items)
            {
                tongTien += item.LuongThucTe * item.DonGia;
            }

            // Gọi DAL tạo phiếu. Tham số "Đã nhập" (tinhTrangChung) sẽ bị DAL bỏ qua do SQL không có cột này, 
            // nhưng vẫn cần truyền để khớp chữ ký hàm.
            if (dal.CreateImportSlip(maNCC, maNV, tongTien, "Đã nhập", items))
            {
                return "Nhập hàng thành công!";
            }
            return "Lỗi nhập hàng.";
        }

        // ==========================================================
        // 6. MODULE NHÂN SỰ
        // ==========================================================
        public DataTable GetStaff() => dal.GetNhanVien();

        // Lưu ý: Hệ thống hiện tại dùng Trigger trên bảng NguoiDung để tạo NhanVien.
        // DAL hiện tại chưa hỗ trợ phương thức InsertNguoiDung với vai trò Nhân viên/Quản lý một cách trực tiếp
        // (chỉ có InsertKhachHang). Do đó chức năng này tạm thời trả về thông báo.
        public string AddStaff(NhanVienDTO nv)
        {
            return "Chức năng thêm nhân viên cần cập nhật DAL để hỗ trợ thêm tài khoản User mới.";
        }

        public string UpdateStaff(NhanVienDTO nv) => dal.SuaNhanVien(nv.MaNV, nv.Ten, nv.SDT, nv.DiaChi) ? "Cập nhật thành công" : "Lỗi";

        public string DeleteStaff(string id) => dal.XoaNhanVien(id) ? "Xóa thành công" : "Lỗi";
    }
}