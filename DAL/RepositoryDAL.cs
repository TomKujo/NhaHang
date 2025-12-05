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
        // 1. MODULE ĐĂNG NHẬP & QUẢN LÝ USER
        // ==========================================================
        public NguoiDungDTO CheckLogin(string tenDN, string matKhau)
        {
            string query = "SELECT * FROM NguoiDung WHERE TenDN = @user AND MatKhau = @pass AND TrangThai = N'Kích hoạt'";
            DataTable dt = DBHelper.ExecuteQuery(query,
                new SqlParameter("@user", tenDN),
                new SqlParameter("@pass", matKhau));

            if (dt.Rows.Count > 0)
            {
                DataRow r = dt.Rows[0];
                return new NguoiDungDTO
                {
                    MaNguoiDung = r["MaNguoiDung"].ToString(),
                    TenDN = r["TenDN"].ToString(),
                    VaiTro = r["VaiTro"].ToString(),
                    TrangThai = r["TrangThai"].ToString(),
                    SoLanSai = r["SoLanSai"] != DBNull.Value ? Convert.ToInt32(r["SoLanSai"]) : 0
                };
            }
            return null;
        }

        public NguoiDungDTO GetUserByUsername(string u)
        {
            DataTable dt = DBHelper.ExecuteQuery("SELECT * FROM NguoiDung WHERE TenDN = @u", new SqlParameter("@u", u));
            if (dt.Rows.Count == 0) return null;
            DataRow r = dt.Rows[0];
            return new NguoiDungDTO
            {
                MaNguoiDung = r["MaNguoiDung"].ToString(),
                TenDN = r["TenDN"].ToString(),
                MatKhau = r["MatKhau"].ToString(),
                VaiTro = r["VaiTro"].ToString(),
                TrangThai = r["TrangThai"].ToString(),
                SoLanSai = r["SoLanSai"] != DBNull.Value ? Convert.ToInt32(r["SoLanSai"]) : 0
            };
        }

        public void UnlockAccount(string u) => DBHelper.ExecuteNonQuery("UPDATE NguoiDung SET TrangThai = N'Kích hoạt', SoLanSai = 0 WHERE TenDN = @u", new SqlParameter("@u", u));
        public void LockAccount(string u) => DBHelper.ExecuteNonQuery("UPDATE NguoiDung SET TrangThai = N'Khóa' WHERE TenDN = @u", new SqlParameter("@u", u));
        public void UpdateLoginFail(string u, int c) => DBHelper.ExecuteNonQuery("UPDATE NguoiDung SET SoLanSai = @c WHERE TenDN = @u", new SqlParameter("@c", c), new SqlParameter("@u", u));
        public void ResetPass(string u, string p) => DBHelper.ExecuteNonQuery("UPDATE NguoiDung SET MatKhau = @p WHERE TenDN = @u", new SqlParameter("@p", p), new SqlParameter("@u", u));

        public DataTable GetAllStaffFull()
        {
            return DBHelper.ExecuteQuery("SELECT nv.*, nd.TrangThai FROM NhanVien nv JOIN NguoiDung nd ON nv.MaNV = nd.MaNguoiDung");
        }

        // ==========================================================
        // 2. MODULE QUẢN LÝ BÀN & LỊCH ĐẶT
        // ==========================================================
        public List<BanDTO> GetListBan()
        {
            var list = new List<BanDTO>();
            DataTable dt = DBHelper.ExecuteQuery("SELECT * FROM Ban");
            foreach (DataRow r in dt.Rows)
            {
                list.Add(new BanDTO
                {
                    MaBan = r["MaBan"].ToString(),
                    Ten = r["Ten"].ToString(),
                    Loai = r["Loai"].ToString(),
                    TrangThai = r["TrangThai"].ToString(),
                    TienCoc = r["TienCoc"] != DBNull.Value ? Convert.ToDecimal(r["TienCoc"]) : 0
                });
            }
            return list;
        }

        // [TRIGGER FIX]: Truyền MaBan='' để Trigger trg_Ban tự sinh mã
        public bool ThemBan(string ten, string loai)
        {
            return DBHelper.ExecuteNonQuery("INSERT INTO Ban (MaBan, Ten, Loai, TrangThai, TienCoc) VALUES ('', @t, @l, N'Trống', 0)",
                new SqlParameter("@t", ten),
                new SqlParameter("@l", loai)) > 0;
        }

        public bool SuaBan(string ma, string ten, string loai)
        {
            return DBHelper.ExecuteNonQuery("UPDATE Ban SET Ten=@t, Loai=@l WHERE MaBan=@m",
                new SqlParameter("@m", ma),
                new SqlParameter("@t", ten),
                new SqlParameter("@l", loai)) > 0;
        }

        public bool XoaBan(string ma)
        {
            string checkSql = "SELECT COUNT(*) FROM Ban WHERE MaBan=@m AND TrangThai != N'Trống'";
            if ((int)DBHelper.ExecuteScalar(checkSql, new SqlParameter("@m", ma)) > 0)
                return false;

            return DBHelper.ExecuteNonQuery("DELETE FROM Ban WHERE MaBan=@m", new SqlParameter("@m", ma)) > 0;
        }

        // --- Đặt Bàn ---
        // [TRIGGER FIX]: Truyền MaDatBan='' để Trigger trg_DatBan tự sinh
        public bool InsertDatBan(string maBan, string maKH, DateTime thoiGian)
        {
            return DBHelper.ExecuteNonQuery(@"INSERT INTO DatBan (MaDatBan, MaKH, MaBan, ThoiGian, TrangThai) 
                                              VALUES ('', @kh, @mb, @tg, N'Chờ phản hồi')",
                new SqlParameter("@kh", maKH),
                new SqlParameter("@mb", maBan),
                new SqlParameter("@tg", thoiGian)) > 0;
        }

        public DataTable GetBookings(string maBan)
        {
            return DBHelper.ExecuteQuery(@"SELECT d.MaDatBan, d.ThoiGian, k.Ten AS TenKhachHang, k.SDT, d.TrangThai 
                                           FROM DatBan d 
                                           LEFT JOIN KhachHang k ON d.MaKH = k.MaKH 
                                           WHERE d.MaBan = @m 
                                           ORDER BY d.ThoiGian DESC", new SqlParameter("@m", maBan));
        }

        public bool CheckTrungLich(string maBan, DateTime thoiGian, string excludeID = null)
        {
            DateTime start = thoiGian.AddHours(-2);
            DateTime end = thoiGian.AddHours(2);

            string sql = @"SELECT COUNT(*) FROM DatBan 
                           WHERE MaBan=@mb AND TrangThai != N'Hủy' 
                           AND ThoiGian > @start AND ThoiGian < @end";

            List<SqlParameter> p = new List<SqlParameter> {
                new SqlParameter("@mb", maBan),
                new SqlParameter("@start", start),
                new SqlParameter("@end", end)
            };

            if (!string.IsNullOrEmpty(excludeID))
            {
                sql += " AND MaDatBan != @ex";
                p.Add(new SqlParameter("@ex", excludeID));
            }

            return (int)DBHelper.ExecuteScalar(sql, p.ToArray()) > 0;
        }

        public bool UpdateDatBan(string maDatBan, string maBan, DateTime thoiGian)
        {
            return DBHelper.ExecuteNonQuery("UPDATE DatBan SET MaBan=@mb, ThoiGian=@tg WHERE MaDatBan=@id",
                new SqlParameter("@mb", maBan), new SqlParameter("@tg", thoiGian), new SqlParameter("@id", maDatBan)) > 0;
        }

        public bool CancelDatBan(string maDatBan)
        {
            return DBHelper.ExecuteNonQuery("UPDATE DatBan SET TrangThai = N'Hủy' WHERE MaDatBan=@id",
                new SqlParameter("@id", maDatBan)) > 0;
        }

        public DataTable GetAvailableTables(DateTime thoiGian)
        {
            DateTime start = thoiGian.AddHours(-2);
            DateTime end = thoiGian.AddHours(2);

            return DBHelper.ExecuteQuery(@"
                SELECT * FROM Ban 
                WHERE MaBan NOT IN (
                    SELECT MaBan FROM DatBan 
                    WHERE TrangThai != N'Hủy'
                    AND ThoiGian > @start AND ThoiGian < @end
                )", new SqlParameter("@start", start), new SqlParameter("@end", end));
        }

        public void UpdateTableStatus(string maBan, string status)
        {
            DBHelper.ExecuteNonQuery("UPDATE Ban SET TrangThai = @st WHERE MaBan = @mb",
                new SqlParameter("@st", status), new SqlParameter("@mb", maBan));
        }

        // ==========================================================
        // 3. MODULE ORDER & THANH TOÁN
        // ==========================================================
        public string GetUnpaidBillID(string maBan)
        {
            var res = DBHelper.ExecuteQuery("SELECT MaHD FROM HoaDon WHERE MaBan=@m AND TrangThai=N'Chưa thanh toán'", new SqlParameter("@m", maBan));
            return res.Rows.Count > 0 ? res.Rows[0][0].ToString() : null;
        }

        // [TRIGGER FIX]: Truyền MaHD='' để Trigger trg_HoaDon tự sinh
        public void InsertHoaDonWithNote(string maBan, string maKH, string ghiChu)
        {
            // Lưu ý: Bảng HoaDon trong SQL bạn đưa KHÔNG có cột GhiChu, nên mình không insert GhiChu vào đây.
            DBHelper.ExecuteNonQuery(@"INSERT INTO HoaDon (MaHD, MaBan, MaKH, ThoiGianLap, TrangThai, HinhThucThanhToan) 
                                       VALUES ('', @m, @k, GETDATE(), N'Chưa thanh toán', N'Tiền mặt')",
                new SqlParameter("@m", maBan),
                new SqlParameter("@k", (object)maKH ?? DBNull.Value));

            DBHelper.ExecuteNonQuery("UPDATE Ban SET TrangThai=N'Có khách' WHERE MaBan=@m", new SqlParameter("@m", maBan));
        }

        public void InsertChiTietHD(string maHD, string maMon, int sl, string ghiChu)
        {
            decimal gia = (decimal)DBHelper.ExecuteScalar("SELECT Gia FROM Mon WHERE MaMon=@m", new SqlParameter("@m", maMon));

            string sqlInsert = @"INSERT INTO ChiTietHD (MaHD, MaMon, SoLuong, DonGia, GhiChu) 
                                 VALUES (@h, @m, @s, @g, @gc)";

            DBHelper.ExecuteNonQuery(sqlInsert,
                new SqlParameter("@h", maHD),
                new SqlParameter("@m", maMon),
                new SqlParameter("@s", sl),
                new SqlParameter("@g", gia),
                new SqlParameter("@gc", ghiChu));
        }

        public DataTable GetOrderDetails(string maHD)
        {
            return DBHelper.ExecuteQuery(@"SELECT c.ID, c.MaMon, m.Ten AS TenMon, c.SoLuong, m.Gia, 
                                   (c.SoLuong * m.Gia) AS ThanhTien, c.GhiChu 
                                   FROM ChiTietHD c JOIN Mon m ON c.MaMon = m.MaMon 
                                   WHERE c.MaHD=@h", new SqlParameter("@h", maHD));
        }

        public void DeductInventoryOnCheckout(string maHD)
        {
            DataTable dt = GetOrderDetails(maHD);
            foreach (DataRow r in dt.Rows)
            {
                string maMon = r["MaMon"].ToString();
                int slMon = Convert.ToInt32(r["SoLuong"]);

                DataTable congThuc = GetRecipe(maMon);
                foreach (DataRow ct in congThuc.Rows)
                {
                    string maNL = ct["MaNguyenLieu"].ToString();
                    decimal hao = Convert.ToDecimal(ct["LuongTieuHao"]) * slMon;

                    DBHelper.ExecuteNonQuery("UPDATE NguyenLieu SET SoLuongTon = SoLuongTon - @h WHERE MaNguyenLieu=@id",
                        new SqlParameter("@h", hao), new SqlParameter("@id", maNL));
                }
            }
        }

        public void Checkout(string maHD, string maBan, decimal tong)
        {
            DBHelper.ExecuteNonQuery("UPDATE HoaDon SET TrangThai=N'Đã thanh toán', TongTien=@t WHERE MaHD=@h",
                new SqlParameter("@t", tong), new SqlParameter("@h", maHD));
            DBHelper.ExecuteNonQuery("UPDATE Ban SET TrangThai=N'Trống' WHERE MaBan=@b", new SqlParameter("@b", maBan));
        }

        // --- Hỗ trợ Khách hàng & Tích điểm ---
        public string GetKhachHangByHoaDon(string maHD)
        {
            object res = DBHelper.ExecuteScalar("SELECT MaKH FROM HoaDon WHERE MaHD = @ma", new SqlParameter("@ma", maHD));
            return (res != null && res != DBNull.Value) ? res.ToString() : null;
        }

        public void CongDiemTichLuy(string maKH, int diem)
        {
            if (string.IsNullOrEmpty(maKH)) return;
            DBHelper.ExecuteNonQuery("UPDATE KhachHang SET DiemTichLuy = DiemTichLuy + @diem WHERE MaKH = @ma",
                new SqlParameter("@diem", diem), new SqlParameter("@ma", maKH));
        }

        public string GetKhachHangBySDT(string sdt)
        {
            object res = DBHelper.ExecuteScalar("SELECT MaKH FROM KhachHang WHERE SDT = @sdt", new SqlParameter("@sdt", sdt));
            return res != null ? res.ToString() : null;
        }

        // [TRIGGER FIX]: Truyền ID='' để Trigger trg_NguoiDung tự sinh
        public string InsertKhachHang(string ten, string sdt)
        {
            // 1. Insert NguoiDung
            DBHelper.ExecuteNonQuery("INSERT INTO NguoiDung (MaNguoiDung, TenDN, MatKhau, VaiTro, TrangThai) VALUES ('', @sdt, '123456', N'Khách hàng', N'Kích hoạt')",
                new SqlParameter("@sdt", sdt));

            // 2. Lấy lại ID vừa sinh
            string maKH = GetUserByUsername(sdt)?.MaNguoiDung;

            // 3. Insert KhachHang
            if (maKH != null)
            {
                DBHelper.ExecuteNonQuery("INSERT INTO KhachHang (MaKH, Ten, SDT) VALUES (@ma, @ten, @sdt)",
                    new SqlParameter("@ma", maKH), new SqlParameter("@ten", ten), new SqlParameter("@sdt", sdt));
            }
            return maKH;
        }

        // ==========================================================
        // 4. MODULE THỰC ĐƠN, KHO, CÔNG THỨC
        // ==========================================================
        public DataTable GetListThucDon() => DBHelper.ExecuteQuery("SELECT * FROM ThucDon");

        public List<MonDTO> GetMonByThucDon(string maTD)
        {
            var list = new List<MonDTO>();
            DataTable dt = DBHelper.ExecuteQuery(@"SELECT m.*, t.Ten AS TenThucDon 
                                                   FROM Mon m JOIN ThucDon t ON m.MaThucDon = t.MaThucDon 
                                                   WHERE m.MaThucDon = @mtd", new SqlParameter("@mtd", maTD));
            foreach (DataRow r in dt.Rows)
            {
                list.Add(new MonDTO
                {
                    MaMon = r["MaMon"].ToString(),
                    Ten = r["Ten"].ToString(),
                    Gia = Convert.ToDecimal(r["Gia"]),
                    DiemTichLuy = r["DiemTichLuy"] != DBNull.Value ? Convert.ToInt32(r["DiemTichLuy"]) : 0,
                    MaThucDon = r["MaThucDon"].ToString()
                });
            }
            return list;
        }

        public DataTable GetFullMenu() => DBHelper.ExecuteQuery("SELECT MaMon, Ten, Gia FROM Mon");

        // [TRIGGER FIX]: MaThucDon=''
        public bool ThemThucDon(string ten, string moTa)
            => DBHelper.ExecuteNonQuery("INSERT INTO ThucDon (MaThucDon, Ten, MoTa) VALUES ('', @t, @m)",
                new SqlParameter("@t", ten), new SqlParameter("@m", moTa)) > 0;

        public bool SuaThucDon(string ma, string ten, string moTa)
            => DBHelper.ExecuteNonQuery("UPDATE ThucDon SET Ten=@t, MoTa=@m WHERE MaThucDon=@id",
                new SqlParameter("@id", ma), new SqlParameter("@t", ten), new SqlParameter("@m", moTa)) > 0;

        public bool XoaThucDon(string ma)
        {
            if ((int)DBHelper.ExecuteScalar("SELECT COUNT(*) FROM Mon WHERE MaThucDon=@id", new SqlParameter("@id", ma)) > 0)
                return false;
            return DBHelper.ExecuteNonQuery("DELETE FROM ThucDon WHERE MaThucDon=@id", new SqlParameter("@id", ma)) > 0;
        }

        // [TRIGGER FIX]: MaMon=''
        public bool ThemMon(MonDTO m)
            => DBHelper.ExecuteNonQuery("INSERT INTO Mon (MaMon, MaThucDon, Ten, Gia, DiemTichLuy) VALUES ('', @td, @ten, @g, @d)",
                new SqlParameter("@td", m.MaThucDon), new SqlParameter("@ten", m.Ten), new SqlParameter("@g", m.Gia), new SqlParameter("@d", m.DiemTichLuy)) > 0;

        public bool SuaMon(MonDTO m)
            => DBHelper.ExecuteNonQuery("UPDATE Mon SET Ten=@ten, Gia=@g, DiemTichLuy=@d WHERE MaMon=@ma",
                new SqlParameter("@ma", m.MaMon), new SqlParameter("@ten", m.Ten), new SqlParameter("@g", m.Gia), new SqlParameter("@d", m.DiemTichLuy)) > 0;

        public bool XoaMon(string ma)
        {
            DBHelper.ExecuteNonQuery("DELETE FROM CongThuc WHERE MaMon=@ma", new SqlParameter("@ma", ma));
            return DBHelper.ExecuteNonQuery("DELETE FROM Mon WHERE MaMon=@ma", new SqlParameter("@ma", ma)) > 0;
        }

        public string CheckCanDeleteDish(string maMon)
        {
            int count = (int)DBHelper.ExecuteScalar("SELECT COUNT(*) FROM ChiTietHD WHERE MaMon = @ma", new SqlParameter("@ma", maMon));
            return count > 0 ? "Món này đã có trong lịch sử hóa đơn, không thể xóa!" : "OK";
        }

        // --- Công Thức ---
        public DataTable GetRecipe(string maMon)
        {
            return DBHelper.ExecuteQuery(@"SELECT c.MaNguyenLieu, n.Ten AS TenNguyenLieu, c.LuongTieuHao, n.DonVi 
                                           FROM CongThuc c JOIN NguyenLieu n ON c.MaNguyenLieu = n.MaNguyenLieu 
                                           WHERE c.MaMon = @m", new SqlParameter("@m", maMon));
        }

        public void UpdateRecipe(string maMon, Dictionary<string, decimal> ingredients)
        {
            DBHelper.ExecuteNonQuery("DELETE FROM CongThuc WHERE MaMon = @m", new SqlParameter("@m", maMon));
            foreach (var kv in ingredients)
            {
                DBHelper.ExecuteNonQuery("INSERT INTO CongThuc (MaMon, MaNguyenLieu, LuongTieuHao) VALUES (@m, @nl, @sl)",
                    new SqlParameter("@m", maMon), new SqlParameter("@nl", kv.Key), new SqlParameter("@sl", kv.Value));
            }
        }

        // --- Kho Nguyên Liệu ---
        public DataTable GetKhoHang() => DBHelper.ExecuteQuery("SELECT * FROM NguyenLieu");

        // [TRIGGER FIX]: MaNguyenLieu=''
        public bool ThemNguyenLieu(string ten, string donVi)
        {
            return DBHelper.ExecuteNonQuery("INSERT INTO NguyenLieu (MaNguyenLieu, Ten, DonVi, SoLuongTon) VALUES ('', @t, @d, 0)",
                new SqlParameter("@t", ten), new SqlParameter("@d", donVi)) > 0;
        }

        public bool UpdateNguyenLieu(string ma, string ten, string donVi)
            => DBHelper.ExecuteNonQuery("UPDATE NguyenLieu SET Ten=@t, DonVi=@d WHERE MaNguyenLieu=@m",
                new SqlParameter("@t", ten), new SqlParameter("@d", donVi), new SqlParameter("@m", ma)) > 0;

        public bool DeleteNguyenLieu(string ma)
        {
            string check = "SELECT COUNT(*) FROM CongThuc WHERE MaNguyenLieu=@m";
            if ((int)DBHelper.ExecuteScalar(check, new SqlParameter("@m", ma)) > 0) return false;

            check = "SELECT COUNT(*) FROM ChiTietNhap WHERE MaNguyenLieu=@m";
            if ((int)DBHelper.ExecuteScalar(check, new SqlParameter("@m", ma)) > 0) return false;

            return DBHelper.ExecuteNonQuery("DELETE FROM NguyenLieu WHERE MaNguyenLieu=@m", new SqlParameter("@m", ma)) > 0;
        }

        // --- Nhập Hàng ---
        public DataTable GetAllPhieuNhap()
        {
            return DBHelper.ExecuteQuery(@"SELECT p.MaPhieu, p.MaNCC, n.Ten AS TenNCC, p.MaNV, p.ThoiGian, p.TongTien 
                                           FROM PhieuNhapHang p 
                                           LEFT JOIN NhaCungCap n ON p.MaNCC = n.MaNCC 
                                           ORDER BY p.ThoiGian DESC");
        }

        public DataTable GetChiTietPhieuNhap(string maPhieu)
        {
            // Sửa lại Query lấy LuongThucTe thay vì SoLuong
            return DBHelper.ExecuteQuery(@"SELECT c.MaNguyenLieu, n.Ten AS TenNguyenLieu, c.LuongThucTe AS SoLuong, c.LuongYeuCau, c.DonGia, c.TinhTrang
                                           FROM ChiTietNhap c JOIN NguyenLieu n ON c.MaNguyenLieu = n.MaNguyenLieu 
                                           WHERE c.MaPhieu = @m", new SqlParameter("@m", maPhieu));
        }

        // [SQL UPDATE FIX]: Tương thích hoàn toàn với bảng PhieuNhapHang (bỏ TinhTrang) và ChiTietNhap (thêm chi tiết)
        public bool CreateImportSlip(string maNCC, string maNV, decimal tongTien, string tinhTrangChung, List<ChiTietNhapDTO> details)
        {
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    // 1. Insert PhieuNhapHang
                    SqlCommand cmdPhieu = new SqlCommand("INSERT INTO PhieuNhapHang (MaPhieu, MaNCC, MaNV, ThoiGian, TongTien) VALUES ('', @ncc, @nv, GETDATE(), @tt)", conn, tran);
                    cmdPhieu.Parameters.Add(new SqlParameter("@ncc", maNCC));
                    cmdPhieu.Parameters.Add(new SqlParameter("@nv", maNV));
                    cmdPhieu.Parameters.Add(new SqlParameter("@tt", tongTien));
                    cmdPhieu.ExecuteNonQuery();

                    // 2. Lấy mã phiếu vừa sinh
                    SqlCommand cmdGetID = new SqlCommand("SELECT TOP 1 MaPhieu FROM PhieuNhapHang WHERE MaNV = @nv ORDER BY ThoiGian DESC", conn, tran);
                    cmdGetID.Parameters.Add(new SqlParameter("@nv", maNV));
                    object objID = cmdGetID.ExecuteScalar();

                    if (objID == null) throw new Exception("Không lấy được mã phiếu nhập vừa tạo.");
                    string maPhieu = objID.ToString();

                    // 3. Insert ChiTiet
                    foreach (var item in details)
                    {
                        string sqlCT = @"INSERT INTO ChiTietNhap (MaPhieu, MaNguyenLieu, LuongYeuCau, LuongThucTe, DonGia, TinhTrang) 
                                 VALUES (@p, @nl, @lyc, @ltt, @dg, @tt)";

                        SqlCommand cmdCT = new SqlCommand(sqlCT, conn, tran);
                        cmdCT.Parameters.Add(new SqlParameter("@p", maPhieu));
                        cmdCT.Parameters.Add(new SqlParameter("@nl", item.MaNguyenLieu));
                        cmdCT.Parameters.Add(new SqlParameter("@lyc", item.LuongYeuCau));
                        cmdCT.Parameters.Add(new SqlParameter("@ltt", item.LuongThucTe));
                        cmdCT.Parameters.Add(new SqlParameter("@dg", item.DonGia));
                        cmdCT.Parameters.Add(new SqlParameter("@tt", string.IsNullOrEmpty(item.TinhTrang) ? "Đủ" : item.TinhTrang));
                        cmdCT.ExecuteNonQuery();

                        // Update kho
                        SqlCommand cmdKho = new SqlCommand("UPDATE NguyenLieu SET SoLuongTon = SoLuongTon + @sl WHERE MaNguyenLieu = @nl", conn, tran);
                        cmdKho.Parameters.Add(new SqlParameter("@sl", item.LuongThucTe));
                        cmdKho.Parameters.Add(new SqlParameter("@nl", item.MaNguyenLieu));
                        cmdKho.ExecuteNonQuery();
                    }
                    tran.Commit();
                    return true;
                }
                catch (Exception ex) // [SỬA]: Bắt lỗi và Ném ra ngoài (Throw)
                {
                    tran.Rollback();
                    // Ném lỗi ra để BLL và GUI bắt được nội dung lỗi (VD: Conflict khóa ngoại)
                    throw new Exception("Lỗi SQL: " + ex.Message);
                }
            }
        }

        public bool DeleteImportSlip(string maPhieu)
        {
            DBHelper.ExecuteNonQuery("DELETE FROM ChiTietNhap WHERE MaPhieu=@p", new SqlParameter("@p", maPhieu));
            return DBHelper.ExecuteNonQuery("DELETE FROM PhieuNhapHang WHERE MaPhieu=@p", new SqlParameter("@p", maPhieu)) > 0;
        }

        // --- Nhà Cung Cấp ---
        public DataTable GetNhaCungCap() => DBHelper.ExecuteQuery("SELECT * FROM NhaCungCap");

        // [TRIGGER FIX]: MaNCC=''
        public bool ThemNCC(string t, string dc, string s)
            => DBHelper.ExecuteNonQuery("INSERT INTO NhaCungCap (MaNCC, Ten, DiaChi, SDT) VALUES ('', @t, @dc, @s)",
                new SqlParameter("@t", t), new SqlParameter("@dc", dc), new SqlParameter("@s", s)) > 0;

        public bool SuaNCC(string ma, string t, string dc, string s)
            => DBHelper.ExecuteNonQuery("UPDATE NhaCungCap SET Ten=@t, DiaChi=@dc, SDT=@s WHERE MaNCC=@ma",
                new SqlParameter("@ma", ma), new SqlParameter("@t", t), new SqlParameter("@dc", dc), new SqlParameter("@s", s)) > 0;

        public bool XoaNCC(string ma)
        {
            if ((int)DBHelper.ExecuteScalar("SELECT COUNT(*) FROM PhieuNhapHang WHERE MaNCC=@ma", new SqlParameter("@ma", ma)) > 0)
                return false;
            return DBHelper.ExecuteNonQuery("DELETE FROM NhaCungCap WHERE MaNCC=@ma", new SqlParameter("@ma", ma)) > 0;
        }

        // --- Nhân Viên ---
        public DataTable GetNhanVien() => DBHelper.ExecuteQuery("SELECT * FROM NhanVien");

        public bool SuaNhanVien(string ma, string t, string s, string d)
            => DBHelper.ExecuteNonQuery("UPDATE NhanVien SET Ten=@t, SDT=@s, DiaChi=@d WHERE MaNV=@ma",
                new SqlParameter("@ma", ma), new SqlParameter("@t", t), new SqlParameter("@s", s), new SqlParameter("@d", d)) > 0;

        public bool XoaNhanVien(string ma)
            => DBHelper.ExecuteNonQuery("DELETE FROM NhanVien WHERE MaNV=@ma", new SqlParameter("@ma", ma)) > 0;
    }
}