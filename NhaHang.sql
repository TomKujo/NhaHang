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
-- 2. TẠO BẢNG (GIỮ NGUYÊN CẤU TRÚC GỐC)
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
    SDT VARCHAR(10) UNIQUE,
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
    TrangThai NVARCHAR(10) DEFAULT N'Trống'
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

CREATE TABLE HoaDon (
    MaHD VARCHAR(20) PRIMARY KEY,
    MaBan VARCHAR(20),
    MaKH VARCHAR(20),
    ThoiGianLap DATETIME DEFAULT GETDATE(),
    TongTien DECIMAL(18,2) DEFAULT 0,
    TrangThai NVARCHAR(20) DEFAULT N'Chưa thanh toán',
    HinhThucThanhToan NVARCHAR(20) DEFAULT N'Tiền mặt',
    FOREIGN KEY (MaBan) REFERENCES Ban(MaBan)
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
-- 3. XÂY DỰNG TRIGGERS (ĐÃ SỬA CHỮA)
-- =============================================

-- 3.1. Trigger NguoiDung (QL-xxx, NV-xxx, KH-YYYYMMDD-xxxx)
CREATE TRIGGER trg_NguoiDung ON NguoiDung
INSTEAD OF INSERT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Today CHAR(8) = CONVERT(CHAR(8), GETDATE(), 112);
    
    -- Lấy số Max hiện tại (Sử dụng ISNULL để tránh lỗi khi bảng trống)
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

-- 3.2. Trigger Ban (BAN-xxx)
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

-- 3.3. Trigger ThucDon (TD-xxx)
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

-- 3.4. Trigger Mon (MA-xxx)
CREATE TRIGGER trg_Mon ON Mon
INSTEAD OF INSERT
AS
BEGIN
    SET NOCOUNT ON;
    -- Kiểm tra khóa ngoại thủ công vì dùng INSTEAD OF INSERT
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

-- 3.5. Trigger NguyenLieu (NL-xxx)
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

-- 3.6. Trigger NhaCungCap (NCC-xxx)
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

-- 3.7. Trigger DatBan (DB-YYYYMMDD-xxxx)
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

-- 3.8. Trigger HoaDon (HD-YYYYMMDD-xxxx)
CREATE TRIGGER trg_HoaDon ON HoaDon
INSTEAD OF INSERT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Today CHAR(8) = CONVERT(CHAR(8), GETDATE(), 112);
    DECLARE @Max INT = ISNULL((SELECT MAX(CAST(RIGHT(MaHD, 4) AS INT)) FROM HoaDon WHERE MaHD LIKE 'HD-' + @Today + '-%'), 0);

    ;WITH SourceData AS (SELECT *, ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS RN FROM inserted)
    INSERT INTO HoaDon (MaHD, MaBan, MaKH, ThoiGianLap, TongTien, TrangThai, HinhThucThanhToan)
    SELECT 
        'HD-' + @Today + '-' + RIGHT('0000' + CAST(@Max + RN AS VARCHAR(10)), 4),
        MaBan, MaKH, ISNULL(ThoiGianLap, GETDATE()), ISNULL(TongTien, 0), 
        ISNULL(TrangThai, N'Chưa thanh toán'), ISNULL(HinhThucThanhToan, N'Tiền mặt')
    FROM SourceData;
END;
GO

-- 3.9. Trigger PhieuNhapHang (PN-YYYYMMDD-xxxx)
-- [SỬA LỖI]: Đã xóa cột TinhTrang khỏi INSERT vì bảng PhieuNhapHang không có cột này
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

-- =============================================
-- 4. CHÈN DỮ LIỆU MẪU (TEST TRIGGERS)
-- =============================================

-- 4.1. Chèn NguoiDung
-- Lưu ý: Không nhập MaNguoiDung, Trigger tự sinh
INSERT INTO NguoiDung (TenDN, MatKhau, VaiTro) VALUES
    ('admin1', '123456', N'Quản lý'),
    ('thungan1', '123456', N'Thu ngân'),
    ('phucvu1', '123456', N'Phục vụ'),
    ('khach1', '123456', N'Khách hàng');
GO

-- 4.2. Chèn thông tin chi tiết (Mapping theo TenDN)
-- Quản lý
INSERT INTO QuanLy (MaQL, Ten, SDT)
SELECT MaNguoiDung, N'Nguyễn Văn Quản Lý', '0909000001'
FROM NguoiDung WHERE TenDN = 'admin1';

-- Nhân viên
INSERT INTO NhanVien (MaNV, Ten, SDT)
SELECT MaNguoiDung, N'Lê Thu Ngân', '0909000002'
FROM NguoiDung WHERE TenDN = 'thungan1';

INSERT INTO NhanVien (MaNV, Ten, SDT)
SELECT MaNguoiDung, N'Trần Phục Vụ', '0909000003'
FROM NguoiDung WHERE TenDN = 'phucvu1';

-- Khách hàng
INSERT INTO KhachHang (MaKH, Ten, SDT)
SELECT MaNguoiDung, N'Khách Vãng Lai', '0901234567'
FROM NguoiDung WHERE TenDN='khach1';
GO

-- 4.3. Chèn Bàn (Trigger tự sinh BAN-xxx)
INSERT INTO Ban (Ten, Loai) VALUES
    (N'Bàn 1', N'Thường'),
    (N'Bàn 2', N'Thường'),
    (N'Bàn 3', N'VIP');
GO

-- 4.4. Chèn Thực Đơn (Trigger tự sinh TD-xxx)
INSERT INTO ThucDon (Ten, MoTa) VALUES
    (N'Món chính', N'Các món ăn chính trong bữa'),
    (N'Đồ uống', N'Nước ngọt và bia');
GO

-- 4.5. Chèn Món (Trigger tự sinh MA-xxx)
INSERT INTO Mon (MaThucDon, Ten, Gia)
SELECT MaThucDon, N'Bò bít tết', 120000 
FROM ThucDon WHERE Ten=N'Món chính';

INSERT INTO Mon (MaThucDon, Ten, Gia)
SELECT MaThucDon, N'Coca Cola', 20000 
FROM ThucDon WHERE Ten=N'Đồ uống';
GO

-- 4.6. Chèn Nguyên Liệu (Trigger tự sinh NL-xxx)
INSERT INTO NguyenLieu (Ten, SoLuongTon, DonVi) VALUES 
(N'Thịt bò', 10, N'kg'),
(N'Coca chai', 100, N'l'),
(N'Gạo', 50, N'kg');
GO

-- 4.7. Chèn Nhà Cung Cấp (Trigger tự sinh NCC-xxx)
INSERT INTO NhaCungCap (Ten, DiaChi, SDT) VALUES
(N'Công ty Thực Phẩm Sạch', N'Quận 1, TP.HCM', '0283999999');
GO

-- 4.8. Chèn Công Thức (Dữ liệu liên kết Món và Nguyên Liệu)
-- Giả sử MA-001 (Bò bít tết) cần NL-001 (Thịt bò)
INSERT INTO CongThuc (MaMon, MaNguyenLieu, LuongTieuHao)
VALUES 
((SELECT MaMon FROM Mon WHERE Ten = N'Bò bít tết'), (SELECT MaNguyenLieu FROM NguyenLieu WHERE Ten = N'Thịt bò'), 0.3),
((SELECT MaMon FROM Mon WHERE Ten = N'Coca Cola'), (SELECT MaNguyenLieu FROM NguyenLieu WHERE Ten = N'Coca chai'), 0.33);
GO

-- 4.9. Chèn Phiếu Nhập Hàng (Trigger tự sinh PN-YYYYMMDD-xxxx)
INSERT INTO PhieuNhapHang (MaNCC, MaNV, TongTien)
VALUES 
(
    (SELECT TOP 1 MaNCC FROM NhaCungCap), 
    (SELECT TOP 1 MaNV FROM NhanVien WHERE Ten LIKE N'%Thu Ngân%'),
    5000000
);
GO

-- =============================================
-- KIỂM TRA DỮ LIỆU
-- =============================================
SELECT * FROM NguoiDung;
SELECT * FROM NhanVien;
SELECT * FROM Ban;
SELECT * FROM Mon;
SELECT * FROM NguyenLieu;
SELECT * FROM PhieuNhapHang;