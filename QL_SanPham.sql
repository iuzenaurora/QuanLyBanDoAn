-- Tạo CSDL
CREATE DATABASE QL_SanPham;
GO

USE QL_SanPham;
GO

-- Bảng Loại sản phẩm
CREATE TABLE tblLoaiSP
(
    MaLoai INT IDENTITY(1,1) PRIMARY KEY,
    TenLoai NVARCHAR(255)
);

-- Bảng Danh mục con
CREATE TABLE tblDanhMucCon
(
    MaDM INT IDENTITY(1,1) PRIMARY KEY,
    TenDM NVARCHAR(255),
	MaLoai INT FOREIGN KEY REFERENCES tblLoaiSP(MaLoai)
);

-- Bảng Sản phẩm
CREATE TABLE tblSanPham
(
    MaSP INT IDENTITY(1,1) PRIMARY KEY,
    TenSP NVARCHAR(255),
--	MaLoai INT FOREIGN KEY REFERENCES tblLoaiSP(MaLoai),
    MaDM INT FOREIGN KEY REFERENCES tblDanhMucCon(MaDM),
    GiaBan DECIMAL(18, 2),
    MoTa NVARCHAR(MAX),
    AnhBia NVARCHAR(255)
);
select * from tblSanPham
-- Bảng Hình ảnh
CREATE TABLE tblHinhAnh (
    MaHinh INT IDENTITY(1,1) PRIMARY KEY,
    MaSP INT FOREIGN KEY REFERENCES tblSanPham(MaSP),
    TenHinh NVARCHAR(255)
);

-- Bảng Khách hàng
CREATE TABLE tblKhachHang
(
    MaKH INT IDENTITY(1,1) PRIMARY KEY,
    TenKH NVARCHAR(255),
	GioiTinh NVARCHAR(10),
	NamSinh INT,
    SoDienThoai VARCHAR(20),
	DiaChi NVARCHAR(255),
	Email NVARCHAR(100),
	Avarta NVARCHAR(255)
);

-- Bảng Vai trò
CREATE TABLE tblVaiTro
(
    MaVaiTro INT IDENTITY(1,1) PRIMARY KEY,
    TenVaiTro NVARCHAR(50),
    MoTa NVARCHAR(255)
);

-- Bảng Nhân viên
CREATE TABLE tblNhanVien 
(
    MaNV INT IDENTITY(1,1) PRIMARY KEY,
    TenNV NVARCHAR(100),
    GioiTinh NVARCHAR(10),
    NamSinh INT,
    VaiTro INT FOREIGN KEY REFERENCES tblVaiTro(MaVaiTro)
);

-- Bảng Tình trạng
CREATE TABLE tblTinhTrang
(
    ID INT IDENTITY(1,1) PRIMARY KEY,
    TinhTrangHoaDon NVARCHAR(50)
);

-- Bảng Hóa đơn
CREATE TABLE tblHoaDon
(
    MaHD INT IDENTITY(1,1) PRIMARY KEY,
    MaKH INT FOREIGN KEY REFERENCES tblKhachHang(MaKH),
    MaNV INT FOREIGN KEY REFERENCES tblNhanVien(MaNV),
    NgayLap DATETIME,
    TongTien DECIMAL(18,2),
    TinhTrang INT FOREIGN KEY REFERENCES tblTinhTrang(ID),
    DiaChiGiaoHang NVARCHAR(255),
    DaThanhToan BIT
);

select * from tblHoaDon

-- Bảng Chi tiết hóa đơn
CREATE TABLE tblChiTietHoaDon
(
    MaHD INT FOREIGN KEY REFERENCES tblHoaDon(MaHD),
    MaSP INT FOREIGN KEY REFERENCES tblSanPham(MaSP), 
    SoLuong INT,
    GiaBan DECIMAL(18,2),
    PRIMARY KEY (MaHD, MaSP)
);

create table tbltaikhoan
(
	username varchar(50) not null primary key,
	matkhau varchar(50) not null,
	manv int foreign key references tblnhanvien(manv),
	makh int foreign key references tblkhachhang(makh),
	mavaitro int foreign key references tblvaitro(mavaitro)
);


-- Dữ liệu mẫu
-- Loại sản phẩm
INSERT INTO tblLoaiSP(TenLoai)
VALUES (N'Món ăn'); -- MaLoai = 1
INSERT INTO tblLoaiSP(TenLoai)
VALUES (N'Đồ uống'); -- MaLoai = 2
INSERT INTO tblLoaiSP(TenLoai)
VALUES (N'Tráng miệng'); -- MaLoai = 3

-- Danh mục con
INSERT INTO tblDanhMucCon(TenDM, MaLoai)
VALUES (N'Burger', 1); -- MaDM = 1
INSERT INTO tblDanhMucCon(TenDM, MaLoai)
VALUES (N'Sandwich', 1); -- MaDM = 2
INSERT INTO tblDanhMucCon(TenDM, MaLoai)
VALUES (N'Gà rán', 1); -- MaDM = 3
INSERT INTO tblDanhMucCon(TenDM, MaLoai)
VALUES (N'Cà phê', 2); -- MaDM = 4
INSERT INTO tblDanhMucCon(TenDM, MaLoai)
VALUES (N'Trà', 2); -- MaDM = 5
INSERT INTO tblDanhMucCon(TenDM, MaLoai)
VALUES (N'Nước ngọt', 2); -- MaDM = 6
INSERT INTO tblDanhMucCon(TenDM, MaLoai)
VALUES (N'Nước suối', 2); -- MaDM = 7
INSERT INTO tblDanhMucCon(TenDM, MaLoai)
VALUES (N'Bánh ngọt', 3); -- MaDM = 8
INSERT INTO tblDanhMucCon(TenDM, MaLoai)
VALUES (N'Kem', 3); -- MaDM = 9

-- Sản Phẩm
-- Món ăn | 1: Burger, 2: Sandwich, 3: Gà rán
-- Đồ uống | 1: Cà phê, 2: Trà, 3: Nước ngọt, 4: Nước suối
-- Tráng miệng | 1: Bánh ngọt, 2: Kem
INSERT INTO tblSanPham (TenSP, MaDM, GiaBan, MoTa, AnhBia)
VALUES (N'Espresso', 4, 20000, N'Cà phê đen', N'espresso.jpg'); -- MaSP = 1
INSERT INTO tblSanPham (TenSP, MaDM, GiaBan, MoTa, AnhBia)
VALUES (N'Bạc xỉu', 4, 25000, N'Sữa pha với cà phê', N'coffee.jpg'); -- MaSP = 2
INSERT INTO tblSanPham (TenSP, MaDM, GiaBan, MoTa, AnhBia)
VALUES (N'Burger tôm', 1, 30000, N'Gồm bánh, thịt tôm, xà lách, cà chua, mayo', N'shrimpburger.jpg'); -- MaSP = 3
INSERT INTO tblSanPham (TenSP, MaDM, GiaBan, MoTa, AnhBia)
VALUES (N'Burger bò', 1, 35000, N'Gồm bánh, thịt bò, xà lách, cà chua, mayo, lát phô mai', N'beefburger.jpg'); -- MaSP = 4
INSERT INTO tblSanPham (TenSP, MaDM, GiaBan, MoTa, AnhBia)
VALUES (N'Trà chanh', 5, 20000, N'Trà chanh tươi', N'trachanh.jpg'); -- MaSP = 5
INSERT INTO tblSanPham (TenSP, MaDM, GiaBan, MoTa, AnhBia)
VALUES (N'Trà đào', 5, 25000, N'Trà đào cam xả', N'tradao.jpg'); -- MaSP = 6
INSERT INTO tblSanPham (TenSP, MaDM, GiaBan, MoTa, AnhBia)
VALUES (N'Gà rán truyền thống', 3, 35000, N'Gà rán truyền thống giòn', N'garan.jpg'); -- MaSP = 7
INSERT INTO tblSanPham (TenSP, MaDM, GiaBan, MoTa, AnhBia)
VALUES (N'Gà rán sốt BBQ', 3, 42000, N'Gà rán sốt BBQ và tiêu đen', N'garanbbq.jpg'); -- MaSP = 8
INSERT INTO tblSanPham (TenSP, MaDM, GiaBan, MoTa, AnhBia)
VALUES (N'Coke', 6, 15000, N'Nước ngọt có ga Coca Cola 500ml', N'coke.jpg'); -- MaSP = 9
INSERT INTO tblSanPham (TenSP, MaDM, GiaBan, MoTa, AnhBia)
VALUES (N'Sprite', 6, 12000, N'Nước ngọt có ga Sprite 500ml', N'sprite.jpg'); -- MaSP = 10
INSERT INTO tblSanPham (TenSP, MaDM, GiaBan, MoTa, AnhBia)
VALUES (N'Sandwich thịt nguội và trứng', 2, 25000, N'Bánh sandwich, thịt nguội, trứng, xà lách, sốt mayo', N'eggsandwich.jpg'); -- MaSP = 11
INSERT INTO tblSanPham (TenSP, MaDM, GiaBan, MoTa, AnhBia)
VALUES (N'Sandwich cá ngừ mayo', 2, 22000, N'Bánh sandwich, cá ngừ, xà lách, mayo', N'tunasandwich.jpg'); -- MaSP = 12
INSERT INTO tblSanPham (TenSP, MaDM, GiaBan, MoTa, AnhBia)
VALUES (N'Aquafina', 7, 15000, N'Nước suối Aquafina 500ml', N'aquafina.jpg'); -- MaSP = 13
INSERT INTO tblSanPham (TenSP, MaDM, GiaBan, MoTa, AnhBia)
VALUES (N'Dasani', 7, 12000, N'Nước suối Dasani 500ml', N'dasani.jpg'); -- MaSP = 14
INSERT INTO tblSanPham (TenSP, MaDM, GiaBan, MoTa, AnhBia)
VALUES (N'Sundae socola', 9, 30000, N'Kem tươi Sundae sốt sô cô la', N'chocosundae.jpg'); -- MaSP = 15
INSERT INTO tblSanPham (TenSP, MaDM, GiaBan, MoTa, AnhBia)
VALUES (N'Sundae dâu', 9, 30000, N'Kem tươi Sundae sốt dâu', N'sundaedau.jpg'); -- MaSP = 16
INSERT INTO tblSanPham (TenSP, MaDM, GiaBan, MoTa, AnhBia)
VALUES (N'Bánh trứng', 8, 18000, N'Bánh trứng nóng giòn', N'banhtrung.jpg'); -- MaSP = 17
INSERT INTO tblSanPham (TenSP, MaDM, GiaBan, MoTa, AnhBia)
VALUES (N'Cupcake', 8, 20000, N'Bánh cupcake hương việt quất', N'cupcake.jpg'); -- MaSP = 18

-- Hình ảnh
INSERT INTO tblHinhAnh (MaSP, TenHinh) VALUES (1, N'espresso1.jpg');
INSERT INTO tblHinhAnh (MaSP, TenHinh) VALUES (2, N'coffee1.jpg'); 
INSERT INTO tblHinhAnh (MaSP, TenHinh) VALUES (3, N'shrimpburger1.jpg');
INSERT INTO tblHinhAnh (MaSP, TenHinh) VALUES (4, N'beefburger1.jpg');
INSERT INTO tblHinhAnh (MaSP, TenHinh) VALUES (5, N'trachanh1.jpg');
INSERT INTO tblHinhAnh (MaSP, TenHinh) VALUES (6, N'tradao1.jpg'); 
INSERT INTO tblHinhAnh (MaSP, TenHinh) VALUES (7, N'garan1.jpg');
INSERT INTO tblHinhAnh (MaSP, TenHinh) VALUES (8, N'garanbbq1.jpg');
INSERT INTO tblHinhAnh (MaSP, TenHinh) VALUES (9, N'coke1.jpg');
INSERT INTO tblHinhAnh (MaSP, TenHinh) VALUES (10, N'sprite1.jpg');
INSERT INTO tblHinhAnh (MaSP, TenHinh) VALUES (11, N'eggsandwich1.jpg');
INSERT INTO tblHinhAnh (MaSP, TenHinh) VALUES (12, N'tunasandwich1.jpg');
INSERT INTO tblHinhAnh (MaSP, TenHinh) VALUES (13, N'aquafina1.jpg');
INSERT INTO tblHinhAnh (MaSP, TenHinh) VALUES (14, N'dasani1.jpg');
INSERT INTO tblHinhAnh (MaSP, TenHinh) VALUES (15, N'chocosundae1.jpg');
INSERT INTO tblHinhAnh (MaSP, TenHinh) VALUES (16, N'sundaedau1.jpg');
INSERT INTO tblHinhAnh (MaSP, TenHinh) VALUES (17, N'banhtrung1.jpg');
INSERT INTO tblHinhAnh (MaSP, TenHinh) VALUES (18, N'cupcake1.jpg');

-- Khách hàng 
INSERT INTO tblKhachHang (TenKH, GioiTinh, NamSinh, SoDienThoai, DiaChi, email, Avarta)
VALUES (N'Nguyễn Văn A', N'Nam', 1995, '0909123456', N'200 Nguyễn Xí, HCM', 'a@gmail.com', N'avt1.jpg'); -- MaKH = 1
INSERT INTO tblKhachHang (TenKH, GioiTinh, NamSinh, SoDienThoai, DiaChi, Email, Avarta)
VALUES (N'Lê Thị B', N'Nữ', 1998, '0911222333', N'15 Cầu Giấy, HN', 'b@gmail.com', N'avt2.jpg'); -- MaKH = 2
INSERT INTO tblKhachHang (TenKH, GioiTinh, NamSinh, SoDienThoai, DiaChi, Email, Avarta)
VALUES (N'Trần Minh K', N'Nam', 2000, '0987654321', N'100 Phan Văn Trị, Bình Thạnh', 'k@yahoo.com', N'avt3.jpg'); -- MaKH = 3

-- Vai trò 
INSERT INTO tblVaiTro (TenVaiTro, MoTa)
VALUES (N'Admin', N'Quản lý hệ thống'); -- MaVaiTro = 1
INSERT INTO tblVaiTro (TenVaiTro, MoTa)
VALUES (N'Nhân viên', N'Quản lý bán hàng'); -- MaVaiTro = 2
INSERT INTO tblVaiTro (TenVaiTro, MoTa)
VALUES (N'Khách hàng', N'Mua sản phẩm'); -- MaVaiTro = 3

-- Nhân viên 
INSERT INTO tblNhanVien (TenNV, GioiTinh, NamSinh, VaiTro)
VALUES (N'Phạm Văn C', N'Nam', 1990, 1); -- MaNV = 1
INSERT INTO tblNhanVien (TenNV, GioiTinh, NamSinh, VaiTro)
VALUES (N'Trần Thị D', N'Nữ', 1992, 2); -- MaNV = 2
INSERT INTO tblNhanVien (TenNV, GioiTinh, NamSinh, VaiTro)
VALUES (N'Đỗ Hoài E', N'Nữ', 1995, 2); -- MaNV = 3

-- Tình trạng 
INSERT INTO tblTinhTrang (TinhTrangHoaDon)
VALUES (N'Đang chờ xử lý'); -- ID = 1
INSERT INTO tblTinhTrang (TinhTrangHoaDon)
VALUES (N'Đã giao hàng'); -- ID = 2
INSERT INTO tblTinhTrang (TinhTrangHoaDon)
VALUES (N'Đã hủy'); -- ID = 3

-- Hóa đơn 
INSERT INTO tblHoaDon (MaKH, MaNV, NgayLap, TongTien, TinhTrang, DiaChiGiaoHang, DaThanhToan)
VALUES (1, 1, '2024-07-01 10:30:00', 40000, 2, N'200 Nguyễn Xí, HCM', 1); -- MaHD = 1
INSERT INTO tblHoaDon (MaKH, MaNV, NgayLap, TongTien, TinhTrang, DiaChiGiaoHang, DaThanhToan)
VALUES (2, 2, '2024-07-02 14:00:00', 250000, 2, N'15 Cầu Giấy, HN', 1); -- MaHD = 2 
INSERT INTO tblHoaDon (MaKH, MaNV, NgayLap, TongTien, TinhTrang, DiaChiGiaoHang, DaThanhToan)
VALUES (3, 2, '2024-07-05 08:15:00', 55000, 1, N'100 Phan Văn Trị, Bình Thạnh', 0); -- MaHD = 3 
INSERT INTO tblHoaDon (MaKH, MaNV, NgayLap, TongTien, TinhTrang, DiaChiGiaoHang, DaThanhToan)
VALUES (1, 3, '2024-07-10 11:45:00', 30000, 3, N'200 Nguyễn Xí, HCM', 0); -- MaHD = 4 

-- Chi tiết hóa đơn
-- MaHD = 1
INSERT INTO tblChiTietHoaDon (MaHD, MaSP, SoLuong, GiaBan)
VALUES (1, 1, 2, 20000);
-- MaHD = 2
INSERT INTO tblChiTietHoaDon (MaHD, MaSP, SoLuong, GiaBan)
VALUES (2, 2, 1, 250000);
-- MaHD = 3
INSERT INTO tblChiTietHoaDon (MaHD, MaSP, SoLuong, GiaBan)
VALUES (3, 4, 1, 35000);
INSERT INTO tblChiTietHoaDon (MaHD, MaSP, SoLuong, GiaBan)
VALUES (3, 5, 1, 20000);
-- MaHD = 4
INSERT INTO tblChiTietHoaDon (MaHD, MaSP, SoLuong, GiaBan)
VALUES (4, 3, 1, 30000);

--Taikhoan
insert into tbltaikhoan values
('admin', 'admin123', 1, null, 1),
('nv1', 'nv123', 2, null, 2),
('nv2', 'nv456', 3, null, 2),
('a@gmail.com', '123456', null, 1, 3),
('b@gmail.com', 'abcdef', null, 2, 3),
('k@yahoo.com', 'kieuhoa', null, 3, 3)

select * from tblKhachHang
select * from tbltaikhoan