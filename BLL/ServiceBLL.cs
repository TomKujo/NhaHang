using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO; // Thư viện để ghi file

namespace BLL
{
    public class ServiceBLL
    {
        // Khởi tạo đối tượng DAL để giao tiếp với CSDL
        private RepositoryDAL dal = new RepositoryDAL();

        // ==========================================================
        // 1. NGHIỆP VỤ ĐĂNG NHẬP
        // ==========================================================
        public NguoiDungDTO Login(string username, string password)
        {
            // Có thể thêm logic mã hóa mật khẩu tại đây trước khi gửi xuống DAL
            // Ví dụ: password = MD5Hash(password);

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return null; // Trả về null nếu để trống
            }

            return dal.CheckLogin(username, password);
        }

        // ==========================================================
        // 2. NGHIỆP VỤ QUẢN LÝ BÀN
        // ==========================================================
        public List<BanDTO> GetListBan()
        {
            return dal.GetListBan();
        }

        // ==========================================================
        // 3. NGHIỆP VỤ THỰC ĐƠN & MÓN ĂN
        // ==========================================================
        public DataTable GetListThucDon()
        {
            return dal.GetListThucDon();
        }

        public List<MonAnDTO> GetListMonAn(string maThucDon)
        {
            return dal.GetMonAnByThucDon(maThucDon);
        }

        // ==========================================================
        // 4. NGHIỆP VỤ GỌI MÓN (ORDER) - QUAN TRỌNG
        // ==========================================================

        // Hàm lấy danh sách món đã gọi của bàn để hiển thị lên lưới
        public DataTable GetOrderDetails(string maBan)
        {
            // 1. Tìm hóa đơn chưa thanh toán của bàn này
            string maHD = dal.GetUnpaidBillID(maBan);

            // 2. Nếu chưa có hóa đơn nào -> Trả về null (hoặc bảng rỗng)
            if (maHD == null) return null;

            // 3. Nếu có -> Lấy chi tiết các món ăn
            return dal.GetChiTietHoaDonInfo(maHD);
        }

        // Hàm xử lý logic gọi món: Tự động tạo hóa đơn nếu chưa có
        // --- LOGIC GỌI MÓN (CẬP NHẬT TÍCH ĐIỂM) ---
        public string OrderMon(string maBan, string maMon, int soLuong)
        {
            try
            {
                // 1. Lấy hoặc Tạo hóa đơn
                string maHD = dal.GetUnpaidBillID(maBan);
                if (maHD == null)
                {
                    dal.InsertHoaDon(maBan, "khach1"); // Mặc định khách vãng lai
                    maHD = dal.GetUnpaidBillID(maBan);
                }

                // 2. Thêm món vào hóa đơn
                dal.InsertChiTietHoaDon(maHD, maMon, soLuong);

                // 3. TÍNH ĐIỂM THƯỞNG
                // Lấy mã khách hàng của hóa đơn này
                string maKH = dal.GetKhachHangByHoaDon(maHD);

                // Lấy điểm thưởng của món ăn
                int diemMon = dal.GetDiemThuongMon(maMon);

                // Tính tổng điểm = Điểm món * Số lượng
                int tongDiem = diemMon * soLuong;

                // Cộng điểm nếu có khách hàng và điểm > 0
                if (!string.IsNullOrEmpty(maKH) && tongDiem > 0)
                {
                    dal.CongDiemTichLuy(maKH, tongDiem);
                }

                return $"Gọi món thành công! (Đã cộng {tongDiem} điểm)";
            }
            catch (Exception ex)
            {
                return "Lỗi: " + ex.Message;
            }
        }

        // --- CÁC HÀM CRUD MENU ---
        public string AddCategory(string ten, string mota) => dal.ThemThucDon(ten, mota) ? "Thêm thành công" : "Lỗi thêm";
        public string UpdateCategory(string ma, string ten, string mota) => dal.SuaThucDon(ma, ten, mota) ? "Sửa thành công" : "Lỗi sửa";
        public string DeleteCategory(string ma) => dal.XoaThucDon(ma) ? "Xóa thành công" : "Không thể xóa (có thể còn món ăn)";

        public string AddDish(MonAnDTO mon) => dal.ThemMonAn(mon) ? "Thêm món thành công" : "Lỗi thêm món";
        public string UpdateDish(MonAnDTO mon) => dal.SuaMonAn(mon) ? "Sửa món thành công" : "Lỗi sửa món";
        public string DeleteDish(string ma) => dal.XoaMonAn(ma) ? "Xóa món thành công" : "Lỗi xóa món";

        // ==========================================================
        // 5. NGHIỆP VỤ THANH TOÁN
        // ==========================================================
        public bool Checkout(string maBan, decimal tongTien)
        {
            try
            {
                // BƯỚC 1: Tìm mã hóa đơn (maHD) đang mở của bàn này
                // (Bạn thiếu dòng này nên mới báo lỗi "maHD does not exist")
                string maHD = dal.GetUnpaidBillID(maBan);

                // BƯỚC 2: Kiểm tra nếu tìm thấy hóa đơn thì mới thanh toán
                if (maHD != null)
                {
                    dal.Checkout(maHD, maBan, tongTien);
                    return true; // Thanh toán thành công
                }

                return false; // Không tìm thấy hóa đơn để thanh toán
            }
            catch
            {
                return false; // Lỗi hệ thống
            }
        }

        // ==========================================================
        // 6. NGHIỆP VỤ KHO
        // ==========================================================
        public DataTable GetInventory()
        {
            return dal.GetKhoHang();
        }

        // --- BỔ SUNG LOGIC NHÂN VIÊN ---
        public System.Data.DataTable GetStaff() => dal.GetNhanVien();

        public string AddStaff(NhanVienDTO nv)
        {
            if (string.IsNullOrEmpty(nv.TenNV)) return "Tên không được để trống";
            if (dal.ThemNhanVien(nv.TenNV, nv.SDT, nv.DiaChi)) return "Thêm thành công";
            return "Thêm thất bại";
        }

        public string UpdateStaff(NhanVienDTO nv)
        {
            if (dal.SuaNhanVien(nv.MaNV, nv.TenNV, nv.SDT, nv.DiaChi)) return "Cập nhật thành công";
            return "Lỗi cập nhật";
        }

        public string DeleteStaff(string maNV)
        {
            if (dal.XoaNhanVien(maNV)) return "Xóa thành công";
            return "Lỗi xóa";
        }

        // Định nghĩa kết quả trả về cho rõ ràng
        public enum LoginResult
        {
            Success,
            InvalidCredentials, // Sai pass
            UserNotFound,       // Không tồn tại
            Locked,             // Bị khóa
            CustomerDenied      // Khách hàng không được vào
        }

        public LoginResult LoginAdvanced(string username, string password, out NguoiDungDTO userOut)
        {
            userOut = null;
            NguoiDungDTO user = dal.GetUserByUsername(username);

            // 1. Kiểm tra tồn tại
            if (user == null) return LoginResult.UserNotFound;

            // 2. Kiểm tra vai trò (Khách hàng không được vào Winform)
            if (user.VaiTro == "Khách hàng") return LoginResult.CustomerDenied;

            // 3. Kiểm tra trạng thái khóa
            if (user.TrangThai == "Khóa") return LoginResult.Locked;

            // 4. Kiểm tra mật khẩu
            if (user.MatKhau == password)
            {
                // Đăng nhập thành công -> Reset số lần sai
                dal.ResetLoginFail(username);
                userOut = user;
                return LoginResult.Success;
            }
            else
            {
                // Sai mật khẩu -> Tăng số lần sai
                int newCount = user.SoLanSai + 1;
                dal.UpdateLoginFail(username, newCount);

                if (newCount >= 3)
                {
                    dal.LockAccount(username);
                    return LoginResult.Locked; // Trả về Locked ngay lần sai thứ 3
                }

                return LoginResult.InvalidCredentials;
            }
        }

        // Logic Quên mật khẩu giả lập
        public string ForgotPassword(string username)
        {
            // Trong thực tế: Gửi Email chứa OTP hoặc Link reset.
            // Ở đây giả lập: Nếu user tồn tại -> Reset mật khẩu về "123456"
            NguoiDungDTO user = dal.GetUserByUsername(username);
            if (user != null)
            {
                dal.ResetPasswordByUsername(username, "123456");
                return "Mật khẩu đã được reset về mặc định: 123456";
            }
            return "Tài khoản không tồn tại";
        }

        // --- BLL QUẢN LÝ BÀN ---
        public string AddTable(string tenBan, string loai)
        {
            if (string.IsNullOrEmpty(tenBan)) return "Tên bàn không được trống";
            if (dal.ThemBan(tenBan, loai)) return "Thêm bàn thành công";
            return "Lỗi thêm bàn";
        }

        public string UpdateTable(string maBan, string tenBan, string loai)
        {
            if (dal.SuaBan(maBan, tenBan, loai)) return "Cập nhật thành công";
            return "Lỗi cập nhật";
        }

        public string DeleteTable(string maBan)
        {
            if (dal.XoaBan(maBan)) return "Xóa thành công";
            return "Không thể xóa bàn đang có khách hoặc đặt trước";
        }

        public string BookTable(string maBan, string tenKhach, string sdt, DateTime time)
        {
            // Logic đặt bàn
            if (dal.DatBanTruoc(maBan, tenKhach, sdt, time)) return "Đặt bàn thành công";
            return "Lỗi đặt bàn";
        }

        // --- BLL KHO & NHÀ CUNG CẤP ---
        public System.Data.DataTable GetListNCC() => dal.GetNhaCungCap();

        public System.Data.DataTable GetListIngredient() => dal.GetThucPham();
        public string AddIngredient(string ten, string dvt) => dal.ThemThucPhamMoi(ten, dvt) ? "Thêm thực phẩm thành công" : "Lỗi thêm";

        // --- NHẬP HÀNG & XUẤT BÁO CÁO ---
        // Trong ServiceBLL.cs
        public string ImportStock(string maNCC, string maNV, List<DTO.CartItemDTO> items, string tenNCC)
        {
            if (items.Count == 0) return "Chưa có sản phẩm nào để nhập!";

            string maPNH;
            // 1. Gọi DAL để lưu vào CSDL (Transaction)
            bool result = dal.TaoPhieuNhap(maNCC, maNV, items, out maPNH);

            if (result)
            {
                // 2. NẾU LƯU THÀNH CÔNG -> GỌI HÀM TẠO FILE BÁO CÁO
                ExportImportBill(maPNH, tenNCC, maNV, items);
                return "Nhập hàng thành công! Đã xuất file phiếu nhập tại thư mục Debug.";
            }
            return "Lỗi nhập hàng (Transaction failed).";
        }

        private void ExportImportBill(string maPNH, string tenNCC, string maNV, List<DTO.CartItemDTO> items)
        {
            try
            {
                // File sẽ được tạo tại thư mục bin/Debug của dự án
                string fileName = $"PhieuNhap_{maPNH}.txt";
                using (StreamWriter sw = new StreamWriter(fileName))
                {
                    sw.WriteLine("========== PHIẾU NHẬP KHO ==========");
                    sw.WriteLine($"Mã Phiếu: {maPNH}");
                    sw.WriteLine($"Ngày: {DateTime.Now:dd/MM/yyyy HH:mm}");
                    sw.WriteLine($"NCC: {tenNCC}");
                    sw.WriteLine("------------------------------------------------");
                    sw.WriteLine(string.Format("{0,-20} | {1,5} | {2,12}", "Tên hàng", "SL", "Thành tiền"));
                    sw.WriteLine("------------------------------------------------");

                    decimal tong = 0;
                    foreach (var item in items)
                    {
                        sw.WriteLine(string.Format("{0,-20} | {1,5} | {2,12:N0}",
                            item.TenTP.Length > 20 ? item.TenTP.Substring(0, 17) + "..." : item.TenTP,
                            item.SoLuong,
                            item.ThanhTien));
                        tong += item.ThanhTien;
                    }
                    sw.WriteLine("------------------------------------------------");
                    sw.WriteLine($"TỔNG CỘNG: {tong:N0} VNĐ");
                }
                // Tự động mở file notepad lên xem
                System.Diagnostics.Process.Start("notepad.exe", fileName);
            }
            catch { /* Bỏ qua nếu lỗi file */ }
        }

        // --- BLL NHÀ CUNG CẤP ---
        public string AddSupplier(string ten, string dc, string sdt)
            => dal.ThemNCC(ten, dc, sdt) ? "Thêm NCC thành công" : "Lỗi thêm NCC";

        public string UpdateSupplier(string ma, string ten, string dc, string sdt)
            => dal.SuaNCC(ma, ten, dc, sdt) ? "Cập nhật thành công" : "Lỗi cập nhật";

        public string DeleteSupplier(string ma)
            => dal.XoaNCC(ma) ? "Xóa thành công" : "Không thể xóa NCC đã có giao dịch nhập hàng!";
    }
}