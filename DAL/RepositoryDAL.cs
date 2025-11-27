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
            DataTable dt = DBHelper.ExecuteQuery(query, new SqlParameter[] {
                new SqlParameter("@user", tenDN), new SqlParameter("@pass", matKhau)
            });

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

        public NguoiDungDTO GetUserByUsername(string username)
        {
            string query = "SELECT * FROM NguoiDung WHERE TenDN = @u";
            DataTable dt = DBHelper.ExecuteQuery(query, new SqlParameter[] { new SqlParameter("@u", username) });
            if (dt.Rows.Count > 0)
            {
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
            return null;
        }

        public void UpdateLoginFail(string username, int count)
        {
            DBHelper.ExecuteNonQuery("UPDATE NguoiDung SET SoLanSai = @c WHERE TenDN = @u",
                new SqlParameter[] { new SqlParameter("@c", count), new SqlParameter("@u", username) });
        }

        public void LockAccount(string username)
        {
            DBHelper.ExecuteNonQuery("UPDATE NguoiDung SET TrangThai = N'Khóa' WHERE TenDN = @u",
                new SqlParameter[] { new SqlParameter("@u", username) });
        }

        public void ResetLoginFail(string username)
        {
            DBHelper.ExecuteNonQuery("UPDATE NguoiDung SET SoLanSai = 0 WHERE TenDN = @u",
                new SqlParameter[] { new SqlParameter("@u", username) });
        }

        public void ResetPasswordByUsername(string username, string newPass)
        {
            DBHelper.ExecuteNonQuery("UPDATE NguoiDung SET MatKhau = @p, SoLanSai = 0, TrangThai = N'Kích hoạt' WHERE TenDN = @u",
                new SqlParameter[] { new SqlParameter("@p", newPass), new SqlParameter("@u", username) });
        }

        // ==========================================================
        // 2. MODULE QUẢN LÝ BÀN & LỊCH ĐẶT (Booking)
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
                    TrangThai = r["TrangThai"].ToString()
                });
            }
            return list;
        }

        // CRUD Bàn
        public bool ThemBan(string ten, string loai) => DBHelper.ExecuteNonQuery("INSERT INTO Ban (MaBan, TenBan, Loai, TrangThai, TienCoc) VALUES ('AUTO', @t, @l, N'Trống', 0)", new SqlParameter[] { new SqlParameter("@t", ten), new SqlParameter("@l", loai) }) > 0;

        public bool SuaBan(string ma, string ten, string loai) => DBHelper.ExecuteNonQuery("UPDATE Ban SET TenBan=@t, Loai=@l WHERE MaBan=@m", new SqlParameter[] { new SqlParameter("@m", ma), new SqlParameter("@t", ten), new SqlParameter("@l", loai) }) > 0;

        public bool XoaBan(string ma)
        {
            // Check trước khi xóa
            if ((int)DBHelper.ExecuteQuery("SELECT COUNT(*) FROM Ban WHERE MaBan=@m AND TrangThai != N'Trống'", new SqlParameter[] { new SqlParameter("@m", ma) }).Rows[0][0] > 0) return false;
            return DBHelper.ExecuteNonQuery("DELETE FROM Ban WHERE MaBan=@m", new SqlParameter[] { new SqlParameter("@m", ma) }) > 0;
        }

        // Lịch Đặt Bàn
        public bool InsertDatBan(string maBan, string maKH, DateTime ngay, TimeSpan den, TimeSpan di)
        {
            return DBHelper.ExecuteNonQuery(@"INSERT INTO DatBan (MaDatBan, MaKH, MaBan, NgayDat, GioDen, GioDi, TrangThai) 
                                              VALUES ('AUTO', @kh, @mb, @d, @gd, @gi, N'Đã cọc')",
                new SqlParameter[] { new SqlParameter("@kh", maKH), new SqlParameter("@mb", maBan), new SqlParameter("@d", ngay), new SqlParameter("@gd", den), new SqlParameter("@gi", di) }) > 0;
        }

        public bool CheckTrungLich(string maBan, DateTime ngay, TimeSpan den, TimeSpan di)
        {
            string sql = "SELECT COUNT(*) FROM DatBan WHERE MaBan=@mb AND NgayDat=@d AND TrangThai!=N'Hủy' AND GioDen < @gi AND @gd < GioDi";
            return (int)DBHelper.ExecuteQuery(sql, new SqlParameter[] { new SqlParameter("@mb", maBan), new SqlParameter("@d", ngay), new SqlParameter("@gd", den), new SqlParameter("@gi", di) }).Rows[0][0] > 0;
        }

        public DataTable GetUpcomingBooking(string maBan)
        {
            return DBHelper.ExecuteQuery(@"SELECT TOP 1 * FROM DatBan WHERE MaBan=@mb AND NgayDat=CAST(GETDATE() AS DATE) AND GioDen > CAST(GETDATE() AS TIME) AND TrangThai=N'Đã cọc' ORDER BY GioDen ASC",
                new SqlParameter[] { new SqlParameter("@mb", maBan) });
        }

        public void AutoUpdateTableStatus()
        {
            DBHelper.ExecuteNonQuery(@"UPDATE Ban SET TrangThai = N'Có khách' WHERE MaBan IN (SELECT MaBan FROM DatBan WHERE NgayDat=CAST(GETDATE() AS DATE) AND CAST(GETDATE() AS TIME) BETWEEN GioDen AND GioDi AND TrangThai=N'Đã cọc') AND TrangThai=N'Trống'");
        }

        // ==========================================================
        // 3. MODULE ORDER & THANH TOÁN
        // ==========================================================
        public string GetUnpaidBillID(string maBan)
        {
            DataTable dt = DBHelper.ExecuteQuery("SELECT MaHoaDon FROM HoaDon WHERE MaBan=@mb AND TrangThai=N'Chưa thanh toán'", new SqlParameter[] { new SqlParameter("@mb", maBan) });
            return dt.Rows.Count > 0 ? dt.Rows[0][0].ToString() : null;
        }

        public void InsertHoaDon(string maBan, string maKH)
        {
            DBHelper.ExecuteNonQuery("INSERT INTO HoaDon (MaHoaDon, MaBan, MaKH, ThoiGianLap, TrangThai, ThanhToan) VALUES ('AUTO', @mb, @kh, GETDATE(), N'Chưa thanh toán', N'Tiền mặt')",
                new SqlParameter[] { new SqlParameter("@mb", maBan), new SqlParameter("@kh", maKH) });

            // Tự động chuyển trạng thái bàn sang 'Có khách'
            DBHelper.ExecuteNonQuery("UPDATE Ban SET TrangThai = N'Có khách' WHERE MaBan = @mb", new SqlParameter[] { new SqlParameter("@mb", maBan) });
        }

        public void InsertChiTietHoaDon(string maHD, string maMon, int sl)
        {
            // Lấy giá hiện tại
            decimal gia = Convert.ToDecimal(DBHelper.ExecuteQuery("SELECT Gia FROM MonAn WHERE MaMonAn=@ma", new SqlParameter[] { new SqlParameter("@ma", maMon) }).Rows[0][0]);

            // Kiểm tra tồn tại để Update hay Insert
            if (DBHelper.ExecuteQuery("SELECT * FROM ChiTietHoaDon WHERE MaHoaDon=@hd AND MaMonAn=@ma", new SqlParameter[] { new SqlParameter("@hd", maHD), new SqlParameter("@ma", maMon) }).Rows.Count > 0)
                DBHelper.ExecuteNonQuery("UPDATE ChiTietHoaDon SET SoLuong=SoLuong+@sl, ThanhTien=(SoLuong+@sl)*@gia WHERE MaHoaDon=@hd AND MaMonAn=@ma",
                    new SqlParameter[] { new SqlParameter("@sl", sl), new SqlParameter("@gia", gia), new SqlParameter("@hd", maHD), new SqlParameter("@ma", maMon) });
            else
                DBHelper.ExecuteNonQuery("INSERT INTO ChiTietHoaDon (MaHoaDon, MaMonAn, SoLuong, ThanhTien) VALUES (@hd, @ma, @sl, @sl*@gia)",
                    new SqlParameter[] { new SqlParameter("@hd", maHD), new SqlParameter("@ma", maMon), new SqlParameter("@sl", sl), new SqlParameter("@gia", gia) });
        }

        public void Checkout(string maHD, string maBan, decimal tong)
        {
            DBHelper.ExecuteNonQuery("UPDATE HoaDon SET TrangThai=N'Đã thanh toán', TongTien=@t WHERE MaHoaDon=@hd", new SqlParameter[] { new SqlParameter("@t", tong), new SqlParameter("@hd", maHD) });
            // Trả bàn về Trống
            DBHelper.ExecuteNonQuery("UPDATE Ban SET TrangThai=N'Trống' WHERE MaBan=@mb", new SqlParameter[] { new SqlParameter("@mb", maBan) });
        }

        public DataTable GetChiTietHoaDonInfo(string maHD)
        {
            return DBHelper.ExecuteQuery(@"SELECT ct.MaMonAn, m.TenMonAn, ct.SoLuong, m.Gia, ct.ThanhTien FROM ChiTietHoaDon ct JOIN MonAn m ON ct.MaMonAn = m.MaMonAn WHERE ct.MaHoaDon = @hd",
                new SqlParameter[] { new SqlParameter("@hd", maHD) });
        }

        // Tích điểm
        public string GetKhachHangByHoaDon(string maHD)
        {
            DataTable dt = DBHelper.ExecuteQuery("SELECT MaKH FROM HoaDon WHERE MaHoaDon = @ma", new SqlParameter[] { new SqlParameter("@ma", maHD) });
            return dt.Rows.Count > 0 ? dt.Rows[0][0].ToString() : null;
        }

        public int GetDiemThuongMon(string maMon)
        {
            DataTable dt = DBHelper.ExecuteQuery("SELECT DiemThuong FROM MonAn WHERE MaMonAn = @ma", new SqlParameter[] { new SqlParameter("@ma", maMon) });
            return (dt.Rows.Count > 0 && dt.Rows[0][0] != DBNull.Value) ? Convert.ToInt32(dt.Rows[0][0]) : 0;
        }

        public void CongDiemTichLuy(string maKH, int diem)
        {
            DBHelper.ExecuteNonQuery("UPDATE KhachHang SET DiemTichLuy = DiemTichLuy + @diem WHERE MaKH = @ma", new SqlParameter[] { new SqlParameter("@diem", diem), new SqlParameter("@ma", maKH) });
        }

        // ==========================================================
        // 4. MODULE THỰC ĐƠN, KHO, NHÂN VIÊN
        // ==========================================================
        public DataTable GetListThucDon() => DBHelper.ExecuteQuery("SELECT * FROM ThucDon");

        public List<MonAnDTO> GetMonAnByThucDon(string maTD)
        {
            List<MonAnDTO> list = new List<MonAnDTO>();
            DataTable dt = DBHelper.ExecuteQuery(@"SELECT m.MaMonAn, m.Gia, m.DiemThuong, t.TenThucDon, m.TenMonAn 
                                                   FROM MonAn m JOIN ThucDon t ON m.MaThucDon = t.MaThucDon WHERE m.MaThucDon = @mtd",
                                                   new SqlParameter[] { new SqlParameter("@mtd", maTD) });
            foreach (DataRow r in dt.Rows)
            {
                list.Add(new MonAnDTO
                {
                    MaMonAn = r["MaMonAn"].ToString(),
                    Gia = Convert.ToDecimal(r["Gia"]),
                    DiemThuong = Convert.ToInt32(r["DiemThuong"]),
                    TenMonAn = r["TenMonAn"].ToString() // Đã cập nhật cột tên món
                });
            }
            return list;
        }

        // CRUD Thực đơn & Món
        public bool ThemThucDon(string ten, string mt) => DBHelper.ExecuteNonQuery("INSERT INTO ThucDon (MaThucDon, TenThucDon, MoTa) VALUES ('AUTO', @t, @m)", new SqlParameter[] { new SqlParameter("@t", ten), new SqlParameter("@m", mt) }) > 0;
        public bool SuaThucDon(string ma, string ten, string mt) => DBHelper.ExecuteNonQuery("UPDATE ThucDon SET TenThucDon=@t, MoTa=@m WHERE MaThucDon=@id", new SqlParameter[] { new SqlParameter("@id", ma), new SqlParameter("@t", ten), new SqlParameter("@m", mt) }) > 0;
        public bool XoaThucDon(string ma)
        {
            if ((int)DBHelper.ExecuteQuery("SELECT COUNT(*) FROM MonAn WHERE MaThucDon=@id", new SqlParameter[] { new SqlParameter("@id", ma) }).Rows[0][0] > 0) return false;
            return DBHelper.ExecuteNonQuery("DELETE FROM ThucDon WHERE MaThucDon=@id", new SqlParameter[] { new SqlParameter("@id", ma) }) > 0;
        }

        public bool ThemMonAn(MonAnDTO m) => DBHelper.ExecuteNonQuery("INSERT INTO MonAn (MaMonAn, MaThucDon, TenMonAn, Gia, DiemThuong) VALUES ('AUTO', @td, @ten, @g, @d)",
            new SqlParameter[] { new SqlParameter("@td", m.MaThucDon), new SqlParameter("@ten", m.TenMonAn), new SqlParameter("@g", m.Gia), new SqlParameter("@d", m.DiemThuong) }) > 0;

        public bool SuaMonAn(MonAnDTO m) => DBHelper.ExecuteNonQuery("UPDATE MonAn SET TenMonAn=@ten, Gia=@g, DiemThuong=@d WHERE MaMonAn=@ma",
            new SqlParameter[] { new SqlParameter("@ma", m.MaMonAn), new SqlParameter("@ten", m.TenMonAn), new SqlParameter("@g", m.Gia), new SqlParameter("@d", m.DiemThuong) }) > 0;

        public bool XoaMonAn(string ma) => DBHelper.ExecuteNonQuery("DELETE FROM MonAn WHERE MaMonAn=@ma", new SqlParameter[] { new SqlParameter("@ma", ma) }) > 0;

        // Kho & NCC & Nhân viên
        public DataTable GetKhoHang() => DBHelper.ExecuteQuery("SELECT * FROM ThucPham");
        public bool ThemThucPhamMoi(string ten, string dvt) => DBHelper.ExecuteNonQuery("INSERT INTO ThucPham (MaThucPham, TenTP, DonViTinh, SoLuongTonKho) VALUES ('AUTO', @t, @d, 0)", new SqlParameter[] { new SqlParameter("@t", ten), new SqlParameter("@d", dvt) }) > 0;

        public DataTable GetNhaCungCap() => DBHelper.ExecuteQuery("SELECT * FROM NhaCungCap");
        public bool ThemNCC(string t, string dc, string s) => DBHelper.ExecuteNonQuery("INSERT INTO NhaCungCap (MaNCC, TenNCC, DiaChi, SDT) VALUES ('AUTO', @t, @dc, @s)", new SqlParameter[] { new SqlParameter("@t", t), new SqlParameter("@dc", dc), new SqlParameter("@s", s) }) > 0;
        public bool SuaNCC(string ma, string t, string dc, string s) => DBHelper.ExecuteNonQuery("UPDATE NhaCungCap SET TenNCC=@t, DiaChi=@dc, SDT=@s WHERE MaNCC=@ma", new SqlParameter[] { new SqlParameter("@ma", ma), new SqlParameter("@t", t), new SqlParameter("@dc", dc), new SqlParameter("@s", s) }) > 0;
        public bool XoaNCC(string ma)
        {
            if ((int)DBHelper.ExecuteQuery("SELECT COUNT(*) FROM PhieuNhapHang WHERE MaNCC=@ma", new SqlParameter[] { new SqlParameter("@ma", ma) }).Rows[0][0] > 0) return false;
            return DBHelper.ExecuteNonQuery("DELETE FROM NhaCungCap WHERE MaNCC=@ma", new SqlParameter[] { new SqlParameter("@ma", ma) }) > 0;
        }

        public DataTable GetNhanVien() => DBHelper.ExecuteQuery("SELECT * FROM NhanVien");
        public bool ThemNhanVien(string t, string s, string d) => DBHelper.ExecuteNonQuery("INSERT INTO NhanVien (MaNV, TenNV, SDT, DiaChi) VALUES ('AUTO', @t, @s, @d)", new SqlParameter[] { new SqlParameter("@t", t), new SqlParameter("@s", s), new SqlParameter("@d", d) }) > 0;
        public bool SuaNhanVien(string ma, string t, string s, string d) => DBHelper.ExecuteNonQuery("UPDATE NhanVien SET TenNV=@t, SDT=@s, DiaChi=@d WHERE MaNV=@ma", new SqlParameter[] { new SqlParameter("@ma", ma), new SqlParameter("@t", t), new SqlParameter("@s", s), new SqlParameter("@d", d) }) > 0;
        public bool XoaNhanVien(string ma) => DBHelper.ExecuteNonQuery("DELETE FROM NhanVien WHERE MaNV=@ma", new SqlParameter[] { new SqlParameter("@ma", ma) }) > 0;

        // Transaction Nhập hàng
        public bool TaoPhieuNhap(string maNCC, string maNV, List<DTO.CartItemDTO> items, out string maPNH)
        {
            maPNH = "";
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    decimal tong = 0; foreach (var i in items) tong += i.ThanhTien;

                    // Insert Phiếu
                    new SqlCommand("INSERT INTO PhieuNhapHang (MaPNH, MaNCC, MaNguoiNhap, NgayNhapHang, TongTien, TinhTrang) VALUES ('AUTO', @ncc, @nv, GETDATE(), @t, N'Đã nhập')", conn, tran)
                    {
                        Parameters = { new SqlParameter("@ncc", maNCC), new SqlParameter("@nv", maNV), new SqlParameter("@t", tong) }
                    }.ExecuteNonQuery();

                    maPNH = new SqlCommand("SELECT TOP 1 MaPNH FROM PhieuNhapHang ORDER BY MaPNH DESC", conn, tran).ExecuteScalar().ToString();

                    foreach (var i in items)
                    {
                        new SqlCommand("INSERT INTO ChiTietNhapHang VALUES (@p, @tp, @sl, @tt)", conn, tran)
                        {
                            Parameters = { new SqlParameter("@p", maPNH), new SqlParameter("@tp", i.MaThucPham), new SqlParameter("@sl", i.SoLuong), new SqlParameter("@tt", i.ThanhTien) }
                        }.ExecuteNonQuery();

                        new SqlCommand("UPDATE ThucPham SET SoLuongTonKho = SoLuongTonKho + @sl WHERE MaThucPham = @tp", conn, tran)
                        {
                            Parameters = { new SqlParameter("@sl", i.SoLuong), new SqlParameter("@tp", i.MaThucPham) }
                        }.ExecuteNonQuery();
                    }
                    tran.Commit(); return true;
                }
                catch { tran.Rollback(); return false; }
            }
        }
    }
}