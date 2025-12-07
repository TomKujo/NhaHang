USE master;
GO

-- 1. CLEANUP & INIT DATABASE
IF EXISTS (SELECT * FROM sys.databases WHERE name = 'NhaHang')
BEGIN
    ALTER DATABASE NhaHang SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE NhaHang;
END
GO

CREATE DATABASE NhaHang;
GO
USE NhaHang;
GO

-- =============================================
-- 2. TẠO BẢNG (GỐC + KHUYẾN MÃI/ĐỔI ĐIỂM)
-- =============================================

CREATE TABLE NguoiDung (
    MaNguoiDung VARCHAR(20) PRIMARY KEY,
    TenDN VARCHAR(255) UNIQUE NOT NULL,
    MatKhau VARCHAR(255) NOT NULL,
    TrangThai NVARCHAR(10) DEFAULT N'Kích hoạt',
    VaiTro NVARCHAR(10),
    SoLanSai INT DEFAULT 0
);

CREATE TABLE KhachHang (
    MaKH VARCHAR(20) PRIMARY KEY,
    Ten NVARCHAR(255),
    SDT VARCHAR(10),
    Email VARCHAR(255),
    DiemTichLuy INT DEFAULT 0,
    FOREIGN KEY (MaKH) REFERENCES NguoiDung(MaNguoiDung)
);

CREATE TABLE NhanVien (
    MaNV VARCHAR(20) PRIMARY KEY,
    Ten NVARCHAR(255),
    DiaChi NVARCHAR(255),
    SDT VARCHAR(10),
	Email VARCHAR(255),
	Luong DECIMAL(18,2) DEFAULT 0,
	FOREIGN KEY (MaNV) REFERENCES NguoiDung(MaNguoiDung)
);

CREATE TABLE QuanLy (
    MaQL VARCHAR(20) PRIMARY KEY,
    Ten NVARCHAR(255),
    SDT VARCHAR(10) UNIQUE,
    FOREIGN KEY (MaQL) REFERENCES NguoiDung(MaNguoiDung)
);

CREATE TABLE Ban (
    MaBan VARCHAR(20) PRIMARY KEY,
    Loai NVARCHAR(10) NOT NULL DEFAULT N'Thường',
    Ten NVARCHAR(255),
    TienCoc DECIMAL(18,2) DEFAULT 0,
    TrangThai NVARCHAR(255) DEFAULT N'Trống'
);

CREATE TABLE DatBan (
    MaDatBan VARCHAR(20) PRIMARY KEY,
    MaKH VARCHAR(20),
    MaBan VARCHAR(20),
    ThoiGian DATETIME NOT NULL,
    TrangThai NVARCHAR(15) DEFAULT N'Chờ phản hồi',
    FOREIGN KEY (MaKH) REFERENCES KhachHang(MaKH),
    FOREIGN KEY (MaBan) REFERENCES Ban(MaBan)
);

CREATE TABLE ThucDon (
    MaThucDon VARCHAR(20) PRIMARY KEY,
    Ten NVARCHAR(255),
    MoTa NVARCHAR(255)
);

CREATE TABLE Mon (
    MaMon VARCHAR(20) PRIMARY KEY,
    MaThucDon VARCHAR(20) NOT NULL,
    Ten NVARCHAR(255),
    Gia DECIMAL(18,2),
    DiemTichLuy INT DEFAULT 0,
    FOREIGN KEY (MaThucDon) REFERENCES ThucDon(MaThucDon)
);

CREATE TABLE KhuyenMai (
    MaKM VARCHAR(20) PRIMARY KEY,
    TenKM NVARCHAR(255) NOT NULL,
    MoTa NVARCHAR(255),
    DiemCanThiet INT NOT NULL, 
    GiaTriGiam DECIMAL(18, 2), 
    LoaiGiam NVARCHAR(10) DEFAULT N'Tiền mặt', 
    NgayBatDau DATETIME DEFAULT GETDATE(),
    NgayKetThuc DATETIME,
    TrangThai NVARCHAR(255) DEFAULT N'Hoạt động'
);

CREATE TABLE DoiDiem (
    MaGiaoDich VARCHAR(20) PRIMARY KEY,
    MaKH VARCHAR(20) NOT NULL,
    MaKM VARCHAR(20) NOT NULL,
    ThoiGianDoi DATETIME DEFAULT GETDATE(),
    DiemDaDung INT NOT NULL, 
    TrangThai NVARCHAR(10) DEFAULT N'Thành công',
    FOREIGN KEY (MaKH) REFERENCES KhachHang(MaKH),
    FOREIGN KEY (MaKM) REFERENCES KhuyenMai(MaKM)
);

CREATE TABLE HoaDon (
    MaHD VARCHAR(20) PRIMARY KEY,
    MaBan VARCHAR(20),
    MaKH VARCHAR(20),
    MaKM VARCHAR(20), 
    ThoiGianLap DATETIME DEFAULT GETDATE(),
    TongTien DECIMAL(18,2) DEFAULT 0,
    TrangThai NVARCHAR(20) DEFAULT N'Chưa thanh toán',
    HinhThucThanhToan NVARCHAR(20) DEFAULT N'Tiền mặt',
    FOREIGN KEY (MaBan) REFERENCES Ban(MaBan),
    FOREIGN KEY (MaKM) REFERENCES KhuyenMai(MaKM)
);

CREATE TABLE ChiTietHD (
	ID INT IDENTITY(1,1) PRIMARY KEY,
    MaHD VARCHAR(20),
    MaMon VARCHAR(20),
    SoLuong INT DEFAULT 1,
    DonGia DECIMAL(18,2) DEFAULT 0,
    GhiChu NVARCHAR(255),
    FOREIGN KEY (MaHD) REFERENCES HoaDon(MaHD),
    FOREIGN KEY (MaMon) REFERENCES Mon(MaMon)
);

CREATE TABLE NguyenLieu (
    MaNguyenLieu VARCHAR(20) PRIMARY KEY,
    Ten NVARCHAR(255),
    SoLuongTon DECIMAL(18, 2) DEFAULT 0,
    DonVi VARCHAR(5)
);

CREATE TABLE NhaCungCap (
    MaNCC VARCHAR(20) PRIMARY KEY,
    Ten NVARCHAR(255),
    DiaChi NVARCHAR(255),
    SDT VARCHAR(10)
);

CREATE TABLE PhieuNhapHang (
    MaPhieu VARCHAR(20) PRIMARY KEY,
    MaNCC VARCHAR(20),
    MaNV VARCHAR(20),
    ThoiGian DATETIME DEFAULT GETDATE(),
    TongTien DECIMAL(18,2),
    FOREIGN KEY (MaNCC) REFERENCES NhaCungCap(MaNCC),
	FOREIGN KEY (MaNV) REFERENCES NguoiDung(MaNguoiDung)
);

CREATE TABLE ChiTietNhap (
    MaPhieu VARCHAR(20),
    MaNguyenLieu VARCHAR(20),
    LuongYeuCau DECIMAL(18,2),
	LuongThucTe DECIMAL(18,2),
	DonGia DECIMAL(18,2),
	TinhTrang NVARCHAR(10) DEFAULT N'Đủ',
    PRIMARY KEY (MaPhieu, MaNguyenLieu),
    FOREIGN KEY (MaNguyenLieu) REFERENCES NguyenLieu(MaNguyenLieu)
);

CREATE TABLE CongThuc (
    MaMon VARCHAR(20),
    MaNguyenLieu VARCHAR(20),
    LuongTieuHao DECIMAL(18,2),
    PRIMARY KEY (MaMon, MaNguyenLieu),
    FOREIGN KEY (MaMon) REFERENCES Mon(MaMon),
    FOREIGN KEY (MaNguyenLieu) REFERENCES NguyenLieu(MaNguyenLieu)
);
GO

-- =============================================
-- 3. XÂY DỰNG TRIGGERS 
-- =============================================

-- 3.1. Trigger NguoiDung
CREATE TRIGGER trg_NguoiDung ON NguoiDung
INSTEAD OF INSERT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Today CHAR(8) = CONVERT(CHAR(8), GETDATE(), 112);
    
    DECLARE @MaxQL INT = ISNULL((SELECT MAX(CAST(SUBSTRING(MaNguoiDung, 4, 10) AS INT)) FROM NguoiDung WHERE MaNguoiDung LIKE 'QL-%'), 0);
    DECLARE @MaxNV INT = ISNULL((SELECT MAX(CAST(SUBSTRING(MaNguoiDung, 4, 10) AS INT)) FROM NguoiDung WHERE MaNguoiDung LIKE 'NV-%'), 0);
    DECLARE @MaxKH INT = ISNULL((SELECT MAX(CAST(RIGHT(MaNguoiDung, 4) AS INT)) FROM NguoiDung WHERE MaNguoiDung LIKE 'KH-' + @Today + '-%'), 0);

    ;WITH SourceData AS (
        SELECT *,
            CASE 
                WHEN VaiTro = N'Quản lý' THEN 'QL'
                WHEN VaiTro = N'Khách hàng' THEN 'KH'
                ELSE 'NV' 
            END AS Prefix,
            ROW_NUMBER() OVER (PARTITION BY 
                CASE 
                    WHEN VaiTro = N'Quản lý' THEN 'QL'
                    WHEN VaiTro = N'Khách hàng' THEN 'KH'
                    ELSE 'NV' 
                END ORDER BY (SELECT NULL)) AS RN
        FROM inserted
    )
    INSERT INTO NguoiDung (MaNguoiDung, TenDN, MatKhau, TrangThai, VaiTro, SoLanSai)
    SELECT
        CASE 
            WHEN Prefix = 'QL' THEN 'QL-' + RIGHT('000' + CAST(@MaxQL + RN AS VARCHAR(10)), 3)
            WHEN Prefix = 'KH' THEN 'KH-' + @Today + '-' + RIGHT('0000' + CAST(@MaxKH + RN AS VARCHAR(10)), 4)
            ELSE 'NV-' + RIGHT('000' + CAST(@MaxNV + RN AS VARCHAR(10)), 3)
        END,
        TenDN, MatKhau, ISNULL(TrangThai, N'Kích hoạt'), VaiTro, ISNULL(SoLanSai, 0)
    FROM SourceData;
END;
GO

-- 3.2. Trigger Ban
CREATE TRIGGER trg_Ban ON Ban
INSTEAD OF INSERT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Max INT = ISNULL((SELECT MAX(CAST(SUBSTRING(MaBan, 5, 10) AS INT)) FROM Ban WHERE MaBan LIKE 'BAN-%'), 0);

    ;WITH SourceData AS (SELECT *, ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS RN FROM inserted)
    INSERT INTO Ban (MaBan, Loai, Ten, TienCoc, TrangThai)
    SELECT 
        'BAN-' + RIGHT('000' + CAST(@Max + RN AS VARCHAR(10)), 3),
        ISNULL(Loai, N'Thường'), Ten, ISNULL(TienCoc, 0), ISNULL(TrangThai, N'Trống')
    FROM SourceData;
END;
GO

-- 3.3. Trigger ThucDon
CREATE TRIGGER trg_ThucDon ON ThucDon
INSTEAD OF INSERT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Max INT = ISNULL((SELECT MAX(CAST(SUBSTRING(MaThucDon, 4, 10) AS INT)) FROM ThucDon WHERE MaThucDon LIKE 'TD-%'), 0);

    ;WITH SourceData AS (SELECT *, ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS RN FROM inserted)
    INSERT INTO ThucDon (MaThucDon, Ten, MoTa)
    SELECT 'TD-' + RIGHT('000' + CAST(@Max + RN AS VARCHAR(10)), 3), Ten, MoTa
    FROM SourceData;
END;
GO

-- 3.4. Trigger Mon
CREATE TRIGGER trg_Mon ON Mon
INSTEAD OF INSERT
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 FROM inserted WHERE MaThucDon IS NULL)
    BEGIN
        RAISERROR(N'Lỗi: Chưa có Mã Thực Đơn.', 16, 1);
        RETURN;
    END
    
    DECLARE @Max INT = ISNULL((SELECT MAX(CAST(SUBSTRING(MaMon, 4, 10) AS INT)) FROM Mon WHERE MaMon LIKE 'MA-%'), 0);

    ;WITH SourceData AS (SELECT *, ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS RN FROM inserted)
    INSERT INTO Mon (MaMon, MaThucDon, Ten, Gia, DiemTichLuy)
    SELECT 'MA-' + RIGHT('000' + CAST(@Max + RN AS VARCHAR(10)), 3), MaThucDon, Ten, Gia, ISNULL(DiemTichLuy, 0)
    FROM SourceData;
END;
GO

-- 3.5. Trigger NguyenLieu
CREATE TRIGGER trg_NguyenLieu ON NguyenLieu
INSTEAD OF INSERT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Max INT = ISNULL((SELECT MAX(CAST(SUBSTRING(MaNguyenLieu, 4, 10) AS INT)) FROM NguyenLieu WHERE MaNguyenLieu LIKE 'NL-%'), 0);

    ;WITH SourceData AS (SELECT *, ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS RN FROM inserted)
    INSERT INTO NguyenLieu (MaNguyenLieu, Ten, SoLuongTon, DonVi)
    SELECT 'NL-' + RIGHT('000' + CAST(@Max + RN AS VARCHAR(10)), 3), Ten, ISNULL(SoLuongTon, 0), DonVi
    FROM SourceData;
END;
GO

-- 3.6. Trigger NhaCungCap
CREATE TRIGGER trg_NhaCungCap ON NhaCungCap
INSTEAD OF INSERT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Max INT = ISNULL((SELECT MAX(CAST(SUBSTRING(MaNCC, 5, 10) AS INT)) FROM NhaCungCap WHERE MaNCC LIKE 'NCC-%'), 0);

    ;WITH SourceData AS (SELECT *, ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS RN FROM inserted)
    INSERT INTO NhaCungCap (MaNCC, Ten, DiaChi, SDT)
    SELECT 'NCC-' + RIGHT('000' + CAST(@Max + RN AS VARCHAR(10)), 3), Ten, DiaChi, SDT
    FROM SourceData;
END;
GO

-- 3.7. Trigger DatBan
CREATE TRIGGER trg_DatBan ON DatBan
INSTEAD OF INSERT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Today CHAR(8) = CONVERT(CHAR(8), GETDATE(), 112);
    DECLARE @Max INT = ISNULL((SELECT MAX(CAST(RIGHT(MaDatBan, 4) AS INT)) FROM DatBan WHERE MaDatBan LIKE 'DB-' + @Today + '-%'), 0);

    ;WITH SourceData AS (SELECT *, ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS RN FROM inserted)
    INSERT INTO DatBan (MaDatBan, MaKH, MaBan, ThoiGian, TrangThai)
    SELECT 
        'DB-' + @Today + '-' + RIGHT('0000' + CAST(@Max + RN AS VARCHAR(10)), 4),
        MaKH, MaBan, ISNULL(ThoiGian, GETDATE()), ISNULL(TrangThai, N'Thành công')
    FROM SourceData;
END;
GO

-- 3.8. Trigger HoaDon
CREATE TRIGGER trg_HoaDon ON HoaDon
INSTEAD OF INSERT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Today CHAR(8) = CONVERT(CHAR(8), GETDATE(), 112);
    DECLARE @Max INT = ISNULL((SELECT MAX(CAST(RIGHT(MaHD, 4) AS INT)) FROM HoaDon WHERE MaHD LIKE 'HD-' + @Today + '-%'), 0);

    ;WITH SourceData AS (SELECT *, ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS RN FROM inserted)
    INSERT INTO HoaDon (MaHD, MaBan, MaKH, MaKM, ThoiGianLap, TongTien, TrangThai, HinhThucThanhToan)
    SELECT 
        'HD-' + @Today + '-' + RIGHT('0000' + CAST(@Max + RN AS VARCHAR(10)), 4),
        MaBan, MaKH, MaKM, ISNULL(ThoiGianLap, GETDATE()), ISNULL(TongTien, 0), 
        ISNULL(TrangThai, N'Chưa thanh toán'), ISNULL(HinhThucThanhToan, N'Tiền mặt')
    FROM SourceData;
END;
GO

-- 3.9. Trigger PhieuNhapHang
CREATE TRIGGER trg_PhieuNhapHang ON PhieuNhapHang
INSTEAD OF INSERT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Today CHAR(8) = CONVERT(CHAR(8), GETDATE(), 112);
    DECLARE @Max INT = ISNULL((SELECT MAX(CAST(RIGHT(MaPhieu, 4) AS INT)) FROM PhieuNhapHang WHERE MaPhieu LIKE 'PN-' + @Today + '-%'), 0);

    ;WITH SourceData AS (SELECT *, ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS RN FROM inserted)
    INSERT INTO PhieuNhapHang (MaPhieu, MaNCC, MaNV, ThoiGian, TongTien)
    SELECT 
        'PN-' + @Today + '-' + RIGHT('0000' + CAST(@Max + RN AS VARCHAR(10)), 4),
        MaNCC, MaNV, ISNULL(ThoiGian, GETDATE()), ISNULL(TongTien, 0)
    FROM SourceData;
END;
GO

-- 3.10. Trigger KhuyenMai
CREATE TRIGGER trg_KhuyenMai ON KhuyenMai
INSTEAD OF INSERT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Max INT = ISNULL((SELECT MAX(CAST(SUBSTRING(MaKM, 4, 10) AS INT)) FROM KhuyenMai WHERE MaKM LIKE 'KM-%'), 0);

    ;WITH SourceData AS (SELECT *, ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS RN FROM inserted)
    INSERT INTO KhuyenMai (MaKM, TenKM, MoTa, DiemCanThiet, GiaTriGiam, LoaiGiam, NgayBatDau, NgayKetThuc, TrangThai)
    SELECT 
        'KM-' + RIGHT('000' + CAST(@Max + RN AS VARCHAR(10)), 3),
        TenKM, MoTa, DiemCanThiet, GiaTriGiam, ISNULL(LoaiGiam, N'Tiền mặt'), 
        ISNULL(NgayBatDau, GETDATE()), NgayKetThuc, ISNULL(TrangThai, N'Hoạt động')
    FROM SourceData;
END;
GO

-- 3.11. Trigger DoiDiem
CREATE TRIGGER trg_DoiDiem ON DoiDiem
INSTEAD OF INSERT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Today CHAR(8) = CONVERT(CHAR(8), GETDATE(), 112);
    DECLARE @Max INT = ISNULL((SELECT MAX(CAST(RIGHT(MaGiaoDich, 4) AS INT)) FROM DoiDiem WHERE MaGiaoDich LIKE 'DD-' + @Today + '-%'), 0);

    ;WITH SourceData AS (SELECT *, ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS RN FROM inserted)
    INSERT INTO DoiDiem (MaGiaoDich, MaKH, MaKM, ThoiGianDoi, DiemDaDung, TrangThai)
    SELECT 
        'DD-' + @Today + '-' + RIGHT('0000' + CAST(@Max + RN AS VARCHAR(10)), 4),
        MaKH, MaKM, ISNULL(ThoiGianDoi, GETDATE()), DiemDaDung, ISNULL(TrangThai, N'Thành công')
    FROM SourceData;
END;
GO

-- 3.12. Trigger TRỪ ĐIỂM TÍCH LŨY khi khách hàng đổi khuyến mãi thành công
CREATE TRIGGER trg_TruDiemTichLuy ON DoiDiem
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    
    IF EXISTS (SELECT 1 FROM inserted)
    BEGIN
        UPDATE KH
        SET DiemTichLuy = KH.DiemTichLuy - I.DiemDaDung
        FROM KhachHang KH
        INNER JOIN inserted I ON KH.MaKH = I.MaKH
        WHERE I.TrangThai = N'Thành công';
    END
END;
GO


-- =============================================
-- 4. STRORED PROCEDURE (THANH TOÁN & ÁP DỤNG KM)
-- =============================================

IF OBJECT_ID('USP_ThanhToanHoaDon', 'P') IS NOT NULL
    DROP PROCEDURE USP_ThanhToanHoaDon;
GO

CREATE PROCEDURE USP_ThanhToanHoaDon
    @MaHD VARCHAR(20),
    @HinhThucThanhToan NVARCHAR(20) = N'Tiền mặt'
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @TongTienGoc DECIMAL(18, 2);
    DECLARE @MaKM VARCHAR(20);
    DECLARE @GiaTriGiam DECIMAL(18, 2);
    DECLARE @LoaiGiam NVARCHAR(10);
    DECLARE @TongTienSauGiam DECIMAL(18, 2);
    DECLARE @MaKH VARCHAR(20);

    -- 1. Tính tổng tiền gốc các món trong hóa đơn
    SELECT @TongTienGoc = SUM(CTHD.SoLuong * CTHD.DonGia), @MaKM = HD.MaKM, @MaKH = HD.MaKH
    FROM HoaDon HD
    JOIN ChiTietHD CTHD ON HD.MaHD = CTHD.MaHD
    WHERE HD.MaHD = @MaHD
    GROUP BY HD.MaKM, HD.MaKH;

    IF @TongTienGoc IS NULL OR @TongTienGoc = 0
    BEGIN
        RAISERROR(N'Hóa đơn không có món ăn nào.', 16, 1);
        RETURN;
    END

    SET @TongTienSauGiam = @TongTienGoc;

    -- 2. Áp dụng Khuyến Mãi (nếu có)
    IF @MaKM IS NOT NULL
    BEGIN
        SELECT @GiaTriGiam = GiaTriGiam, @LoaiGiam = LoaiGiam
        FROM KhuyenMai
        WHERE MaKM = @MaKM AND TrangThai = N'Hoạt động'
        AND NgayKetThuc >= GETDATE();

        IF @GiaTriGiam IS NOT NULL
        BEGIN
            IF @LoaiGiam = N'Phần trăm' 
            BEGIN
                DECLARE @SoTienGiam_PhanTram DECIMAL(18, 2) = @TongTienGoc * @GiaTriGiam / 100;
                SET @TongTienSauGiam = @TongTienGoc - @SoTienGiam_PhanTram;
            END
            ELSE IF @LoaiGiam = N'Tiền mặt'
            BEGIN
                SET @TongTienSauGiam = @TongTienGoc - @GiaTriGiam;
                IF @TongTienSauGiam < 0 SET @TongTienSauGiam = 0;
            END
            
            PRINT N'Áp dụng khuyến mãi ' + @MaKM + N'. Giảm giá: ' + 
                  CAST((@TongTienGoc - @TongTienSauGiam) AS NVARCHAR(50)) + N' VNĐ.';
        END
        ELSE
        BEGIN
            UPDATE HoaDon SET MaKM = NULL WHERE MaHD = @MaHD;
            PRINT N'Mã khuyến mãi ' + @MaKM + N' không hợp lệ/hết hạn. Không áp dụng giảm giá.';
        END
    END
    
    -- 3. Cập nhật Hóa Đơn và Bàn
    UPDATE HoaDon
    SET 
        TongTien = @TongTienSauGiam,
        TrangThai = N'Đã thanh toán',
        HinhThucThanhToan = @HinhThucThanhToan
    WHERE MaHD = @MaHD;

    DECLARE @MaBan VARCHAR(20) = (SELECT MaBan FROM HoaDon WHERE MaHD = @MaHD);
    UPDATE Ban SET TrangThai = N'Trống' WHERE MaBan = @MaBan;

    -- 4. Cộng Điểm Tích Lũy mới cho Khách hàng (Mỗi 100K = 5 điểm)
    DECLARE @DiemCong INT = FLOOR(@TongTienSauGiam / 100000) * 5;

    IF @MaKH IS NOT NULL
    BEGIN
        UPDATE KhachHang
        SET DiemTichLuy = DiemTichLuy + @DiemCong
        WHERE MaKH = @MaKH;
        PRINT N'Khách hàng được cộng thêm ' + CAST(@DiemCong AS NVARCHAR(10)) + N' điểm tích lũy.';
    END

    SELECT N'Thanh toán thành công. Tổng tiền cuối cùng: ' + 
           CAST(@TongTienSauGiam AS NVARCHAR(50)) + N' VNĐ.' AS KetQua;
END;
GO

-- Helper class để chứa kết quả báo cáo
IF OBJECT_ID('USP_GetDoanhThuLoiNhuan', 'P') IS NOT NULL
    DROP PROCEDURE USP_GetDoanhThuLoiNhuan;
GO

CREATE PROCEDURE USP_GetDoanhThuLoiNhuan
    @Nam INT
AS
BEGIN
    -- 1. Tạo bảng tạm chứa 12 tháng
    WITH Months AS (
        SELECT 1 AS Thang UNION ALL SELECT 2 UNION ALL SELECT 3 UNION ALL SELECT 4
        UNION ALL SELECT 5 UNION ALL SELECT 6 UNION ALL SELECT 7 UNION ALL SELECT 8
        UNION ALL SELECT 9 UNION ALL SELECT 10 UNION ALL SELECT 11 UNION ALL SELECT 12
    ),
    -- 2. Tổng doanh thu (Chỉ hóa đơn đã thanh toán)
    Revenue AS (
        SELECT MONTH(ThoiGianLap) AS Thang, SUM(TongTien) AS DoanhThu
        FROM HoaDon 
        WHERE YEAR(ThoiGianLap) = @Nam AND TrangThai = N'Đã thanh toán'
        GROUP BY MONTH(ThoiGianLap)
    ),
    -- 3. Chi phí nhập hàng
    ImportCost AS (
        SELECT MONTH(ThoiGian) AS Thang, SUM(TongTien) AS ChiPhiNhap
        FROM PhieuNhapHang 
        WHERE YEAR(ThoiGian) = @Nam
        GROUP BY MONTH(ThoiGian)
    ),
    -- 4. Tổng lương nhân viên (Cố định theo tháng hiện tại - giả định)
    SalaryCost AS (
        SELECT SUM(Luong) AS TongLuong FROM NhanVien
    )
    
    -- 5. Tổng hợp
    SELECT 
        m.Thang,
        ISNULL(r.DoanhThu, 0) AS DoanhThu,
        ISNULL(i.ChiPhiNhap, 0) AS ChiPhiNhap,
        ISNULL(s.TongLuong, 0) AS LuongNhanVien,
        (ISNULL(i.ChiPhiNhap, 0) + ISNULL(s.TongLuong, 0)) AS TongChiPhi,
        (ISNULL(r.DoanhThu, 0) - (ISNULL(i.ChiPhiNhap, 0) + ISNULL(s.TongLuong, 0))) AS LoiNhuan
    FROM Months m
    LEFT JOIN Revenue r ON m.Thang = r.Thang
    LEFT JOIN ImportCost i ON m.Thang = i.Thang
    CROSS JOIN SalaryCost s
    ORDER BY m.Thang;
END;
GO
-- =============================================
-- 5. CHÈN DỮ LIỆU MẪU VÀ KIỂM THỬ (ĐÃ SỬA LỖI BIẾN CỤC BỘ)
-- =============================================

-- KHAI BÁO BIẾN CHUNG CHO TOÀN BỘ PHẦN CHÈN DỮ LIỆU VÀ KIỂM THỬ
DECLARE @MaKH_Test VARCHAR(20);
DECLARE @MaNV_Test VARCHAR(20);
DECLARE @MaQL_Test VARCHAR(20);
DECLARE @MaBan_Test VARCHAR(20);
DECLARE @MaTD_Chinh VARCHAR(20);
DECLARE @MaTD_Nuoc VARCHAR(20);
DECLARE @MaMon_BoBifTet VARCHAR(20);
DECLARE @MaMon_Coca VARCHAR(20);
DECLARE @MaKM_Giam50K VARCHAR(20);
DECLARE @MaHD_Moi VARCHAR(20);

-- 5.1. Chèn NguoiDung
INSERT INTO NguoiDung (TenDN, MatKhau, VaiTro) VALUES
    ('admin1', '123456', N'Quản lý'),
    ('thungan1', '123456', N'Thu ngân'),
    ('phucvu1', '123456', N'Phục vụ'),
    ('khach1', '123456', N'Khách hàng');
-- ĐÃ XÓA GO Ở ĐÂY ĐỂ TRÁNH LỖI BIẾN CỤC BỘ

-- Lấy mã KH và NV/QL
SET @MaKH_Test = (SELECT TOP 1 MaNguoiDung FROM NguoiDung WHERE TenDN = 'khach1');
SET @MaNV_Test = (SELECT TOP 1 MaNguoiDung FROM NguoiDung WHERE TenDN = 'thungan1');
SET @MaQL_Test = (SELECT TOP 1 MaNguoiDung FROM NguoiDung WHERE TenDN = 'admin1');

-- 5.2. Chèn thông tin chi tiết
INSERT INTO QuanLy (MaQL, Ten, SDT)
SELECT @MaQL_Test, N'Nguyễn Văn Quản Lý', '0909000001';

INSERT INTO NhanVien (MaNV, Ten, SDT, Luong) -- <<-- CẬP NHẬT
SELECT @MaNV_Test, N'Lê Thu Ngân', '0909000002', 8000000; -- <<-- Thêm lương 8 triệu

INSERT INTO NhanVien (MaNV, Ten, SDT, Luong) -- <<-- CẬP NHẬT
SELECT MaNguoiDung, N'Trần Phục Vụ', '0909000003', 6000000 -- <<-- Thêm lương 6 triệu
FROM NguoiDung WHERE TenDN = 'phucvu1';

INSERT INTO KhachHang (MaKH, Ten, SDT, DiemTichLuy)
SELECT @MaKH_Test, N'Khách Vãng Lai', '0901234567', 150; 

-- 5.3. Chèn Bàn
INSERT INTO Ban (Ten, Loai) VALUES
    (N'Bàn 1', N'Thường'),
    (N'Bàn 2', N'Thường'),
    (N'Bàn 3', N'VIP');
SET @MaBan_Test = (SELECT MaBan FROM Ban WHERE Ten = N'Bàn 1');

-- 5.4. Chèn Thực Đơn và Món
INSERT INTO ThucDon (Ten, MoTa) VALUES
    (N'Món chính', N'Các món ăn chính trong bữa'),
    (N'Đồ uống', N'Nước ngọt và bia');
SET @MaTD_Chinh = (SELECT MaThucDon FROM ThucDon WHERE Ten=N'Món chính');
SET @MaTD_Nuoc = (SELECT MaThucDon FROM ThucDon WHERE Ten=N'Đồ uống');

INSERT INTO Mon (MaThucDon, Ten, Gia)
VALUES (@MaTD_Chinh, N'Bò bít tết', 120000),
       (@MaTD_Nuoc, N'Coca Cola', 20000);
SET @MaMon_BoBifTet = (SELECT MaMon FROM Mon WHERE Ten = N'Bò bít tết');
SET @MaMon_Coca = (SELECT MaMon FROM Mon WHERE Ten = N'Coca Cola');

-- 5.5. Chèn Nguyên Liệu và Công Thức
INSERT INTO NguyenLieu (Ten, SoLuongTon, DonVi) VALUES 
(N'Thịt bò', 10, N'kg'),
(N'Coca chai', 100, N'l'),
(N'Gạo', 50, N'kg');

INSERT INTO CongThuc (MaMon, MaNguyenLieu, LuongTieuHao)
VALUES 
(@MaMon_BoBifTet, (SELECT MaNguyenLieu FROM NguyenLieu WHERE Ten = N'Thịt bò'), 0.3),
(@MaMon_Coca, (SELECT MaNguyenLieu FROM NguyenLieu WHERE Ten = N'Coca chai'), 0.33);

-- 5.6. Chèn Nhà Cung Cấp & Phiếu Nhập Hàng
INSERT INTO NhaCungCap (Ten, DiaChi, SDT) VALUES
(N'Công ty Thực Phẩm Sạch', N'Quận 1, TP.HCM', '0283999999');

INSERT INTO PhieuNhapHang (MaNCC, MaNV, TongTien)
VALUES 
(
    (SELECT TOP 1 MaNCC FROM NhaCungCap), 
    @MaNV_Test,
    5000000
);

-- 5.7. Chèn Khuyến Mãi
INSERT INTO KhuyenMai (TenKM, MoTa, DiemCanThiet, GiaTriGiam, LoaiGiam, NgayKetThuc) VALUES
    (N'Giảm 50K hóa đơn', N'Giảm 50.000 VNĐ', 100, 50000, N'Tiền mặt', DATEADD(month, 1, GETDATE())),
    (N'Giảm 10% tổng bill', N'Giảm 10% trên tổng hóa đơn', 200, 10, N'Phần trăm', DATEADD(month, 2, GETDATE()));
SET @MaKM_Giam50K = (SELECT MaKM FROM KhuyenMai WHERE DiemCanThiet = 100);

-- 5.8. Chèn giao dịch Đổi Điểm (Trừ 100 điểm)
INSERT INTO DoiDiem (MaKH, MaKM, DiemDaDung) 
VALUES (@MaKH_Test, @MaKM_Giam50K, 100);
PRINT N'--- Sau khi đổi điểm (150 - 100 = 50 điểm còn lại) ---';
SELECT MaKH, Ten, DiemTichLuy FROM KhachHang WHERE MaKH = @MaKH_Test;

-- 5.9. Tạo Hóa Đơn và Áp dụng KM
UPDATE Ban SET TrangThai = N'Đang phục vụ' WHERE MaBan = @MaBan_Test;

INSERT INTO HoaDon (MaBan, MaKH, MaKM) 
VALUES (@MaBan_Test, @MaKH_Test, @MaKM_Giam50K); 
SET @MaHD_Moi = (SELECT MAX(MaHD) FROM HoaDon);

-- Thêm Chi Tiết Hóa Đơn (Tổng tiền gốc: 660,000 VNĐ)
INSERT INTO ChiTietHD (MaHD, MaMon, SoLuong, DonGia) VALUES
(@MaHD_Moi, @MaMon_BoBifTet, 5, 120000), 
(@MaHD_Moi, @MaMon_Coca, 3, 20000);

-- 5.10. KIỂM THỬ THANH TOÁN
PRINT N'--- Thực hiện thanh toán (Tổng tiền gốc 660K, giảm 50K -> 610K) ---';
EXEC USP_ThanhToanHoaDon @MaHD = @MaHD_Moi, @HinhThucThanhToan = N'Chuyển khoản';

-- 5.11. KIỂM TRA KẾT QUẢ CUỐI CÙNG (Sử dụng biến đã được set giá trị)
PRINT N'--- KẾT QUẢ CUỐI CÙNG ---';

SELECT N'1. Chi tiết Hóa Đơn (TongTien=610,000, TrangThai=Đã thanh toán)' AS Description;
SELECT MaHD, MaKM, TongTien, TrangThai, HinhThucThanhToan 
FROM HoaDon WHERE MaHD = @MaHD_Moi;

SELECT N'2. Trạng thái Bàn (Trống)' AS Description;
SELECT MaBan, TrangThai FROM Ban WHERE MaBan = @MaBan_Test;

SELECT N'3. Điểm Tích Lũy Khách Hàng (50 điểm còn lại + 30 điểm mới = 80 điểm)' AS Description;
SELECT MaKH, Ten, DiemTichLuy FROM KhachHang WHERE MaKH = @MaKH_Test;

-- =============================================
-- KẾT THÚC SCRIPT HOÀN CHỈNH
-- =============================================
SELECT * FROM NguoiDung
SELECT * FROM KhachHang
SELECT * FROM NhanVien