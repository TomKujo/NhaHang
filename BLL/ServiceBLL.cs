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
        public RepositoryDAL dal = new RepositoryDAL();

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

        public bool Checkout(string maBan, string hinhThucTT, out string resultMessage)
        {
            try
            {
                string maHD = dal.GetUnpaidBillID(maBan);

                if (maHD != null)
                {
                    try
                    {
                        dal.DeductInventoryOnCheckout(maHD);
                    }
                    catch (Exception ex)
                    {
                        // Nếu lỗi trừ kho (ví dụ lỗi SQL), dừng thanh toán và báo lỗi
                        resultMessage = "Lỗi cập nhật kho hàng: " + ex.Message;
                        return false;
                    }
                    DataTable dt = dal.ExecuteCheckout(maHD, hinhThucTT); // <--- HÀM QUAN TRỌNG

                    if (dt.Rows.Count > 0)
                    {
                        resultMessage = dt.Rows[0]["KetQua"].ToString();
                        return true;
                    }
                    resultMessage = "Lỗi hệ thống khi thực thi thanh toán.";
                    return false;
                }
                resultMessage = "Không tìm thấy hóa đơn chưa thanh toán.";
                return false;
            }
            catch (Exception ex)
            {
                resultMessage = "Lỗi thanh toán: " + ex.Message;
                return false;
            }
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
        // File: ServiceBLL.cs

        // ... (giữ nguyên code)

        // ==========================================================
        // 6. MODULE NHÂN SỰ
        // ==========================================================
        public DataTable GetAllStaffFull() => dal.GetAllStaffFull();

        public string AddStaff(NhanVienDTO nv)
        {
            if (dal.GetUserByUsername(nv.SDT) != null)
            {
                return "Lỗi: Số điện thoại này đã được dùng làm Tên đăng nhập!";
            }

            string maNV = dal.InsertStaffUser(nv.SDT, "123456", nv.VaiTro);

            if (string.IsNullOrEmpty(maNV)) return "Lỗi hệ thống khi tạo tài khoản người dùng!";

            nv.MaNV = maNV;
            return dal.InsertNhanVien(nv.MaNV, nv.Ten, nv.SDT, nv.DiaChi, nv.Luong, nv.Email)
                ? "Thêm nhân viên thành công!"
                : "Lỗi hệ thống khi thêm thông tin nhân viên.";
        }

        public string UpdateStaff(NhanVienDTO nv)
        {
            bool nvSuccess = dal.SuaNhanVien(nv.MaNV, nv.Ten, nv.SDT, nv.DiaChi, nv.Luong, nv.Email);
            bool userSuccess = dal.UpdateUserRoleAndStatus(nv.MaNV, nv.VaiTro, nv.TrangThaiTK);

            if (nvSuccess && userSuccess) return "Cập nhật thành công!";
            return "Lỗi cập nhật.";
        }

        public string DeleteStaff(string id) => dal.XoaNhanVien(id) ? "Xóa thành công" : "Lỗi";

        // ==========================================================
        // 7. MODULE KHUYẾN MÃI & ĐỔI ĐIỂM (BỔ SUNG)
        // ==========================================================
        public List<KhuyenMaiDTO> GetListKhuyenMai() => dal.GetListKhuyenMai();

        public string AddKhuyenMai(KhuyenMaiDTO km)
        {
            if (km.GiaTriGiam <= 0 || km.DiemCanThiet <= 0) return "Giá trị giảm và điểm yêu cầu phải lớn hơn 0!";
            // Ngày kết thúc cho phép NULL (không hết hạn), nhưng nếu có giá trị thì phải lớn hơn hiện tại
            if (km.NgayKetThuc.HasValue && km.NgayKetThuc.Value <= DateTime.Now) return "Ngày kết thúc không hợp lệ!";

            return dal.InsertKhuyenMai(km) ? "Thêm khuyến mãi thành công!" : "Lỗi thêm khuyến mãi.";
        }

        public string UpdateKhuyenMai(KhuyenMaiDTO km)
        {
            if (km.GiaTriGiam <= 0 || km.DiemCanThiet <= 0) return "Giá trị giảm và điểm yêu cầu phải lớn hơn 0!";
            if (km.NgayKetThuc.HasValue && km.NgayKetThuc.Value <= DateTime.Now) return "Ngày kết thúc không hợp lệ!";

            return dal.UpdateKhuyenMai(km) ? "Cập nhật khuyến mãi thành công!" : "Lỗi cập nhật khuyến mãi.";
        }

        public string DeleteKhuyenMai(string maKM)
        {
            return dal.DeleteKhuyenMai(maKM) ? "Xóa khuyến mãi thành công!" : "Không thể xóa khuyến mãi đã có giao dịch/sử dụng!";
        }

        public int GetDiemTichLuy(string maKH) => dal.GetDiemTichLuy(maKH);

        public string UpdateMaKMToBill(string maHD, string maKH, string maKM)
        {
            // Nếu chưa có MaKH, tìm/tạo khách hàng qua SĐT
            if (maKH == null)
            {
                // Logic này cần được gọi trước khi mở form thanh toán nếu muốn lưu lại khách hàng mới
                // Tạm thời, ta chỉ gán MaKM vào hóa đơn. MaKH sẽ được gán nếu đã có sẵn.
            }

            // Gán MaKM và MaKH vào Hóa đơn (MaKH có thể là null nếu là khách vãng lai không SĐT)
            return dal.UpdateBillMaKM(maHD, maKM) ? "OK" : "Lỗi gán khuyến mãi";
        }

        // Hàm MỚI: Trừ điểm khách hàng và tạo giao dịch Đổi Điểm sau khi thanh toán thành công
        public string DeductPointOnUse(string maHD, string maKH, string maKM, int diemDung)
        {
            // Cần kiểm tra điểm lần nữa để đảm bảo tính toàn vẹn (nhưng Trigger đã làm việc này)
            // Thay vào đó, ta chỉ cần ghi nhận giao dịch Đổi Điểm.

            // Lưu ý: Trigger trg_TruDiemTichLuy trong SQL đã tự động trừ điểm khi Insert vào DoiDiem
            if (dal.InsertDoiDiem(maKH, maKM, diemDung))
            {
                return "Trừ điểm thành công!";
            }
            return "Lỗi ghi nhận giao dịch đổi điểm.";
        }

        public List<BaoCaoDTO> GetReport(int year)
        {
            return dal.GetBaoCaoTaiChinh(year);
        }

        // ==========================================================
        // 8. MODULE TÀI KHOẢN CÁ NHÂN (MỚI)
        // ==========================================================

        // Lấy thông tin chi tiết nhân viên (bao gồm Lương) dựa trên Mã NV
        public NhanVienDTO GetStaffDetail(string maNV)
        {
            DataTable dt = dal.GetUserInfoFull(maNV);

            if (dt.Rows.Count > 0)
            {
                DataRow r = dt.Rows[0];
                return new NhanVienDTO
                {
                    MaNV = r["MaNV"].ToString(),
                    Ten = r["Ten"] != DBNull.Value ? r["Ten"].ToString() : "",
                    SDT = r["SDT"] != DBNull.Value ? r["SDT"].ToString() : "",
                    DiaChi = r["DiaChi"] != DBNull.Value ? r["DiaChi"].ToString() : "",
                    Luong = r["Luong"] != DBNull.Value ? Convert.ToDecimal(r["Luong"]) : 0,
                    VaiTro = r["VaiTro"].ToString(),
                    TrangThaiTK = r["TrangThai"].ToString()
                };
            }
            return null;
        }

        // Cập nhật thông tin cá nhân (chỉ địa chỉ)
        public string UpdatePersonalAddress(string maNV, string diaChiMoi)
        {
            NhanVienDTO nv = GetStaffDetail(maNV);
            if (nv != null)
            {
                nv.DiaChi = diaChiMoi;
                // Gọi hàm UpdateStaff (hàm này update bảng NhanVien)
                return UpdateStaff(nv);
            }
            return "Không tìm thấy thông tin nhân viên để cập nhật.";
        }

        // Đổi mật khẩu
        public string ChangePassword(string username, string newPass)
        {
            dal.ResetPass(username, newPass);
            return "Đổi mật khẩu thành công!";
        }

        // Giả lập gửi OTP
        public string SendOTP(string sdt)
        {
            // Trong thực tế, gọi API SMS ở đây.
            // Demo: Random 6 số
            Random r = new Random();
            string otp = r.Next(100000, 999999).ToString();
            return otp;
        }
    }
}