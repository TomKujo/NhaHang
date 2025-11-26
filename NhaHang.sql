CREATE DATABASE NhaHang;
GO
USE NhaHang;
GO

-- ================================================================
-- 2. TẠO BẢNG (SCHEMA)
-- ================================================================

-- Người dùng (Đã cập nhật cột SoLanSai và VaiTro đầy đủ)
CREATE TABLE NguoiDung (
    MaNguoiDung VARCHAR(20) CONSTRAINT PK_NguoiDung PRIMARY KEY,
    TenDN VARCHAR(50) CONSTRAINT UQ_NguoiDung_TenDN UNIQUE NOT NULL,
    MatKhau VARCHAR(255) NOT NULL,
    TrangThai NVARCHAR(20) DEFAULT N'Kích hoạt'
        CONSTRAINT CK_NguoiDung_TrangThai CHECK (TrangThai IN (N'Kích hoạt', N'Khóa')),
    VaiTro NVARCHAR(20)
        CONSTRAINT CK_NguoiDung_VaiTro CHECK (VaiTro IN (N'Khách hàng', N'Quản lý', N'Thu ngân', N'Phục vụ')),
    SoLanSai INT DEFAULT 0 -- Cột mới để đếm số lần sai pass
);
CREATE INDEX IX_NguoiDung_VaiTro ON NguoiDung(VaiTro);

-- Khách hàng
CREATE TABLE KhachHang (
    MaKH VARCHAR(20) CONSTRAINT PK_KhachHang PRIMARY KEY,
    HoVaTen NVARCHAR(100),
    SDT VARCHAR(15) CONSTRAINT UQ_KhachHang_SDT UNIQUE,
    Email VARCHAR(100) CONSTRAINT UQ_KhachHang_Email UNIQUE,
    DiemTichLuy INT DEFAULT 0,
    CONSTRAINT FK_KhachHang_MaKH_NguoiDung FOREIGN KEY (MaKH) REFERENCES NguoiDung(MaNguoiDung)
);

-- Quản lý
CREATE TABLE QuanLy (
    MaQL VARCHAR(20) CONSTRAINT PK_QuanLy PRIMARY KEY,
    TenQL NVARCHAR(100),
    DiaChi NVARCHAR(200),
    SDT VARCHAR(15) CONSTRAINT UQ_QuanLy_SDT UNIQUE,
    CONSTRAINT FK_QuanLy_MaQL_NguoiDung FOREIGN KEY (MaQL) REFERENCES NguoiDung(MaNguoiDung)
);

-- Nhân viên (Chứa cả Thu ngân và Phục vụ)
CREATE TABLE NhanVien (
    MaNV VARCHAR(20) CONSTRAINT PK_NhanVien PRIMARY KEY,
    TenNV NVARCHAR(100),
    DiaChi NVARCHAR(200),
    SDT VARCHAR(15) CONSTRAINT UQ_NhanVien_SDT UNIQUE,
    CONSTRAINT FK_NhanVien_MaNV_NguoiDung FOREIGN KEY (MaNV) REFERENCES NguoiDung(MaNguoiDung)
);

-- Bàn
CREATE TABLE Ban (
    MaBan VARCHAR(20) CONSTRAINT PK_Ban PRIMARY KEY,
    Loai NVARCHAR(50),
    TenBan NVARCHAR(100),
    TrangThai NVARCHAR(20) DEFAULT N'Trống'
        CONSTRAINT CK_Ban_TrangThai CHECK (TrangThai IN (N'Trống', N'Có khách')),
    TienCoc DECIMAL(18,2) DEFAULT 0
);

-- Danh mục Thực đơn
CREATE TABLE ThucDon (
    MaThucDon VARCHAR(20) CONSTRAINT PK_ThucDon PRIMARY KEY,
    TenThucDon NVARCHAR(100),
    MoTa NVARCHAR(255)
);

-- Món ăn
CREATE TABLE MonAn (
    MaMonAn VARCHAR(20) CONSTRAINT PK_MonAn PRIMARY KEY,
    MaThucDon VARCHAR(20) NOT NULL,
    Gia DECIMAL(18,2),
    DiemThuong INT DEFAULT 0,
    CONSTRAINT FK_MonAn_MaThucDon_ThucDon FOREIGN KEY (MaThucDon) REFERENCES ThucDon(MaThucDon)
);

-- Voucher
CREATE TABLE Voucher (
    MaVoucher VARCHAR(20) CONSTRAINT PK_Voucher PRIMARY KEY,
    MaMonAn VARCHAR(20) NOT NULL,
    MoTa NVARCHAR(255),
    LoaiGiamGia NVARCHAR(20)
        CONSTRAINT CK_Voucher_LoaiGiamGia CHECK (LoaiGiamGia IN (N'Phần trăm', N'Trừ thẳng')),
    HanSuDung DATE,
    CONSTRAINT FK_Voucher_MaMonAn_MonAn FOREIGN KEY (MaMonAn) REFERENCES MonAn(MaMonAn)
);

-- Hóa đơn
CREATE TABLE HoaDon (
    MaHoaDon VARCHAR(20) CONSTRAINT PK_HoaDon PRIMARY KEY,
    MaBan VARCHAR(20) NOT NULL,
    MaVoucher VARCHAR(20) NULL,
    MaKH VARCHAR(20) NULL, -- Cho phép Null nếu khách vãng lai
    ThoiGianLap DATETIME DEFAULT GETDATE(),
    MoTa NVARCHAR(255),
    ThanhTien DECIMAL(18,2) DEFAULT 0,
    TienGiam DECIMAL(18,2) DEFAULT 0,
    TongTien DECIMAL(18,2) DEFAULT 0,
    TrangThai NVARCHAR(20) DEFAULT N'Chưa thanh toán'
        CONSTRAINT CK_HoaDon_TrangThai CHECK (TrangThai IN (N'Đã thanh toán', N'Chưa thanh toán', N'Hủy')),
    ThanhToan NVARCHAR(20) DEFAULT N'Tiền mặt'
        CONSTRAINT CK_HoaDon_ThanhToan CHECK (ThanhToan IN (N'Tiền mặt', N'Chuyển khoản')),
    CONSTRAINT FK_HoaDon_MaBan_Ban FOREIGN KEY (MaBan) REFERENCES Ban(MaBan),
    CONSTRAINT FK_HoaDon_MaVoucher_Voucher FOREIGN KEY (MaVoucher) REFERENCES Voucher(MaVoucher)
);

-- Chi tiết hóa đơn
CREATE TABLE ChiTietHoaDon (
    MaHoaDon VARCHAR(20),
    MaMonAn VARCHAR(20),
    SoLuong INT DEFAULT 1,
    ThanhTien DECIMAL(18,2),
    CONSTRAINT PK_ChiTietHoaDon PRIMARY KEY (MaHoaDon, MaMonAn),
    CONSTRAINT FK_ChiTietHoaDon_MaHoaDon_HoaDon FOREIGN KEY (MaHoaDon) REFERENCES HoaDon(MaHoaDon),
    CONSTRAINT FK_ChiTietHoaDon_MaMonAn_MonAn FOREIGN KEY (MaMonAn) REFERENCES MonAn(MaMonAn)
);

-- Thực phẩm (Kho)
CREATE TABLE ThucPham (
    MaThucPham VARCHAR(20) CONSTRAINT PK_ThucPham PRIMARY KEY,
    TenTP NVARCHAR(100),
    SoLuongTonKho INT DEFAULT 0,
    DonViTinh NVARCHAR(20)
);

-- Nhà cung cấp
CREATE TABLE NhaCungCap (
    MaNCC VARCHAR(20) CONSTRAINT PK_NhaCungCap PRIMARY KEY,
    TenNCC NVARCHAR(100),
    DiaChi NVARCHAR(200),
    SDT VARCHAR(15)
);

-- Phiếu nhập hàng
CREATE TABLE PhieuNhapHang (
    MaPNH VARCHAR(20) CONSTRAINT PK_PhieuNhapHang PRIMARY KEY,
    MaNCC VARCHAR(20) NOT NULL,
    MaNguoiNhap VARCHAR(20) NOT NULL,
    TinhTrang NVARCHAR(50) DEFAULT N'Đã nhập',
    NgayNhapHang DATE DEFAULT GETDATE(),
    TongTien DECIMAL(18,2) DEFAULT 0,
    CONSTRAINT FK_PNH_MaNCC_NhaCungCap FOREIGN KEY (MaNCC) REFERENCES NhaCungCap(MaNCC),
    CONSTRAINT FK_PNH_MaNguoiNhap_NguoiDung FOREIGN KEY (MaNguoiNhap) REFERENCES NguoiDung(MaNguoiDung)
);

-- Chi tiết nhập hàng
CREATE TABLE ChiTietNhapHang (
    MaPNH VARCHAR(20),
    MaThucPham VARCHAR(20),
    SoLuong INT,
    TongTien DECIMAL(18,2),
    CONSTRAINT PK_ChiTietNhapHang PRIMARY KEY (MaPNH, MaThucPham),
    CONSTRAINT FK_ChiTietNhapHang_MaPNH_PhieuNhapHang FOREIGN KEY (MaPNH) REFERENCES PhieuNhapHang(MaPNH),
    CONSTRAINT FK_ChiTietNhapHang_MaThucPham_ThucPham FOREIGN KEY (MaThucPham) REFERENCES ThucPham(MaThucPham)
);

GO

-- ================================================================
-- 3. TẠO TRIGGERS (QUAN TRỌNG: Xử lý sinh mã)
-- ================================================================

-- Trigger sinh mã Người dùng (Xử lý cả Thu ngân/Phục vụ)
CREATE TRIGGER trg_CreateMaNguoiDung
ON NguoiDung
INSTEAD OF INSERT
AS
BEGIN
    DECLARE @TenDN VARCHAR(50), @MatKhau VARCHAR(255), @TrangThai NVARCHAR(20), @VaiTro NVARCHAR(20), @SoLanSai INT, @NewID VARCHAR(20);

    SELECT @TenDN = TenDN, @MatKhau = MatKhau, @TrangThai = TrangThai, @VaiTro = VaiTro, @SoLanSai = SoLanSai FROM inserted;

    -- QUẢN LÝ -> QL-001
    IF (@VaiTro = N'Quản lý')
    BEGIN
        DECLARE @MaxQL INT;
        SELECT @MaxQL = MAX(CAST(SUBSTRING(MaNguoiDung, 4, 10) AS INT)) FROM NguoiDung WHERE MaNguoiDung LIKE 'QL-%';
        SET @NewID = 'QL-' + RIGHT('000' + CAST(ISNULL(@MaxQL, 0) + 1 AS VARCHAR(3)), 3);
    END
    -- KHÁCH HÀNG -> KH-YYYYMMDD-0001
    ELSE IF (@VaiTro = N'Khách hàng')
    BEGIN
        DECLARE @Today CHAR(8) = CONVERT(CHAR(8), GETDATE(), 112);
        DECLARE @MaxKH INT;
        SELECT @MaxKH = MAX(CAST(RIGHT(MaNguoiDung, 4) AS INT)) FROM NguoiDung WHERE MaNguoiDung LIKE 'KH-' + @Today + '-%';
        SET @NewID = 'KH-' + @Today + '-' + RIGHT('0000' + CAST(ISNULL(@MaxKH, 0) + 1 AS VARCHAR(4)), 4);
    END
    -- THU NGÂN / PHỤC VỤ -> NV-001
    ELSE
    BEGIN
        DECLARE @MaxNV INT;
        SELECT @MaxNV = MAX(CAST(SUBSTRING(MaNguoiDung, 4, 10) AS INT)) FROM NguoiDung WHERE MaNguoiDung LIKE 'NV-%';
        SET @NewID = 'NV-' + RIGHT('000' + CAST(ISNULL(@MaxNV, 0) + 1 AS VARCHAR(3)), 3);
    END

    INSERT INTO NguoiDung (MaNguoiDung, TenDN, MatKhau, TrangThai, VaiTro, SoLanSai)
    VALUES (@NewID, @TenDN, @MatKhau, ISNULL(@TrangThai, N'Kích hoạt'), @VaiTro, ISNULL(@SoLanSai, 0));
END;
GO

-- Trigger sinh mã Hóa đơn
CREATE TRIGGER trg_CreateMaHoaDon ON HoaDon INSTEAD OF INSERT AS
BEGIN
    DECLARE @MaBan VARCHAR(20), @MaKH VARCHAR(20), @NewID VARCHAR(20);
    SELECT @MaBan = MaBan, @MaKH = MaKH FROM inserted;
    
    DECLARE @Today CHAR(8) = CONVERT(CHAR(8), GETDATE(), 112);
    DECLARE @MaxHD INT;
    SELECT @MaxHD = MAX(CAST(RIGHT(MaHoaDon, 4) AS INT)) FROM HoaDon WHERE MaHoaDon LIKE 'HD-' + @Today + '-%';
    SET @NewID = 'HD-' + @Today + '-' + RIGHT('0000' + CAST(ISNULL(@MaxHD, 0) + 1 AS VARCHAR(4)), 4);

    INSERT INTO HoaDon (MaHoaDon, MaBan, MaKH, ThoiGianLap, TrangThai, ThanhToan)
    VALUES (@NewID, @MaBan, @MaKH, GETDATE(), N'Chưa thanh toán', N'Tiền mặt');
    
    -- Trả về ID để C# lấy
    SELECT @NewID AS MaHoaDon;
END;
GO

-- ================================================================
-- 4. ĐỔ DỮ LIỆU MẪU (TEST DATA)
-- ================================================================

-- Tài khoản
INSERT INTO NguoiDung (TenDN, MatKhau, VaiTro) VALUES ('admin1', '123456', N'Quản lý');
INSERT INTO NguoiDung (TenDN, MatKhau, VaiTro) VALUES ('thungan1', '123456', N'Thu ngân');
INSERT INTO NguoiDung (TenDN, MatKhau, VaiTro) VALUES ('phucvu1', '123456', N'Phục vụ');
INSERT INTO NguoiDung (TenDN, MatKhau, VaiTro) VALUES ('khach1', '123456', N'Khách hàng');

-- Thông tin chi tiết nhân sự (Ánh xạ từ NguoiDung sang NhanVien/QuanLy)
-- Lưu ý: Vì mã sinh tự động, ta dùng SubQuery để map đúng ID
INSERT INTO QuanLy (MaQL, TenQL) SELECT MaNguoiDung, N'Nguyễn Quản Lý' FROM NguoiDung WHERE TenDN = 'admin1';
INSERT INTO NhanVien (MaNV, TenNV) SELECT MaNguoiDung, N'Lê Thu Ngân' FROM NguoiDung WHERE TenDN = 'thungan1';
INSERT INTO NhanVien (MaNV, TenNV) SELECT MaNguoiDung, N'Trần Phục Vụ' FROM NguoiDung WHERE TenDN = 'phucvu1';

-- Bàn
INSERT INTO Ban (MaBan, TenBan, Loai, TrangThai) VALUES ('B01', N'Bàn 1', N'Thường', N'Trống');
INSERT INTO Ban (MaBan, TenBan, Loai, TrangThai) VALUES ('B02', N'Bàn 2', N'Thường', N'Trống');
INSERT INTO Ban (MaBan, TenBan, Loai, TrangThai) VALUES ('B03', N'Bàn 3', N'VIP', N'Trống');
INSERT INTO Ban (MaBan, TenBan, Loai, TrangThai) VALUES ('B04', N'Bàn 4', N'VIP', N'Có khách');

-- Thực đơn & Món
INSERT INTO ThucDon (MaThucDon, TenThucDon) VALUES ('TD01', N'Món chính');
INSERT INTO ThucDon (MaThucDon, TenThucDon) VALUES ('TD02', N'Đồ uống');

INSERT INTO MonAn (MaMonAn, MaThucDon, Gia, DiemThuong) VALUES ('MA01', 'TD01', 120000, 10); -- Bò bít tết
INSERT INTO MonAn (MaMonAn, MaThucDon, Gia, DiemThuong) VALUES ('MA02', 'TD01', 50000, 5);   -- Cơm gà
INSERT INTO MonAn (MaMonAn, MaThucDon, Gia, DiemThuong) VALUES ('MA03', 'TD02', 20000, 2);   -- Coca Cola

-- Kho
INSERT INTO ThucPham (MaThucPham, TenTP, SoLuongTonKho, DonViTinh) VALUES ('TP01', N'Thịt bò', 50, 'Kg');
INSERT INTO ThucPham (MaThucPham, TenTP, SoLuongTonKho, DonViTinh) VALUES ('TP02', N'Gà', 30, 'Kg');

-- Kiểm tra kết quả
SELECT * FROM NguoiDung;