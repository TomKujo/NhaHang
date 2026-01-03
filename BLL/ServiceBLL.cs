using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;

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
            if (user.VaiTro == "Khách hàng") return LoginResult.CustomerDenied;
            if (user.TrangThai == "Khóa") return LoginResult.Locked;
            string passHash = EncryptMD5(password);
            if (user.MatKhau == passHash)
            {
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

        // ==========================================================
        // 2. MODULE QUẢN LÝ BÀN & ĐẶT LỊCH
        // ==========================================================
        public List<BanDTO> GetListBan() => dal.GetListBan();

        public string AddTable(string t, string l) => dal.ThemBan(t, l) ? "OK" : "Fail";
        public string UpdateTable(string m, string t, string l) => dal.SuaBan(m, t, l) ? "Sửa thành công" : "Lỗi sửa bàn";
        public string DeleteTable(string m) => dal.XoaBan(m) ? "Xóa thành công" : "Không thể xóa bàn đang có khách/đặt trước!";
        public string UpdateTableState(string maBan, string state)
        {
            dal.UpdateTableStatus(maBan, state);
            return "Cập nhật trạng thái thành công";
        }

        public string BookTableSmart(string maBan, string tenKhach, string sdt, DateTime thoiGian)
        {
            if (thoiGian < DateTime.Now) return "Thời gian đặt phải lớn hơn hiện tại!";

            if (dal.CheckTrungLich(maBan, thoiGian)) return "Bàn này đã bị trùng lịch!";

            return dal.InsertDatBan(maBan, tenKhach, sdt, thoiGian) ? "Đặt bàn thành công!" : "Lỗi hệ thống.";
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

        public bool Checkout(string maBan, string hinhThucTT, out string resultMessage, out int pointsAdded)
        {
            pointsAdded = 0;
            try
            {
                string maHD = dal.GetUnpaidBillID(maBan);

                if (maHD != null)
                {
                    try { dal.DeductInventoryOnCheckout(maHD); }
                    catch (Exception ex)
                    {
                        resultMessage = "Lỗi cập nhật kho: " + ex.Message;
                        return false;
                    }

                    DataTable dt = dal.ExecuteCheckout(maHD, hinhThucTT);

                    if (dt.Rows.Count > 0)
                    {
                        resultMessage = dt.Rows[0]["KetQua"].ToString();
                        if (dt.Columns.Contains("DiemDuocCong") && dt.Rows[0]["DiemDuocCong"] != DBNull.Value)
                        {
                            pointsAdded = Convert.ToInt32(dt.Rows[0]["DiemDuocCong"]);
                        }
                        return true;
                    }
                    resultMessage = "Lỗi hệ thống khi thanh toán.";
                    return false;
                }
                resultMessage = "Không tìm thấy hóa đơn.";
                return false;
            }
            catch (Exception ex)
            {
                resultMessage = "Lỗi thanh toán: " + ex.Message;
                return false;
            }
        }

        public string UpdateDishStatus(int idChiTiet, string trangThai)
        {
            return dal.UpdateChiTietTrangThai(idChiTiet, trangThai) ? "Cập nhật thành công" : "Lỗi cập nhật";
        }

        // ==========================================================
        // 4. MODULE THỰC ĐƠN & MÓN
        // ==========================================================
        public DataTable GetListThucDon() => dal.GetListThucDon();
        public List<MonDTO> GetListMonAn(string maTD) => dal.GetMonByThucDon(maTD);
        public DataTable GetFullMenu() => dal.GetFullMenu();
        public string AddCategory(string ten, string mt) => dal.ThemThucDon(ten, mt) ? "Thêm thành công" : "Lỗi";
        public string UpdateCategory(string id, string ten, string mt) => dal.SuaThucDon(id, ten, mt) ? "Sửa thành công" : "Lỗi";
        public string DeleteCategory(string id) => dal.XoaThucDon(id) ? "Xóa thành công" : "Không thể xóa (còn món ăn)";

        public string AddDishWithRecipe(MonDTO m, Dictionary<string, decimal> ingredients)
        {
            if (dal.ThemMon(m))
            {
                List<MonDTO> listMon = dal.GetMonByThucDon(m.MaThucDon);
                string newID = null;

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
            if (check != "OK") return check;
            return dal.XoaMon(maMon) ? "Xóa món thành công!" : "Lỗi hệ thống khi xóa.";
        }

        public DataTable GetDishRecipe(string maMon) => dal.GetRecipe(maMon);

        // ==========================================================
        // 5. MODULE KHO & NHÀ CUNG CẤP & NHẬP HÀNG
        // ==========================================================
        public DataTable GetInventory() => dal.GetKhoHang();

        public string AddNL(string ten, string dv) => dal.ThemNguyenLieu(ten, dv) ? "OK" : "Lỗi (trùng mã?)";
        public string UpdateNL(string ma, string ten, string dv) => dal.UpdateNguyenLieu(ma, ten, dv) ? "OK" : "Lỗi";
        public string DeleteIngredient(string ma) => dal.DeleteNguyenLieu(ma) ? "Xóa thành công" : "Nguyên liệu đang được sử dụng!";

        public DataTable GetListNCC() => dal.GetNhaCungCap();
        public string AddSupplier(string t, string d, string s) => dal.ThemNCC(t, d, s) ? "Thêm NCC thành công" : "Lỗi";
        public string UpdateSupplier(string ma, string t, string d, string s) => dal.SuaNCC(ma, t, d, s) ? "Cập nhật thành công" : "Lỗi";
        public string DeleteSupplier(string ma) => dal.XoaNCC(ma) ? "Xóa thành công" : "NCC đã có giao dịch, không thể xóa!";

        public DataTable GetAllImportSlips() => dal.GetAllPhieuNhap();
        public string DeleteImportSlip(string id) => dal.DeleteImportSlip(id) ? "Xóa phiếu thành công" : "Lỗi xóa phiếu";

        public string ImportStock(string maNCC, string maNV, List<ChiTietNhapDTO> items, string tenNCC)
        {
            if (items == null || items.Count == 0) return "Giỏ hàng trống!";

            decimal tongTien = 0;

            foreach (var item in items)
            {
                tongTien += item.LuongThucTe * item.DonGia;
            }

            if (dal.CreateImportSlip(maNCC, maNV, tongTien, "Đã nhập", items))
            {
                return "Nhập hàng thành công!";
            }
            return "Lỗi nhập hàng.";
        }

        // ==========================================================
        // 6. MODULE NHÂN SỰ
        // ==========================================================
        public DataTable GetAllStaffFull() => dal.GetAllStaffFull();

        public string AddStaff(NhanVienDTO nv)
        {
            if (dal.GetUserByUsername(nv.Email) != null)
            {
                return "Lỗi: Email này đã được dùng làm Tên đăng nhập!";
            }

            string defaultPassHash = EncryptMD5("123456");
            string maNV = dal.InsertStaffUser(nv.Email, defaultPassHash, nv.VaiTro);

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
            if (km.GiaTriGiam <= 0 || km.DiemCan <= 0) return "Giá trị giảm và điểm yêu cầu phải lớn hơn 0!";
            if (km.NgayKT.HasValue && km.NgayKT.Value <= DateTime.Now) return "Ngày kết thúc không hợp lệ!";

            return dal.InsertKhuyenMai(km) ? "Thêm khuyến mãi thành công!" : "Lỗi thêm khuyến mãi.";
        }

        public string UpdateKhuyenMai(KhuyenMaiDTO km)
        {
            if (km.GiaTriGiam <= 0 || km.DiemCan <= 0) return "Giá trị giảm và điểm yêu cầu phải lớn hơn 0!";
            if (km.NgayKT.HasValue && km.NgayKT.Value <= DateTime.Now) return "Ngày kết thúc không hợp lệ!";

            return dal.UpdateKhuyenMai(km) ? "Cập nhật khuyến mãi thành công!" : "Lỗi cập nhật khuyến mãi.";
        }

        public string DeleteKhuyenMai(string maKM)
        {
            return dal.DeleteKhuyenMai(maKM) ? "Xóa khuyến mãi thành công!" : "Không thể xóa khuyến mãi đã có giao dịch/sử dụng!";
        }

        public int GetDiemTichLuy(string maKH) => dal.GetDiemTichLuy(maKH);

        public string UpdateMaKMToBill(string maHD, string maKH, string maKM)
        {
            if (maKH == null)
            {
            }

            return dal.UpdateBillMaKM(maHD, maKM) ? "OK" : "Lỗi gán khuyến mãi";
        }

        public string DeductPointOnUse(string maHD, string maKH, string maKM, int diemDung)
        {
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
        // 8. MODULE TÀI KHOẢN CÁ NHÂN
        // ==========================================================
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
                    Email = r["Email"] != DBNull.Value ? r["Email"].ToString() : "",
                    Luong = r["Luong"] != DBNull.Value ? Convert.ToDecimal(r["Luong"]) : 0,
                    VaiTro = r["VaiTro"].ToString(),
                    TrangThaiTK = r["TrangThai"].ToString()
                };
            }
            return null;
        }

        public string ChangePassword(string username, string newPass)
        {
            string newPassHash = EncryptMD5(newPass);
            dal.ResetPass(username, newPassHash);
            return "Đổi mật khẩu thành công!";
        }

        public string GenerateOTP()
        {
            Random r = new Random();
            return r.Next(100000, 999999).ToString();
        }

        public string SendRealEmail(string toEmail, string otpCode)
        {
            try
            {
                string fromEmail = "vnphtom1@gmail.com";
                string password = "zaatvmwuudbkonsh";
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(fromEmail);
                mail.To.Add(toEmail);
                mail.Subject = "Mã xác thực đổi mật khẩu - Quản Lý Nhà Hàng";
                mail.Body = $"Mã OTP của bạn là: {otpCode}\nMã này có hiệu lực trong 5 phút. Vui lòng không chia sẻ cho ai.";
                mail.IsBodyHtml = false;

                SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                smtp.EnableSsl = true;
                smtp.Port = 587;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(fromEmail, password);
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                smtp.Send(mail);
                return "OK";
            }
            catch (Exception ex)
            {
                return "Lỗi gửi mail: " + ex.Message + "\n" + ex.InnerException?.Message;
            }
        }

        public string CheckEmailAndSendOTP(string username, string inputEmail, out string generatedOTP)
        {
            generatedOTP = "";

            string dbEmail = dal.GetEmailByUsername(username);

            if (string.IsNullOrEmpty(dbEmail))
            {
                return "Tài khoản này chưa đăng ký Email trong hệ thống. Vui lòng liên hệ Admin.";
            }

            generatedOTP = GenerateOTP();
            return SendRealEmail(dbEmail, generatedOTP);
        }

        public DataTable FindCustomers(string keyword, string type)
        {
            if (string.IsNullOrWhiteSpace(keyword)) return dal.GetListKhachHang();
            return dal.SearchKhachHang(keyword, type);
        }

        private string EncryptMD5(string sToEncrypt)
        {
            StringBuilder sBuilder = new StringBuilder();
            MD5 md5Hasher = MD5.Create();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(sToEncrypt));
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
    }
}