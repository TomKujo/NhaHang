CREATE DATABASE NhaHang;
GO
USE NhaHang;
GO

-- ================================================================
-- 2. TẠO BẢNG (SCHEMA)
-- ================================================================

CREATE TABLE NguoiDung (
    MaNguoiDung VARCHAR(20) CONSTRAINT PK_NguoiDung PRIMARY KEY,
    TenDN VARCHAR(50) CONSTRAINT UQ_NguoiDung_TenDN UNIQUE NOT NULL,
    MatKhau VARCHAR(255) NOT NULL,
    TrangThai NVARCHAR(20) DEFAULT N'Kích hoạt',
    VaiTro NVARCHAR(20),
    SoLanSai INT DEFAULT 0
);

CREATE TABLE KhachHang (
    MaKH VARCHAR(20) CONSTRAINT PK_KhachHang PRIMARY KEY,
    HoVaTen NVARCHAR(100),
    SDT VARCHAR(15) CONSTRAINT UQ_KhachHang_SDT UNIQUE,
    Email VARCHAR(100),
    DiemTichLuy INT DEFAULT 0,
    CONSTRAINT FK_KhachHang_MaKH_NguoiDung FOREIGN KEY (MaKH) REFERENCES NguoiDung(MaNguoiDung)
);

CREATE TABLE QuanLy (
    MaQL VARCHAR(20) CONSTRAINT PK_QuanLy PRIMARY KEY,
    TenQL NVARCHAR(100),
    SDT VARCHAR(15) CONSTRAINT UQ_QuanLy_SDT UNIQUE,
    CONSTRAINT FK_QuanLy_MaQL_NguoiDung FOREIGN KEY (MaQL) REFERENCES NguoiDung(MaNguoiDung)
);

CREATE TABLE NhanVien (
    MaNV VARCHAR(20) CONSTRAINT PK_NhanVien PRIMARY KEY,
    TenNV NVARCHAR(100),
    DiaChi NVARCHAR(200),
    SDT VARCHAR(15) CONSTRAINT UQ_NhanVien_SDT UNIQUE,
    CONSTRAINT FK_NhanVien_MaNV_NguoiDung FOREIGN KEY (MaNV) REFERENCES NguoiDung(MaNguoiDung)
);

CREATE TABLE Ban (
    MaBan VARCHAR(20) CONSTRAINT PK_Ban PRIMARY KEY,
    Loai NVARCHAR(50),
    TenBan NVARCHAR(100),
    TrangThai NVARCHAR(20) DEFAULT N'Trống',
    TienCoc DECIMAL(18,2) DEFAULT 0
);

CREATE TABLE ThucDon (
    MaThucDon VARCHAR(20) CONSTRAINT PK_ThucDon PRIMARY KEY,
    TenThucDon NVARCHAR(100),
    MoTa NVARCHAR(255)
);

CREATE TABLE MonAn (
    MaMonAn VARCHAR(20) CONSTRAINT PK_MonAn PRIMARY KEY,
    MaThucDon VARCHAR(20) NOT NULL,
    TenMonAn NVARCHAR(100),
    Gia DECIMAL(18,2),
    DiemThuong INT DEFAULT 0,
    CONSTRAINT FK_MonAn_MaThucDon_ThucDon FOREIGN KEY (MaThucDon) REFERENCES ThucDon(MaThucDon)
);

CREATE TABLE Voucher (
    MaVoucher VARCHAR(20) CONSTRAINT PK_Voucher PRIMARY KEY,
    MaMonAn VARCHAR(20) NOT NULL,
    MoTa NVARCHAR(255),
    LoaiGiamGia NVARCHAR(20),
    HanSuDung DATE,
    CONSTRAINT FK_Voucher_MaMonAn_MonAn FOREIGN KEY (MaMonAn) REFERENCES MonAn(MaMonAn)
);

CREATE TABLE HoaDon (
    MaHoaDon VARCHAR(20) CONSTRAINT PK_HoaDon PRIMARY KEY,
    MaBan VARCHAR(20) NOT NULL,
    MaVoucher VARCHAR(20) NULL,
    MaKH VARCHAR(20) NULL,
    ThoiGianLap DATETIME DEFAULT GETDATE(),
    MoTa NVARCHAR(255),
    ThanhTien DECIMAL(18,2) DEFAULT 0,
    TienGiam DECIMAL(18,2) DEFAULT 0,
    TongTien DECIMAL(18,2) DEFAULT 0,
    TrangThai NVARCHAR(20) DEFAULT N'Chưa thanh toán',
    ThanhToan NVARCHAR(20) DEFAULT N'Tiền mặt',
    CONSTRAINT FK_HoaDon_MaBan_Ban FOREIGN KEY (MaBan) REFERENCES Ban(MaBan),
    CONSTRAINT FK_HoaDon_MaVoucher_Voucher FOREIGN KEY (MaVoucher) REFERENCES Voucher(MaVoucher)
);

CREATE TABLE ChiTietHoaDon (
    MaHoaDon VARCHAR(20),
    MaMonAn VARCHAR(20),
    SoLuong INT DEFAULT 1,
    ThanhTien DECIMAL(18,2),
    CONSTRAINT PK_ChiTietHoaDon PRIMARY KEY (MaHoaDon, MaMonAn),
    CONSTRAINT FK_ChiTietHoaDon_MaHoaDon_HoaDon FOREIGN KEY (MaHoaDon) REFERENCES HoaDon(MaHoaDon),
    CONSTRAINT FK_ChiTietHoaDon_MaMonAn_MonAn FOREIGN KEY (MaMonAn) REFERENCES MonAn(MaMonAn)
);

CREATE TABLE ThucPham (
    MaThucPham VARCHAR(20) CONSTRAINT PK_ThucPham PRIMARY KEY,
    TenTP NVARCHAR(100),
    SoLuongTonKho INT DEFAULT 0,
    DonViTinh NVARCHAR(20)
);

CREATE TABLE NhaCungCap (
    MaNCC VARCHAR(20) CONSTRAINT PK_NhaCungCap PRIMARY KEY,
    TenNCC NVARCHAR(100),
    DiaChi NVARCHAR(200),
    SDT VARCHAR(15)
);

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
-- 3. TẠO TRIGGERS
-- ================================================================

CREATE TRIGGER trg_CreateMaNguoiDung ON NguoiDung INSTEAD OF INSERT AS
BEGIN
    DECLARE @TenDN VARCHAR(50), @MatKhau VARCHAR(255), @TrangThai NVARCHAR(20), @VaiTro NVARCHAR(20), @SoLanSai INT, @NewID VARCHAR(20);
    SELECT @TenDN = TenDN, @MatKhau = MatKhau, @TrangThai = TrangThai, @VaiTro = VaiTro, @SoLanSai = SoLanSai FROM inserted;

    IF (@VaiTro = N'Quản lý') BEGIN
        DECLARE @MaxQL INT; SELECT @MaxQL = MAX(CAST(SUBSTRING(MaNguoiDung, 4, 10) AS INT)) FROM NguoiDung WHERE MaNguoiDung LIKE 'QL-%';
        SET @NewID = 'QL-' + RIGHT('000' + CAST(ISNULL(@MaxQL, 0) + 1 AS VARCHAR(3)), 3);
    END
    ELSE IF (@VaiTro = N'Khách hàng') BEGIN
        DECLARE @Today CHAR(8) = CONVERT(CHAR(8), GETDATE(), 112);
        DECLARE @MaxKH INT; SELECT @MaxKH = MAX(CAST(RIGHT(MaNguoiDung, 4) AS INT)) FROM NguoiDung WHERE MaNguoiDung LIKE 'KH-' + @Today + '-%';
        SET @NewID = 'KH-' + @Today + '-' + RIGHT('0000' + CAST(ISNULL(@MaxKH, 0) + 1 AS VARCHAR(4)), 4);
    END
    ELSE BEGIN
        DECLARE @MaxNV INT; SELECT @MaxNV = MAX(CAST(SUBSTRING(MaNguoiDung, 4, 10) AS INT)) FROM NguoiDung WHERE MaNguoiDung LIKE 'NV-%';
        SET @NewID = 'NV-' + RIGHT('000' + CAST(ISNULL(@MaxNV, 0) + 1 AS VARCHAR(3)), 3);
    END

    INSERT INTO NguoiDung (MaNguoiDung, TenDN, MatKhau, TrangThai, VaiTro, SoLanSai)
    VALUES (@NewID, @TenDN, @MatKhau, ISNULL(@TrangThai, N'Kích hoạt'), @VaiTro, ISNULL(@SoLanSai, 0));
END;
GO

CREATE TRIGGER trg_CreateMaMonAn ON MonAn INSTEAD OF INSERT AS
BEGIN
    DECLARE @MaThucDon VARCHAR(20), @Gia DECIMAL(18,2), @DiemThuong INT, @TenMonAn NVARCHAR(100), @NewID VARCHAR(20);
    SELECT @MaThucDon = MaThucDon, @Gia = Gia, @DiemThuong = DiemThuong, @TenMonAn = TenMonAn FROM inserted;

    DECLARE @MaxMA INT;
    SELECT @MaxMA = MAX(CAST(SUBSTRING(MaMonAn, 4, 10) AS INT)) FROM MonAn WHERE MaMonAn LIKE 'MA-%';
    SET @NewID = 'MA-' + RIGHT('000' + CAST(ISNULL(@MaxMA, 0) + 1 AS VARCHAR(3)), 3);

    INSERT INTO MonAn (MaMonAn, MaThucDon, Gia, DiemThuong, TenMonAn)
    VALUES (@NewID, @MaThucDon, @Gia, @DiemThuong, @TenMonAn);
END;
GO

CREATE TRIGGER trg_CreateMaBan ON Ban INSTEAD OF INSERT AS BEGIN
    DECLARE @Loai NVARCHAR(50), @TenBan NVARCHAR(100), @TrangThai NVARCHAR(20), @TienCoc DECIMAL(18,2), @NewID VARCHAR(20);
    SELECT @Loai=Loai, @TenBan=TenBan, @TrangThai=TrangThai, @TienCoc=TienCoc FROM inserted;
    DECLARE @MaxBan INT; SELECT @MaxBan = MAX(CAST(SUBSTRING(MaBan, 5, 10) AS INT)) FROM Ban WHERE MaBan LIKE 'BAN-%';
    SET @NewID = 'BAN-' + RIGHT('000' + CAST(ISNULL(@MaxBan, 0) + 1 AS VARCHAR(3)), 3);
    INSERT INTO Ban (MaBan, Loai, TenBan, TrangThai, TienCoc) VALUES (@NewID, @Loai, @TenBan, @TrangThai, @TienCoc);
END;
GO

CREATE TRIGGER trg_CreateMaThucDon ON ThucDon INSTEAD OF INSERT AS BEGIN
    DECLARE @TenThucDon NVARCHAR(100), @MoTa NVARCHAR(255), @NewID VARCHAR(20);
    SELECT @TenThucDon=TenThucDon, @MoTa=MoTa FROM inserted;
    DECLARE @MaxTD INT; SELECT @MaxTD = MAX(CAST(SUBSTRING(MaThucDon, 4, 10) AS INT)) FROM ThucDon WHERE MaThucDon LIKE 'TD-%';
    SET @NewID = 'TD-' + RIGHT('000' + CAST(ISNULL(@MaxTD, 0) + 1 AS VARCHAR(3)), 3);
    INSERT INTO ThucDon (MaThucDon, TenThucDon, MoTa) VALUES (@NewID, @TenThucDon, @MoTa);
END;
GO

CREATE TRIGGER trg_CreateMaHoaDon ON HoaDon INSTEAD OF INSERT AS BEGIN
    DECLARE @MaBan VARCHAR(20), @MaKH VARCHAR(20), @NewID VARCHAR(20);
    SELECT @MaBan = MaBan, @MaKH = MaKH FROM inserted;
    DECLARE @Today CHAR(8) = CONVERT(CHAR(8), GETDATE(), 112);
    DECLARE @MaxHD INT; SELECT @MaxHD = MAX(CAST(RIGHT(MaHoaDon, 4) AS INT)) FROM HoaDon WHERE MaHoaDon LIKE 'HD-' + @Today + '-%';
    SET @NewID = 'HD-' + @Today + '-' + RIGHT('0000' + CAST(ISNULL(@MaxHD, 0) + 1 AS VARCHAR(4)), 4);
    INSERT INTO HoaDon (MaHoaDon, MaBan, MaKH, ThoiGianLap, TrangThai, ThanhToan) VALUES (@NewID, @MaBan, @MaKH, GETDATE(), N'Chưa thanh toán', N'Tiền mặt');
    SELECT @NewID AS MaHoaDon;
END;
GO

CREATE TRIGGER trg_CreateMaThucPham ON ThucPham INSTEAD OF INSERT AS BEGIN
    DECLARE @TenTP NVARCHAR(100), @SL INT, @DVT NVARCHAR(20), @NewID VARCHAR(20);
    SELECT @TenTP=TenTP, @SL=SoLuongTonKho, @DVT=DonViTinh FROM inserted;
    DECLARE @MaxTP INT; SELECT @MaxTP = MAX(CAST(SUBSTRING(MaThucPham, 4, 10) AS INT)) FROM ThucPham WHERE MaThucPham LIKE 'TP-%';
    SET @NewID = 'TP-' + RIGHT('000' + CAST(ISNULL(@MaxTP, 0) + 1 AS VARCHAR(3)), 3);
    INSERT INTO ThucPham (MaThucPham, TenTP, SoLuongTonKho, DonViTinh) VALUES (@NewID, @TenTP, @SL, @DVT);
END;
GO

CREATE TRIGGER trg_CreateMaNCC ON NhaCungCap INSTEAD OF INSERT AS BEGIN
    DECLARE @Ten NVARCHAR(100), @DC NVARCHAR(200), @SDT VARCHAR(15), @NewID VARCHAR(20);
    SELECT @Ten=TenNCC, @DC=DiaChi, @SDT=SDT FROM inserted;
    DECLARE @MaxNCC INT; SELECT @MaxNCC = MAX(CAST(SUBSTRING(MaNCC, 5, 10) AS INT)) FROM NhaCungCap WHERE MaNCC LIKE 'NCC-%';
    SET @NewID = 'NCC-' + RIGHT('000' + CAST(ISNULL(@MaxNCC, 0) + 1 AS VARCHAR(3)), 3);
    INSERT INTO NhaCungCap (MaNCC, TenNCC, DiaChi, SDT) VALUES (@NewID, @Ten, @DC, @SDT);
END;
GO

-- ================================================================
-- 4. ĐỔ DỮ LIỆU MẪU (DÙNG LOOKUP ID THAY VÌ HARDCODE)
-- ================================================================

-- Tài khoản
INSERT INTO NguoiDung (TenDN, MatKhau, VaiTro) VALUES ('admin1', '123456', N'Quản lý');
INSERT INTO NguoiDung (TenDN, MatKhau, VaiTro) VALUES ('thungan1', '123456', N'Thu ngân');
INSERT INTO NguoiDung (TenDN, MatKhau, VaiTro) VALUES ('phucvu1', '123456', N'Phục vụ');
INSERT INTO NguoiDung (TenDN, MatKhau, VaiTro) VALUES ('khach1', '123456', N'Khách hàng');

-- Thông tin chi tiết (Lấy ID tự động từ bảng NguoiDung)
INSERT INTO QuanLy (MaQL, TenQL, SDT) 
SELECT MaNguoiDung, N'Nguyễn Quản Lý', '0901111111' FROM NguoiDung WHERE TenDN = 'admin1';

INSERT INTO NhanVien (MaNV, TenNV, SDT) 
SELECT MaNguoiDung, N'Lê Thu Ngân', '0902222222' FROM NguoiDung WHERE TenDN = 'thungan1';

INSERT INTO NhanVien (MaNV, TenNV, SDT) 
SELECT MaNguoiDung, N'Trần Phục Vụ', '0903333333' FROM NguoiDung WHERE TenDN = 'phucvu1';

INSERT INTO KhachHang (MaKH, HoVaTen, DiemTichLuy, SDT) 
SELECT MaNguoiDung, N'Khách Hàng A', 100, '0904444444' FROM NguoiDung WHERE TenDN = 'khach1';

-- Bàn (Trigger tự sinh B01, B02...)
INSERT INTO Ban (Loai, TenBan, TrangThai) VALUES (N'Thường', N'Bàn 1', N'Trống');
INSERT INTO Ban (Loai, TenBan, TrangThai) VALUES (N'Thường', N'Bàn 2', N'Trống');
INSERT INTO Ban (Loai, TenBan, TrangThai) VALUES (N'VIP', N'Bàn 3', N'Trống');
INSERT INTO Ban (Loai, TenBan, TrangThai) VALUES (N'VIP', N'Bàn 4', N'Có khách');

-- Thực đơn
INSERT INTO ThucDon (TenThucDon, MoTa) VALUES (N'Món chính', N'Đồ ăn mặn');
INSERT INTO ThucDon (TenThucDon, MoTa) VALUES (N'Đồ uống', N'Giải khát');

-- Món ăn (Lấy ID thực đơn tự động)
INSERT INTO MonAn (MaThucDon, TenMonAn, Gia, DiemThuong) 
SELECT MaThucDon, N'Bò bít tết', 120000, 10 FROM ThucDon WHERE TenThucDon = N'Món chính';

INSERT INTO MonAn (MaThucDon, TenMonAn, Gia, DiemThuong) 
SELECT MaThucDon, N'Cơm gà', 50000, 5 FROM ThucDon WHERE TenThucDon = N'Món chính';

INSERT INTO MonAn (MaThucDon, TenMonAn, Gia, DiemThuong) 
SELECT MaThucDon, N'Coca Cola', 20000, 2 FROM ThucDon WHERE TenThucDon = N'Đồ uống';

-- Kiểm tra kết quả
SELECT * FROM MonAn;