USE master;
GO

-- 1. CLEAN DATABASE
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

-- 2. TABLES
CREATE TABLE NguoiDung (
    MaNguoiDung VARCHAR(20) PRIMARY KEY,
    TenDN VARCHAR(255) UNIQUE NOT NULL,
    MatKhau VARCHAR(255) DEFAULT '123456',
    TrangThai NVARCHAR(255) DEFAULT N'Kích hoạt',
    VaiTro NVARCHAR(255),
    SoLanSai INT DEFAULT 0
);

CREATE TABLE KhachHang (
    MaKH VARCHAR(20) PRIMARY KEY,
    Ten NVARCHAR(255) NOT NULL,
    SDT VARCHAR(15),
    Email VARCHAR(255) NOT NULL,
    DiemTichLuy INT DEFAULT 0,
    FOREIGN KEY (MaKH) REFERENCES NguoiDung(MaNguoiDung)
);

CREATE TABLE NhanVien (
    MaNV VARCHAR(20) PRIMARY KEY,
    Ten NVARCHAR(255) NOT NULL,
    DiaChi NVARCHAR(255),
    SDT VARCHAR(15),
	Email VARCHAR(255) NOT NULL,
	Luong DECIMAL(18,2),
	FOREIGN KEY (MaNV) REFERENCES NguoiDung(MaNguoiDung)
);

CREATE TABLE QuanLy (
    MaQL VARCHAR(20) PRIMARY KEY,
    Ten NVARCHAR(255) NOT NULL,
    SDT VARCHAR(15),
	Email VARCHAR(255) NOT NULL,
    FOREIGN KEY (MaQL) REFERENCES NguoiDung(MaNguoiDung)
);

CREATE TABLE Ban (
    MaBan VARCHAR(20) PRIMARY KEY,
    Loai NVARCHAR(255) NOT NULL,
    Ten NVARCHAR(255) NOT NULL,
    TienCoc DECIMAL(18,2),
    TrangThai NVARCHAR(255) DEFAULT N'Trống'
);

CREATE TABLE DatBan (
    MaDatBan VARCHAR(20) PRIMARY KEY,
    MaBan VARCHAR(20),
	TenKH NVARCHAR(255) NOT NULL,
	SDT VARCHAR(15) NOT NULL,
    ThoiGian DATETIME NOT NULL,
    TrangThai NVARCHAR(255) DEFAULT N'Chờ phản hồi',
    FOREIGN KEY (MaBan) REFERENCES Ban(MaBan)
);

CREATE TABLE ThucDon (
    MaThucDon VARCHAR(20) PRIMARY KEY,
    Ten NVARCHAR(255) NOT NULL,
    MoTa NVARCHAR(255)
);

CREATE TABLE Mon (
    MaMon VARCHAR(20) PRIMARY KEY,
    MaThucDon VARCHAR(20),
    Ten NVARCHAR(255) NOT NULL,
    Gia DECIMAL(18,2) NOT NULL,
    DiemTichLuy INT DEFAULT 0,
    FOREIGN KEY (MaThucDon) REFERENCES ThucDon(MaThucDon)
);

CREATE TABLE KhuyenMai (
    MaKM VARCHAR(20) PRIMARY KEY,
    Ten NVARCHAR(255) NOT NULL,
    MoTa NVARCHAR(255),
    DiemCan INT NOT NULL, 
    GiaTriGiam DECIMAL(18, 2) NOT NULL, 
    LoaiGiam NVARCHAR(10) NOT NULL, 
    NgayBD DATETIME DEFAULT GETDATE(),
    NgayKT DATETIME,
    TrangThai NVARCHAR(255) DEFAULT N'Hoạt động'
);

CREATE TABLE DoiDiem (
    MaGD VARCHAR(20) PRIMARY KEY,
    MaKH VARCHAR(20),
    MaKM VARCHAR(20),
    ThoiGian DATETIME DEFAULT GETDATE(),
    DiemDung INT NOT NULL, 
    TrangThai NVARCHAR(255) DEFAULT N'Thành công',
    FOREIGN KEY (MaKH) REFERENCES KhachHang(MaKH),
    FOREIGN KEY (MaKM) REFERENCES KhuyenMai(MaKM)
);

CREATE TABLE HoaDon (
    MaHD VARCHAR(20) PRIMARY KEY,
    MaBan VARCHAR(20),
    MaKH VARCHAR(20),
    MaKM VARCHAR(20), 
    ThoiGian DATETIME DEFAULT GETDATE(),
    TongTien DECIMAL(18,2) NOT NULL,
    TrangThai NVARCHAR(255) DEFAULT N'Chưa thanh toán',
    FOREIGN KEY (MaBan) REFERENCES Ban(MaBan),
    FOREIGN KEY (MaKM) REFERENCES KhuyenMai(MaKM)
);

CREATE TABLE ChiTietHD (
	ID INT IDENTITY(1,1) PRIMARY KEY,
    MaHD VARCHAR(20),
    MaMon VARCHAR(20),
    SoLuong INT DEFAULT 1,
    DonGia DECIMAL(18,2) NOT NULL,
    GhiChu NVARCHAR(255),
	TrangThai NVARCHAR(255) NOT NULL DEFAULT N'Chưa phục vụ',
    FOREIGN KEY (MaHD) REFERENCES HoaDon(MaHD),
    FOREIGN KEY (MaMon) REFERENCES Mon(MaMon)
);

CREATE TABLE NguyenLieu (
    MaNguyenLieu VARCHAR(20) PRIMARY KEY,
    Ten NVARCHAR(255) NOT NULL,
    SoLuongTon DECIMAL(18, 2) DEFAULT 0,
    DonVi VARCHAR(255) NOT NULL
);

CREATE TABLE NhaCungCap (
    MaNCC VARCHAR(20) PRIMARY KEY,
    Ten NVARCHAR(255) NOT NULL,
    DiaChi NVARCHAR(255),
    SDT VARCHAR(15) NOT NULL
);

CREATE TABLE PhieuNhapHang (
    MaPhieu VARCHAR(20) PRIMARY KEY,
    MaNCC VARCHAR(20),
    MaNV VARCHAR(20),
    ThoiGian DATETIME DEFAULT GETDATE(),
    TongTien DECIMAL(18,2) NOT NULL,
    FOREIGN KEY (MaNCC) REFERENCES NhaCungCap(MaNCC),
	FOREIGN KEY (MaNV) REFERENCES NguoiDung(MaNguoiDung)
);

CREATE TABLE ChiTietNhap (
    MaPhieu VARCHAR(20),
    MaNguyenLieu VARCHAR(20),
    LuongYeuCau DECIMAL(18,2) NOT NULL,
	LuongThucTe DECIMAL(18,2) NOT NULL,
	DonGia DECIMAL(18,2) NOT NULL,
	TinhTrang NVARCHAR(255) DEFAULT N'Đủ',
    PRIMARY KEY (MaPhieu, MaNguyenLieu),
	FOREIGN KEY (MaPhieu) REFERENCES PhieuNhapHang(MaPhieu),
    FOREIGN KEY (MaNguyenLieu) REFERENCES NguyenLieu(MaNguyenLieu)
);

CREATE TABLE CongThuc (
    MaMon VARCHAR(20),
    MaNguyenLieu VARCHAR(20),
    LuongTieuHao DECIMAL(18,2) NOT NULL,
    PRIMARY KEY (MaMon, MaNguyenLieu),
    FOREIGN KEY (MaMon) REFERENCES Mon(MaMon),
    FOREIGN KEY (MaNguyenLieu) REFERENCES NguyenLieu(MaNguyenLieu)
);
GO

-- 3. TRIGGERS 
-- 3.1. NguoiDung
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

-- 3.2. Ban
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

-- 3.3. ThucDon
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

-- 3.4. Mon
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

-- 3.5. NguyenLieu
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

-- 3.6. NhaCungCap
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

-- 3.7. DatBan
IF OBJECT_ID('trg_DatBan', 'TR') IS NOT NULL DROP TRIGGER trg_DatBan;
GO

CREATE TRIGGER trg_DatBan ON DatBan
INSTEAD OF INSERT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Today CHAR(8) = CONVERT(CHAR(8), GETDATE(), 112);
    DECLARE @Max INT = ISNULL((SELECT MAX(CAST(RIGHT(MaDatBan, 4) AS INT)) FROM DatBan WHERE MaDatBan LIKE 'DB-' + @Today + '-%'), 0);

    ;WITH SourceData AS (SELECT *, ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS RN FROM inserted)
    INSERT INTO DatBan (MaDatBan, MaBan, TenKH, SDT, ThoiGian, TrangThai)
    SELECT 
        'DB-' + @Today + '-' + RIGHT('0000' + CAST(@Max + RN AS VARCHAR(10)), 4),
        MaBan, 
        ISNULL(TenKH, N'Khách vãng lai'),
        ISNULL(SDT, N''),
        ISNULL(ThoiGian, GETDATE()), 
        ISNULL(TrangThai, N'Chờ phản hồi')
    FROM SourceData;
END;
GO

-- 3.8. HoaDon
IF OBJECT_ID('trg_HoaDon', 'TR') IS NOT NULL DROP TRIGGER trg_HoaDon;
GO
CREATE TRIGGER trg_HoaDon ON HoaDon
INSTEAD OF INSERT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Today CHAR(8) = CONVERT(CHAR(8), GETDATE(), 112);
    DECLARE @Max INT = ISNULL((SELECT MAX(CAST(RIGHT(MaHD, 4) AS INT)) FROM HoaDon WHERE MaHD LIKE 'HD-' + @Today + '-%'), 0);

    ;WITH SourceData AS (SELECT *, ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS RN FROM inserted)
    INSERT INTO HoaDon (MaHD, MaBan, MaKH, MaKM, ThoiGian, TongTien, TrangThai)
    SELECT 
        'HD-' + @Today + '-' + RIGHT('0000' + CAST(@Max + RN AS VARCHAR(10)), 4),
        MaBan, MaKH, MaKM, ISNULL(ThoiGian, GETDATE()), ISNULL(TongTien, 0), 
        ISNULL(TrangThai, N'Chưa thanh toán')
    FROM SourceData;
END;
GO

-- 3.9. PhieuNhapHang
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

-- 3.10. KhuyenMai
IF OBJECT_ID('trg_KhuyenMai', 'TR') IS NOT NULL DROP TRIGGER trg_KhuyenMai;
GO
CREATE TRIGGER trg_KhuyenMai ON KhuyenMai
INSTEAD OF INSERT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Max INT = ISNULL((SELECT MAX(CAST(SUBSTRING(MaKM, 4, 10) AS INT)) FROM KhuyenMai WHERE MaKM LIKE 'KM-%'), 0);

    ;WITH SourceData AS (SELECT *, ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS RN FROM inserted)
    INSERT INTO KhuyenMai (MaKM, Ten, MoTa, DiemCan, GiaTriGiam, LoaiGiam, NgayBD, NgayKT, TrangThai)
    SELECT 
        'KM-' + RIGHT('000' + CAST(@Max + RN AS VARCHAR(10)), 3),
        Ten, MoTa, DiemCan, GiaTriGiam, ISNULL(LoaiGiam, N'Tiền mặt'), 
        ISNULL(NgayBD, GETDATE()), NgayKT, ISNULL(TrangThai, N'Hoạt động')
    FROM SourceData;
END;
GO

-- 3.11. DoiDiem
IF OBJECT_ID('trg_DoiDiem', 'TR') IS NOT NULL DROP TRIGGER trg_DoiDiem;
GO
CREATE TRIGGER trg_DoiDiem ON DoiDiem
INSTEAD OF INSERT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Today CHAR(8) = CONVERT(CHAR(8), GETDATE(), 112);
    DECLARE @Max INT = ISNULL((SELECT MAX(CAST(RIGHT(MaGD, 4) AS INT)) FROM DoiDiem WHERE MaGD LIKE 'DD-' + @Today + '-%'), 0);

    ;WITH SourceData AS (SELECT *, ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS RN FROM inserted)
    INSERT INTO DoiDiem (MaGD, MaKH, MaKM, ThoiGian, DiemDung, TrangThai)
    SELECT 
        'DD-' + @Today + '-' + RIGHT('0000' + CAST(@Max + RN AS VARCHAR(10)), 4),
        MaKH, MaKM, ISNULL(ThoiGian, GETDATE()), DiemDung, ISNULL(TrangThai, N'Thành công')
    FROM SourceData;
END;
GO

-- 3.12. TruDiemTichLuy
IF OBJECT_ID('trg_TruDiemTichLuy', 'TR') IS NOT NULL DROP TRIGGER trg_TruDiemTichLuy;
GO
CREATE TRIGGER trg_TruDiemTichLuy ON DoiDiem
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    
    IF EXISTS (SELECT 1 FROM inserted)
    BEGIN
        UPDATE KH
        SET DiemTichLuy = KH.DiemTichLuy - I.DiemDung
        FROM KhachHang KH
        INNER JOIN inserted I ON KH.MaKH = I.MaKH
        WHERE I.TrangThai = N'Thành công';
    END
END;
GO

-- 4. STRORED PROCEDURE
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
    DECLARE @DiemCong INT = 0; -- Biến lưu tổng điểm sẽ cộng

    -- 1. Tính tổng tiền gốc và lấy thông tin hóa đơn
    SELECT @TongTienGoc = SUM(CTHD.SoLuong * CTHD.DonGia), @MaKM = HD.MaKM, @MaKH = HD.MaKH
    FROM HoaDon HD
    JOIN ChiTietHD CTHD ON HD.MaHD = CTHD.MaHD
    WHERE HD.MaHD = @MaHD
    GROUP BY HD.MaKM, HD.MaKH;

    IF @TongTienGoc IS NULL SET @TongTienGoc = 0;
    SET @TongTienSauGiam = @TongTienGoc;

    -- 2. Áp dụng Khuyến Mãi (Nếu có)
    IF @MaKM IS NOT NULL
    BEGIN
        -- Lấy thông tin khuyến mãi còn hạn
        SELECT @GiaTriGiam = GiaTriGiam, @LoaiGiam = LoaiGiam
        FROM KhuyenMai
        WHERE MaKM = @MaKM AND TrangThai = N'Hoạt động'
        AND (NgayKT IS NULL OR NgayKT >= GETDATE());

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
        END
        ELSE
        BEGIN
            -- Nếu KM hết hạn hoặc bị khóa, xóa KM khỏi hóa đơn
            UPDATE HoaDon SET MaKM = NULL WHERE MaHD = @MaHD;
            SET @MaKM = NULL;
        END
    END
    
    -- 3. Cập nhật Hóa Đơn và Trạng thái Bàn
    UPDATE HoaDon
    SET 
        TongTien = @TongTienSauGiam,
        TrangThai = N'Đã thanh toán'
    WHERE MaHD = @MaHD;

    DECLARE @MaBan VARCHAR(20) = (SELECT MaBan FROM HoaDon WHERE MaHD = @MaHD);
    UPDATE Ban SET TrangThai = N'Trống' WHERE MaBan = @MaBan;

    -- 4. TÍNH ĐIỂM VÀ CỘNG ĐIỂM (Logic Mới)
    IF @MaKH IS NOT NULL
    BEGIN
        -- A. Điểm từ tổng tiền hóa đơn (Quy đổi: 100.000 VNĐ = 5 điểm)
        DECLARE @DiemBill INT = FLOOR(@TongTienSauGiam / 100000) * 5;

        -- B. Điểm thưởng riêng từ từng món ăn (Lấy từ bảng Mon)
        DECLARE @DiemMon INT = 0;
        SELECT @DiemMon = SUM(ISNULL(m.DiemTichLuy, 0) * ct.SoLuong)
        FROM ChiTietHD ct
        JOIN Mon m ON ct.MaMon = m.MaMon
        WHERE ct.MaHD = @MaHD;

        -- Tổng điểm cộng
        SET @DiemCong = @DiemBill + ISNULL(@DiemMon, 0);

        -- Cập nhật vào Khách hàng
        UPDATE KhachHang
        SET DiemTichLuy = DiemTichLuy + @DiemCong
        WHERE MaKH = @MaKH;
    END

    -- 5. Trả về kết quả (Kèm cột DiemDuocCong cho C# sử dụng)
    SELECT 
        N'Thanh toán thành công. Tổng tiền: ' + FORMAT(@TongTienSauGiam, 'N0') + N' VNĐ.' AS KetQua,
        @DiemCong AS DiemDuocCong;
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
    WITH Months AS (
        SELECT 1 AS Thang UNION ALL SELECT 2 UNION ALL SELECT 3 UNION ALL SELECT 4
        UNION ALL SELECT 5 UNION ALL SELECT 6 UNION ALL SELECT 7 UNION ALL SELECT 8
        UNION ALL SELECT 9 UNION ALL SELECT 10 UNION ALL SELECT 11 UNION ALL SELECT 12
    ),
    Revenue AS (
        SELECT MONTH(ThoiGian) AS Thang, SUM(TongTien) AS DoanhThu
        FROM HoaDon 
        WHERE YEAR(ThoiGian) = @Nam AND TrangThai = N'Đã thanh toán'
        GROUP BY MONTH(ThoiGian)
    ),
    ImportCost AS (
        SELECT MONTH(ThoiGian) AS Thang, SUM(TongTien) AS ChiPhiNhap
        FROM PhieuNhapHang 
        WHERE YEAR(ThoiGian) = @Nam
        GROUP BY MONTH(ThoiGian)
    ),
    SalaryCost AS (
        SELECT SUM(Luong) AS TongLuong FROM NhanVien
    )
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

-- 5. DATA
DELETE FROM ChiTietHD; DELETE FROM HoaDon; DELETE FROM DoiDiem; 
DELETE FROM KhuyenMai; DELETE FROM ChiTietNhap; DELETE FROM PhieuNhapHang;
DELETE FROM CongThuc; DELETE FROM NguyenLieu; DELETE FROM Mon; 
DELETE FROM ThucDon; DELETE FROM DatBan; DELETE FROM Ban; 
DELETE FROM QuanLy; DELETE FROM NhanVien; DELETE FROM KhachHang; DELETE FROM NguoiDung; 
DELETE FROM NhaCungCap;

DECLARE @MaKH_Test VARCHAR(20), @MaNV_Test VARCHAR(20), @MaQL_Test VARCHAR(20);
DECLARE @MaBan_Test VARCHAR(20);
DECLARE @MaTD_Chinh VARCHAR(20), @MaTD_Nuoc VARCHAR(20);
DECLARE @MaMon_BoBifTet VARCHAR(20), @MaMon_Coca VARCHAR(20);
DECLARE @MaKM_Giam50K VARCHAR(20);
DECLARE @MaHD_Moi VARCHAR(20);

-- 5.1. Người dùng
INSERT INTO NguoiDung (TenDN, MatKhau, VaiTro) VALUES
    ('admin1@gmail.com', '123456', N'Quản lý'),
    ('thungan1@gmail.com', '123456', N'Thu ngân'),
    ('phucvu1@gmail.com', '123456', N'Phục vụ'),
    ('khach1@gmail.com', '123456', N'Khách hàng');

SELECT TOP 1 @MaKH_Test = MaNguoiDung FROM NguoiDung WHERE TenDN = 'khach1@gmail.com';
SELECT TOP 1 @MaNV_Test = MaNguoiDung FROM NguoiDung WHERE TenDN = 'thungan1@gmail.com';
SELECT TOP 1 @MaQL_Test = MaNguoiDung FROM NguoiDung WHERE TenDN = 'admin1@gmail.com';

INSERT INTO QuanLy (MaQL, Ten, Email, SDT) 
VALUES (@MaQL_Test, N'Nguyễn Văn Quản Lý', 'admin1@gmail.com', '0909000001');

INSERT INTO NhanVien (MaNV, Ten, SDT, Luong, Email, DiaChi) VALUES 
(@MaNV_Test, N'Lê Thu Ngân', '0909000002', 8000000, 'thungan1@gmail.com', N'Hà Nội');

INSERT INTO NhanVien (MaNV, Ten, SDT, Luong, Email, DiaChi)
SELECT MaNguoiDung, N'Trần Phục Vụ', '0909000003', 6000000, 'phucvu1@gmail.com', N'Hà Nội'
FROM NguoiDung WHERE TenDN = 'phucvu1@gmail.com';

INSERT INTO KhachHang (MaKH, Ten, SDT, DiemTichLuy, Email) VALUES 
(@MaKH_Test, N'Khách Vãng Lai', '0901234567', 500, 'khach1@gmail.com');

UPDATE NguoiDung 
SET MatKhau = 'e10adc3949ba59abbe56e057f20f883e' 
WHERE MatKhau = '123456';
-- 5.2. Bàn
INSERT INTO Ban (Ten, Loai) VALUES (N'Bàn 1', N'Thường'), (N'Bàn 2', N'Thường'), (N'Bàn 3', N'VIP');
SELECT TOP 1 @MaBan_Test = MaBan FROM Ban WHERE Ten = N'Bàn 1';

-- 5.3. Thực Đơn và Món
INSERT INTO ThucDon (Ten, MoTa) VALUES (N'Món chính', N'Các món ăn chính'), (N'Đồ uống', N'Nước giải khát');
SELECT TOP 1 @MaTD_Chinh = MaThucDon FROM ThucDon WHERE Ten=N'Món chính';
SELECT TOP 1 @MaTD_Nuoc = MaThucDon FROM ThucDon WHERE Ten=N'Đồ uống';

INSERT INTO Mon (MaThucDon, Ten, Gia) VALUES 
(@MaTD_Chinh, N'Bò bít tết', 120000), (@MaTD_Nuoc, N'Coca Cola', 20000);

SELECT TOP 1 @MaMon_BoBifTet = MaMon FROM Mon WHERE Ten = N'Bò bít tết';
SELECT TOP 1 @MaMon_Coca = MaMon FROM Mon WHERE Ten = N'Coca Cola';

-- 5.4. Nguyên Liệu và Công Thức
INSERT INTO NguyenLieu (Ten, SoLuongTon, DonVi) VALUES (N'Thịt bò', 1000, N'kg'), (N'Coca chai', 1000, N'l');

DECLARE @MaNL_Bo VARCHAR(20) = (SELECT MaNguyenLieu FROM NguyenLieu WHERE Ten = N'Thịt bò');
DECLARE @MaNL_Coca VARCHAR(20) = (SELECT MaNguyenLieu FROM NguyenLieu WHERE Ten = N'Coca chai');

INSERT INTO CongThuc (MaMon, MaNguyenLieu, LuongTieuHao) VALUES 
(@MaMon_BoBifTet, @MaNL_Bo, 0.3),
(@MaMon_Coca, @MaNL_Coca, 0.33);

-- 5.5. Nhà Cung Cấp & Phiếu Nhập
INSERT INTO NhaCungCap (Ten, DiaChi, SDT) VALUES (N'CTY Thực Phẩm Sạch', N'HCM', '0283999999');
INSERT INTO PhieuNhapHang (MaNCC, MaNV, TongTien) VALUES ((SELECT TOP 1 MaNCC FROM NhaCungCap), @MaNV_Test, 5000000);
INSERT INTO ChiTietNhap (MaPhieu, MaNguyenLieu, LuongYeuCau, LuongThucTe, DonGia) VALUES 
((SELECT TOP 1 MaPhieu FROM PhieuNhapHang), @MaNL_Bo, 20, 20, 250000);

-- 5.6. Khuyến Mãi
INSERT INTO KhuyenMai (Ten, MoTa, DiemCan, GiaTriGiam, LoaiGiam, NgayKT) VALUES
    (N'Giảm 50K hóa đơn', N'Giảm 50.000 VNĐ', 100, 50000, N'Tiền mặt', DATEADD(month, 1, GETDATE())),
    (N'Giảm 10% tổng bill', N'Giảm 10%', 200, 10, N'Phần trăm', DATEADD(month, 2, GETDATE()));

SELECT TOP 1 @MaKM_Giam50K = MaKM FROM KhuyenMai WHERE DiemCan = 100;

-- 5.7. Điểm
INSERT INTO DoiDiem (MaKH, MaKM, DiemDung) VALUES (@MaKH_Test, @MaKM_Giam50K, 100);

PRINT N'--- Sau khi đổi điểm (150 - 100 = 50 điểm còn lại) ---';
SELECT MaKH, Ten, DiemTichLuy FROM KhachHang WHERE MaKH = @MaKH_Test;

-- 5.8. Hóa Đơn
UPDATE Ban SET TrangThai = N'Có khách' WHERE MaBan = @MaBan_Test;
INSERT INTO HoaDon (MaBan, MaKH, MaKM) VALUES (@MaBan_Test, @MaKH_Test, @MaKM_Giam50K);
SELECT @MaHD_Moi = MAX(MaHD) FROM HoaDon;

INSERT INTO ChiTietHD (MaHD, MaMon, SoLuong, DonGia) VALUES
(@MaHD_Moi, @MaMon_BoBifTet, 5, 120000), 
(@MaHD_Moi, @MaMon_Coca, 3, 20000);

SELECT * FROM NguoiDung