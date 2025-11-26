using DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class RepositoryDAL
    {
        // ==========================================================
        // 1. MODULE ĐĂNG NHẬP
        // ==========================================================
        public NguoiDungDTO CheckLogin(string tenDN, string matKhau)
        {
            string query = "SELECT * FROM NguoiDung WHERE TenDN = @user AND MatKhau = @pass AND TrangThai = N'Kích hoạt'";
            SqlParameter[] param = {
                new SqlParameter("@user", tenDN),
                new SqlParameter("@pass", matKhau)
            };

            DataTable dt = DBHelper.ExecuteQuery(query, param);
            if (dt.Rows.Count > 0)
            {
                DataRow r = dt.Rows[0];
                return new NguoiDungDTO
                {
                    MaNguoiDung = r["MaNguoiDung"].ToString(),
                    TenDN = r["TenDN"].ToString(),
                    VaiTro = r["VaiTro"].ToString(),
                    TrangThai = r["TrangThai"].ToString()
                };
            }
            return null;
        }

        // ==========================================================
        // 2. MODULE QUẢN LÝ BÀN
        // ==========================================================
        public List<BanDTO> GetListBan()
        {
            List<BanDTO> list = new List<BanDTO>();
            DataTable dt = DBHelper.ExecuteQuery("SELECT * FROM Ban");
            foreach (DataRow r in dt.Rows)
            {
                list.Add(new BanDTO
                {
                    MaBan = r["MaBan"].ToString(),
                    TenBan = r["TenBan"].ToString(),
                    Loai = r["Loai"].ToString(),
                    TrangThai = r["TrangThai"].ToString(),
                    TienCoc = r["TienCoc"] != DBNull.Value ? Convert.ToDecimal(r["TienCoc"]) : 0
                });
            }
            return list;
        }

        public bool UpdateTrangThaiBan(string maBan, string trangThai)
        {
            string query = "UPDATE Ban SET TrangThai = @tt WHERE MaBan = @mb";
            return DBHelper.ExecuteNonQuery(query, new SqlParameter[] {
                new SqlParameter("@tt", trangThai),
                new SqlParameter("@mb", maBan)
            }) > 0;
        }

        // ==========================================================
        // 3. MODULE THỰC ĐƠN & MÓN ĂN
        // ==========================================================
        public DataTable GetListThucDon()
        {
            return DBHelper.ExecuteQuery("SELECT * FROM ThucDon");
        }

        public List<MonAnDTO> GetMonAnByThucDon(string maTD)
        {
            List<MonAnDTO> list = new List<MonAnDTO>();
            // Join bảng Món ăn với Thực đơn để lấy tên hiển thị
            string query = @"SELECT m.MaMonAn, m.Gia, m.DiemThuong, t.TenThucDon 
                             FROM MonAn m JOIN ThucDon t ON m.MaThucDon = t.MaThucDon
                             WHERE m.MaThucDon = @mtd";

            DataTable dt = DBHelper.ExecuteQuery(query, new SqlParameter[] { new SqlParameter("@mtd", maTD) });
            foreach (DataRow r in dt.Rows)
            {
                list.Add(new MonAnDTO
                {
                    MaMonAn = r["MaMonAn"].ToString(),
                    Gia = Convert.ToDecimal(r["Gia"]),
                    DiemThuong = Convert.ToInt32(r["DiemThuong"]),
                    // Tạo tên giả lập để hiển thị vì bảng MonAn thiếu cột tên
                    TenMonAn = r["TenThucDon"].ToString() + " (" + r["MaMonAn"].ToString() + ")"
                });
            }
            return list;
        }

        // ==========================================================
        // 4. MODULE ORDER & HÓA ĐƠN
        // ==========================================================

        // Lấy ID hóa đơn chưa thanh toán của bàn (nếu có)
        public string GetUnpaidBillID(string maBan)
        {
            string query = "SELECT MaHoaDon FROM HoaDon WHERE MaBan = @mb AND TrangThai = N'Chưa thanh toán'";
            DataTable dt = DBHelper.ExecuteQuery(query, new SqlParameter[] { new SqlParameter("@mb", maBan) });
            if (dt.Rows.Count > 0)
                return dt.Rows[0][0].ToString();
            return null; // Không có hóa đơn nào đang mở
        }

        // Tạo hóa đơn mới (Trigger DB sẽ tự sinh MaHoaDon)
        public void InsertHoaDon(string maBan, string maKH)
        {
            // Truyền giá trị giả 'AUTO' cho MaHoaDon, Trigger trg_CreateMaHoaDon sẽ xử lý
            string query = @"INSERT INTO HoaDon (MaHoaDon, MaBan, MaKH, ThoiGianLap, TrangThai, ThanhToan, ThanhTien, TongTien) 
                             VALUES ('AUTO', @mb, @kh, GETDATE(), N'Chưa thanh toán', N'Tiền mặt', 0, 0)";

            DBHelper.ExecuteNonQuery(query, new SqlParameter[] {
                new SqlParameter("@mb", maBan),
                new SqlParameter("@kh", maKH)
            });

            // Cập nhật bàn sang trạng thái Có khách
            UpdateTrangThaiBan(maBan, "Có khách");
        }

        // Thêm món vào hóa đơn (Insert hoặc Update số lượng)
        public void InsertChiTietHoaDon(string maHD, string maMon, int soLuong)
        {
            // 1. Lấy giá món hiện tại
            string qGia = "SELECT Gia FROM MonAn WHERE MaMonAn = @ma";
            DataTable dtGia = DBHelper.ExecuteQuery(qGia, new SqlParameter[] { new SqlParameter("@ma", maMon) });
            if (dtGia.Rows.Count == 0) return; // Kiểm tra lỗi nếu món không tồn tại

            decimal gia = Convert.ToDecimal(dtGia.Rows[0]["Gia"]);

            // 2. Kiểm tra món đã tồn tại trong bill chưa
            string qCheck = "SELECT * FROM ChiTietHoaDon WHERE MaHoaDon = @hd AND MaMonAn = @ma";
            DataTable dtCheck = DBHelper.ExecuteQuery(qCheck, new SqlParameter[] {
                new SqlParameter("@hd", maHD), new SqlParameter("@ma", maMon)
            });

            if (dtCheck.Rows.Count > 0)
            {
                // Nếu có rồi -> Cộng dồn số lượng
                string qUpd = @"UPDATE ChiTietHoaDon 
                                SET SoLuong = SoLuong + @sl, ThanhTien = (SoLuong + @sl) * @gia 
                                WHERE MaHoaDon = @hd AND MaMonAn = @ma";
                DBHelper.ExecuteNonQuery(qUpd, new SqlParameter[] {
                    new SqlParameter("@sl", soLuong),
                    new SqlParameter("@gia", gia),
                    new SqlParameter("@hd", maHD),
                    new SqlParameter("@ma", maMon)
                });
            }
            else
            {
                // Nếu chưa có -> Thêm mới
                string qIns = @"INSERT INTO ChiTietHoaDon (MaHoaDon, MaMonAn, SoLuong, ThanhTien) 
                                VALUES (@hd, @ma, @sl, @sl * @gia)";
                DBHelper.ExecuteNonQuery(qIns, new SqlParameter[] {
                    new SqlParameter("@hd", maHD),
                    new SqlParameter("@ma", maMon),
                    new SqlParameter("@sl", soLuong),
                    new SqlParameter("@gia", gia)
                });
            }
        }

        // Lấy danh sách món đã gọi của 1 bàn
        public DataTable GetChiTietHoaDonInfo(string maHD)
        {
            // Join để lấy tên món hiển thị
            string query = @"SELECT ct.MaMonAn, td.TenThucDon + ' - ' + ct.MaMonAn AS TenMonAn, ct.SoLuong, m.Gia, ct.ThanhTien
                             FROM ChiTietHoaDon ct
                             JOIN MonAn m ON ct.MaMonAn = m.MaMonAn
                             JOIN ThucDon td ON m.MaThucDon = td.MaThucDon
                             WHERE ct.MaHoaDon = @hd";
            return DBHelper.ExecuteQuery(query, new SqlParameter[] { new SqlParameter("@hd", maHD) });
        }

        // Thanh toán
        public void Checkout(string maHD, string maBan, decimal tongTien)
        {
            string query = "UPDATE HoaDon SET TrangThai = N'Đã thanh toán', TongTien = @tt WHERE MaHoaDon = @hd";
            DBHelper.ExecuteNonQuery(query, new SqlParameter[] {
                new SqlParameter("@tt", tongTien),
                new SqlParameter("@hd", maHD)
            });

            // Trả bàn về trạng thái Trống
            UpdateTrangThaiBan(maBan, "Trống");
        }

        // ==========================================================
        // 5. MODULE KHO
        // ==========================================================
        public DataTable GetKhoHang()
        {
            return DBHelper.ExecuteQuery("SELECT * FROM ThucPham");
        }

        // --- BỔ SUNG QUẢN LÝ NHÂN VIÊN ---
        public bool ThemNhanVien(string ten, string sdt, string diachi)
        {
            // ID được sinh tự động bởi Trigger trg_CreateMaNhanVien
            // Ta truyền 'AUTO' vào MaNV để thỏa mãn cú pháp Insert
            string query = "INSERT INTO NhanVien (MaNV, TenNV, SDT, DiaChi) VALUES ('AUTO', @ten, @sdt, @dc)";
            return DBHelper.ExecuteNonQuery(query, new System.Data.SqlClient.SqlParameter[] {
                new System.Data.SqlClient.SqlParameter("@ten", ten),
                new System.Data.SqlClient.SqlParameter("@sdt", sdt),
                new System.Data.SqlClient.SqlParameter("@dc", diachi)
            }) > 0;
        }

        public bool SuaNhanVien(string maNV, string ten, string sdt, string diachi)
        {
            string query = "UPDATE NhanVien SET TenNV=@ten, SDT=@sdt, DiaChi=@dc WHERE MaNV=@ma";
            return DBHelper.ExecuteNonQuery(query, new System.Data.SqlClient.SqlParameter[] {
                new System.Data.SqlClient.SqlParameter("@ma", maNV),
                new System.Data.SqlClient.SqlParameter("@ten", ten),
                new System.Data.SqlClient.SqlParameter("@sdt", sdt),
                new System.Data.SqlClient.SqlParameter("@dc", diachi)
            }) > 0;
        }

        public bool XoaNhanVien(string maNV)
        {
            string query = "DELETE FROM NhanVien WHERE MaNV=@ma";
            return DBHelper.ExecuteNonQuery(query, new System.Data.SqlClient.SqlParameter[] {
                new System.Data.SqlClient.SqlParameter("@ma", maNV)
            }) > 0;
        }

        public System.Data.DataTable GetNhanVien() => DBHelper.ExecuteQuery("SELECT * FROM NhanVien");

        // --- BỔ SUNG LOGIC ĐĂNG NHẬP NÂNG CAO ---

        // 1. Lấy thông tin user theo Username (bất kể pass đúng hay sai)
        public NguoiDungDTO GetUserByUsername(string username)
        {
            string query = "SELECT * FROM NguoiDung WHERE TenDN = @u";
            DataTable dt = DBHelper.ExecuteQuery(query, new System.Data.SqlClient.SqlParameter[] {
        new System.Data.SqlClient.SqlParameter("@u", username)
    });

            if (dt.Rows.Count > 0)
            {
                DataRow r = dt.Rows[0];
                return new NguoiDungDTO
                {
                    MaNguoiDung = r["MaNguoiDung"].ToString(),
                    TenDN = r["TenDN"].ToString(),
                    MatKhau = r["MatKhau"].ToString(), // Lưu ý: thực tế nên hash
                    VaiTro = r["VaiTro"].ToString(),
                    TrangThai = r["TrangThai"].ToString(),
                    SoLanSai = r["SoLanSai"] != DBNull.Value ? Convert.ToInt32(r["SoLanSai"]) : 0
                };
            }
            return null;
        }

        // 2. Cập nhật số lần sai
        public void UpdateLoginFail(string username, int count)
        {
            string query = "UPDATE NguoiDung SET SoLanSai = @c WHERE TenDN = @u";
            DBHelper.ExecuteNonQuery(query, new System.Data.SqlClient.SqlParameter[] {
        new System.Data.SqlClient.SqlParameter("@c", count),
        new System.Data.SqlClient.SqlParameter("@u", username)
    });
        }

        // 3. Khóa tài khoản
        public void LockAccount(string username)
        {
            string query = "UPDATE NguoiDung SET TrangThai = N'Khóa' WHERE TenDN = @u";
            DBHelper.ExecuteNonQuery(query, new System.Data.SqlClient.SqlParameter[] {
        new System.Data.SqlClient.SqlParameter("@u", username)
    });
        }

        // 4. Reset khi đăng nhập thành công
        public void ResetLoginFail(string username)
        {
            string query = "UPDATE NguoiDung SET SoLanSai = 0 WHERE TenDN = @u";
            DBHelper.ExecuteNonQuery(query, new System.Data.SqlClient.SqlParameter[] {
        new System.Data.SqlClient.SqlParameter("@u", username)
    });
        }

        // 5. Kiểm tra email để reset mật khẩu (Join bảng NhanVien/KhachHang)
        public bool CheckEmailExist(string email)
        {
            // Kiểm tra trong bảng Khách hàng hoặc Nhân viên (vì User kết nối tới 2 bảng này)
            string query = @"
        SELECT 1 FROM KhachHang WHERE Email = @e
        UNION
        SELECT 1 FROM NhanVien WHERE Email = @e -- Giả sử bảng NV có cột Email (nếu chưa có bạn cần thêm vào DB)
    ";
            // Nếu bảng NhanVien chưa có Email, bạn có thể chỉ check KhachHang hoặc thêm cột Email vào NV

            DataTable dt = DBHelper.ExecuteQuery(query, new System.Data.SqlClient.SqlParameter[] {
        new System.Data.SqlClient.SqlParameter("@e", email)
    });
            return dt.Rows.Count > 0;
        }

        public void ResetPassword(string email, string newPass)
        {
            // Cần query phức tạp hơn để update password dựa trên Email (Join bảng). 
            // Để đơn giản cho bài này, ta giả lập update theo username nhập vào form Quên MK
            // Hoặc update user có liên kết với email đó.
        }
        public void ResetPasswordByUsername(string username, string newPass)
        {
            string query = "UPDATE NguoiDung SET MatKhau = @p, SoLanSai = 0, TrangThai = N'Kích hoạt' WHERE TenDN = @u";
            DBHelper.ExecuteNonQuery(query, new System.Data.SqlClient.SqlParameter[] {
                new System.Data.SqlClient.SqlParameter("@p", newPass),
                new System.Data.SqlClient.SqlParameter("@u", username)
            });
        }

        // --- QUẢN LÝ BÀN (CRUD) ---
        public bool ThemBan(string tenBan, string loai)
        {
            // Trigger trg_CreateMaBan sẽ tự sinh MaBan
            // Mặc định tạo bàn mới là 'Trống'
            string query = "INSERT INTO Ban (MaBan, TenBan, Loai, TrangThai, TienCoc) VALUES ('AUTO', @ten, @loai, N'Trống', 0)";
            return DBHelper.ExecuteNonQuery(query, new System.Data.SqlClient.SqlParameter[] {
        new System.Data.SqlClient.SqlParameter("@ten", tenBan),
        new System.Data.SqlClient.SqlParameter("@loai", loai)
    }) > 0;
        }

        public bool SuaBan(string maBan, string tenBan, string loai)
        {
            string query = "UPDATE Ban SET TenBan=@ten, Loai=@loai WHERE MaBan=@ma";
            return DBHelper.ExecuteNonQuery(query, new System.Data.SqlClient.SqlParameter[] {
        new System.Data.SqlClient.SqlParameter("@ma", maBan),
        new System.Data.SqlClient.SqlParameter("@ten", tenBan),
        new System.Data.SqlClient.SqlParameter("@loai", loai)
    }) > 0;
        }

        public bool XoaBan(string maBan)
        {
            // Kiểm tra bàn có đang có khách hoặc đặt trước không
            string check = "SELECT Count(*) FROM Ban WHERE MaBan=@ma AND TrangThai != N'Trống'";
            int count = (int)DBHelper.ExecuteQuery(check, new System.Data.SqlClient.SqlParameter[] { new System.Data.SqlClient.SqlParameter("@ma", maBan) }).Rows[0][0];
            if (count > 0) return false; // Không cho xóa bàn đang dùng

            string query = "DELETE FROM Ban WHERE MaBan=@ma";
            return DBHelper.ExecuteNonQuery(query, new System.Data.SqlClient.SqlParameter[] { new System.Data.SqlClient.SqlParameter("@ma", maBan) }) > 0;
        }

        // --- ĐẶT BÀN (BOOKING) ---
        public bool DatBanTruoc(string maBan, string tenKhach, string sdt, DateTime gioDen)
        {
            // 1. Kiểm tra bàn có trống vào giờ đó không
            // (Logic đơn giản: Kiểm tra xem có đơn đặt nào trùng ngày giờ chưa)
            // ... (Code check trùng lịch phức tạp, ở đây làm đơn giản trước)

            // 2. Tạo khách hàng vãng lai nếu chưa có (Dựa vào SĐT)
            // ...

            // 3. Insert vào bảng DatBan
            string query = "INSERT INTO DatBan (MaDatBan, MaKH, MaBan, GioDen, NgayDat, TrangThai) VALUES ('AUTO', 'khach1', @mb, @gd, GETDATE(), N'Chờ phản hồi')";
            return DBHelper.ExecuteNonQuery(query, new System.Data.SqlClient.SqlParameter[] {
        new System.Data.SqlClient.SqlParameter("@mb", maBan),
                new System.Data.SqlClient.SqlParameter("@gd", gioDen.TimeOfDay) // Chỉ lấy giờ
            }) > 0;
        }

        // --- QUẢN LÝ THỰC ĐƠN (Categories) ---
        public bool ThemThucDon(string ten, string mota)
        {
            string query = "INSERT INTO ThucDon (MaThucDon, TenThucDon, MoTa) VALUES ('AUTO', @ten, @mota)";
            return DBHelper.ExecuteNonQuery(query, new System.Data.SqlClient.SqlParameter[] {
        new System.Data.SqlClient.SqlParameter("@ten", ten),
        new System.Data.SqlClient.SqlParameter("@mota", mota)
    }) > 0;
        }

        public bool SuaThucDon(string ma, string ten, string mota)
        {
            string query = "UPDATE ThucDon SET TenThucDon=@ten, MoTa=@mota WHERE MaThucDon=@ma";
            return DBHelper.ExecuteNonQuery(query, new System.Data.SqlClient.SqlParameter[] {
        new System.Data.SqlClient.SqlParameter("@ma", ma),
        new System.Data.SqlClient.SqlParameter("@ten", ten),
        new System.Data.SqlClient.SqlParameter("@mota", mota)
    }) > 0;
        }

        public bool XoaThucDon(string ma)
        {
            // Cần xóa món ăn con trước hoặc báo lỗi (Ở đây chọn báo lỗi nếu còn món)
            string check = "SELECT COUNT(*) FROM MonAn WHERE MaThucDon=@ma";
            int count = (int)DBHelper.ExecuteQuery(check, new System.Data.SqlClient.SqlParameter[] { new System.Data.SqlClient.SqlParameter("@ma", ma) }).Rows[0][0];
            if (count > 0) return false;

            string query = "DELETE FROM ThucDon WHERE MaThucDon=@ma";
            return DBHelper.ExecuteNonQuery(query, new System.Data.SqlClient.SqlParameter[] { new System.Data.SqlClient.SqlParameter("@ma", ma) }) > 0;
        }

        // --- QUẢN LÝ MÓN ĂN (Dishes) ---
        public bool ThemMonAn(MonAnDTO mon)
        {
            string query = "INSERT INTO MonAn (MaMonAn, MaThucDon, TenMonAn, Gia, DiemThuong) VALUES ('AUTO', @mtd, @ten, @gia, @diem)";
            return DBHelper.ExecuteNonQuery(query, new System.Data.SqlClient.SqlParameter[] {
        new System.Data.SqlClient.SqlParameter("@mtd", mon.MaThucDon),
        new System.Data.SqlClient.SqlParameter("@ten", mon.TenMonAn),
        new System.Data.SqlClient.SqlParameter("@gia", mon.Gia),
        new System.Data.SqlClient.SqlParameter("@diem", mon.DiemThuong)
    }) > 0;
        }

        public bool SuaMonAn(MonAnDTO mon)
        {
            string query = "UPDATE MonAn SET TenMonAn=@ten, Gia=@gia, DiemThuong=@diem WHERE MaMonAn=@ma";
            return DBHelper.ExecuteNonQuery(query, new System.Data.SqlClient.SqlParameter[] {
        new System.Data.SqlClient.SqlParameter("@ma", mon.MaMonAn),
        new System.Data.SqlClient.SqlParameter("@ten", mon.TenMonAn),
        new System.Data.SqlClient.SqlParameter("@gia", mon.Gia),
        new System.Data.SqlClient.SqlParameter("@diem", mon.DiemThuong)
    }) > 0;
        }

        public bool XoaMonAn(string ma)
        {
            string query = "DELETE FROM MonAn WHERE MaMonAn=@ma";
            return DBHelper.ExecuteNonQuery(query, new System.Data.SqlClient.SqlParameter[] { new System.Data.SqlClient.SqlParameter("@ma", ma) }) > 0;
        }

        // --- TÍCH ĐIỂM KHÁCH HÀNG ---
        public void CongDiemTichLuy(string maKH, int diem)
        {
            string query = "UPDATE KhachHang SET DiemTichLuy = DiemTichLuy + @diem WHERE MaKH = @ma";
            DBHelper.ExecuteNonQuery(query, new System.Data.SqlClient.SqlParameter[] {
        new System.Data.SqlClient.SqlParameter("@diem", diem),
        new System.Data.SqlClient.SqlParameter("@ma", maKH)
    });
        }

        // Lấy MaKH từ MaHoaDon
        public string GetKhachHangByHoaDon(string maHD)
        {
            string query = "SELECT MaKH FROM HoaDon WHERE MaHoaDon = @ma";
            System.Data.DataTable dt = DBHelper.ExecuteQuery(query, new System.Data.SqlClient.SqlParameter[] { new System.Data.SqlClient.SqlParameter("@ma", maHD) });
            if (dt.Rows.Count > 0) return dt.Rows[0][0].ToString();
            return null;
        }

        // Lấy điểm thưởng của món
        public int GetDiemThuongMon(string maMon)
        {
            string query = "SELECT DiemThuong FROM MonAn WHERE MaMonAn = @ma";
            System.Data.DataTable dt = DBHelper.ExecuteQuery(query, new System.Data.SqlClient.SqlParameter[] { new System.Data.SqlClient.SqlParameter("@ma", maMon) });
            if (dt.Rows.Count > 0 && dt.Rows[0][0] != DBNull.Value) return Convert.ToInt32(dt.Rows[0][0]);
            return 0;
        }

        // --- QUẢN LÝ NHÀ CUNG CẤP ---
        public System.Data.DataTable GetNhaCungCap() => DBHelper.ExecuteQuery("SELECT * FROM NhaCungCap");

        public bool ThemNCC(string ten, string dc, string sdt)
        {
            string query = "INSERT INTO NhaCungCap (MaNCC, TenNCC, DiaChi, SDT) VALUES ('AUTO', @ten, @dc, @sdt)";
            return DBHelper.ExecuteNonQuery(query, new System.Data.SqlClient.SqlParameter[] {
                new System.Data.SqlClient.SqlParameter("@ten", ten),
                new System.Data.SqlClient.SqlParameter("@dc", dc),
                new System.Data.SqlClient.SqlParameter("@sdt", sdt)
            }) > 0;
        }
        // (Tương tự cho Sửa/Xóa NCC nếu cần)

        // --- QUẢN LÝ THỰC PHẨM (KHO) ---
        public System.Data.DataTable GetThucPham() => DBHelper.ExecuteQuery("SELECT * FROM ThucPham");

        public bool ThemThucPhamMoi(string ten, string dvt)
        {
            // Tạo mã mới, số lượng ban đầu = 0
            string sql = "INSERT INTO ThucPham (MaThucPham, TenTP, DonViTinh, SoLuongTonKho) VALUES ('AUTO', @ten, @dvt, 0)";
            return DBHelper.ExecuteNonQuery(sql, new System.Data.SqlClient.SqlParameter[] {
        new System.Data.SqlClient.SqlParameter("@ten", ten),
        new System.Data.SqlClient.SqlParameter("@dvt", dvt)
    }) > 0;
        }

        // --- TẠO PHIẾU NHẬP HÀNG (TRANSACTION) ---
        public bool TaoPhieuNhap(string maNCC, string maNV, List<DTO.CartItemDTO> listItems, out string maPNH_New)
        {
            maPNH_New = "";
            using (System.Data.SqlClient.SqlConnection conn = DBHelper.GetConnection())
            {
                conn.Open();
                System.Data.SqlClient.SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // 1. Insert PhieuNhapHang (Trigger tự sinh MaPNH)
                    // Tính tổng tiền
                    decimal tongTien = 0;
                    foreach (var item in listItems) tongTien += item.ThanhTien;

                    // Insert giả định ID='AUTO', Trigger xử lý
                    string sqlPhieu = @"INSERT INTO PhieuNhapHang (MaPNH, MaNCC, MaNguoiNhap, NgayNhapHang, TongTien, TinhTrang) 
                                VALUES ('AUTO', @ncc, @nv, GETDATE(), @tong, N'Đã nhập');";

                    // Để lấy được ID vừa sinh ra bởi Trigger, ta cần logic phức tạp hơn.
                    // Cách đơn giản nhất cho bài này: Insert xong -> Select lại ID mới nhất.

                    System.Data.SqlClient.SqlCommand cmdPhieu = new System.Data.SqlClient.SqlCommand(sqlPhieu, conn, transaction);
                    cmdPhieu.Parameters.AddWithValue("@ncc", maNCC);
                    cmdPhieu.Parameters.AddWithValue("@nv", maNV);
                    cmdPhieu.Parameters.AddWithValue("@tong", tongTien);
                    cmdPhieu.ExecuteNonQuery();

                    // Lấy ID vừa tạo (Logic dựa trên Trigger của bạn: PNH-YYYYMMDD-xxxx)
                    string sqlGetID = "SELECT TOP 1 MaPNH FROM PhieuNhapHang ORDER BY MaPNH DESC";
                    System.Data.SqlClient.SqlCommand cmdGetID = new System.Data.SqlClient.SqlCommand(sqlGetID, conn, transaction);
                    maPNH_New = cmdGetID.ExecuteScalar().ToString();

                    // 2. Insert ChiTiet và Update Kho
                    foreach (var item in listItems)
                    {
                        // Insert Chi Tiet
                        string sqlCT = "INSERT INTO ChiTietNhapHang (MaPNH, MaThucPham, SoLuong, TongTien) VALUES (@pnh, @tp, @sl, @tt)";
                        System.Data.SqlClient.SqlCommand cmdCT = new System.Data.SqlClient.SqlCommand(sqlCT, conn, transaction);
                        cmdCT.Parameters.AddWithValue("@pnh", maPNH_New);
                        cmdCT.Parameters.AddWithValue("@tp", item.MaThucPham);
                        cmdCT.Parameters.AddWithValue("@sl", item.SoLuong);
                        cmdCT.Parameters.AddWithValue("@tt", item.ThanhTien);
                        cmdCT.ExecuteNonQuery();

                        // Update Tồn Kho (Cộng dồn)
                        string sqlUpd = "UPDATE ThucPham SET SoLuongTonKho = SoLuongTonKho + @sl WHERE MaThucPham = @tp";
                        System.Data.SqlClient.SqlCommand cmdUpd = new System.Data.SqlClient.SqlCommand(sqlUpd, conn, transaction);
                        cmdUpd.Parameters.AddWithValue("@sl", item.SoLuong);
                        cmdUpd.Parameters.AddWithValue("@tp", item.MaThucPham);
                        cmdUpd.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    return true;
                }
                catch
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }

        // --- CRUD NHÀ CUNG CẤP ---
        public bool SuaNCC(string ma, string ten, string dc, string sdt)
        {
            string query = "UPDATE NhaCungCap SET TenNCC=@ten, DiaChi=@dc, SDT=@sdt WHERE MaNCC=@ma";
            return DBHelper.ExecuteNonQuery(query, new System.Data.SqlClient.SqlParameter[] {
        new System.Data.SqlClient.SqlParameter("@ma", ma),
        new System.Data.SqlClient.SqlParameter("@ten", ten),
        new System.Data.SqlClient.SqlParameter("@dc", dc),
        new System.Data.SqlClient.SqlParameter("@sdt", sdt)
    }) > 0;
        }

        public bool XoaNCC(string ma)
        {
            // Kiểm tra khóa ngoại: NCC đã có phiếu nhập hàng chưa?
            string check = "SELECT COUNT(*) FROM PhieuNhapHang WHERE MaNCC = @ma";
            int count = (int)DBHelper.ExecuteQuery(check, new System.Data.SqlClient.SqlParameter[] { new System.Data.SqlClient.SqlParameter("@ma", ma) }).Rows[0][0];

            if (count > 0) return false; // Không cho xóa nếu đã từng giao dịch

            string query = "DELETE FROM NhaCungCap WHERE MaNCC=@ma";
            return DBHelper.ExecuteNonQuery(query, new System.Data.SqlClient.SqlParameter[] { new System.Data.SqlClient.SqlParameter("@ma", ma) }) > 0;
        }
    }
}