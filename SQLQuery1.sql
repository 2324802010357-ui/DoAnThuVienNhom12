-- =============================================
-- SCRIPT TẠO DATABASE THƯ VIỆN - DoAnThuVienNhom12
-- Phiên bản: 1.0
-- Ngày tạo: 2025-01-03
-- =============================================

-- XÓA VÀ TẠO LẠI DATABASE
USE master;
GO

IF EXISTS (SELECT name FROM sys.databases WHERE name = 'DoAnThuVienNhom12')
BEGIN
    ALTER DATABASE DoAnThuVienNhom12 SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE DoAnThuVienNhom12;
END
GO

CREATE DATABASE DoAnThuVienNhom12;
GO

USE DoAnThuVienNhom12;
GO

-- =============================================
-- PHẦN 1: TẠO CÁC BẢNG CƠ BẢN
-- =============================================

-- Bảng Vai Trò (Phân quyền)
CREATE TABLE VaiTro (
    MaVaiTro INT PRIMARY KEY IDENTITY(1,1),
    TenVaiTro NVARCHAR(50) NOT NULL UNIQUE,
    MoTa NVARCHAR(255),
    NgayTao DATETIME DEFAULT GETDATE()
);

-- Bảng Người Dùng
CREATE TABLE NguoiDung (
    MaNguoiDung INT PRIMARY KEY IDENTITY(1,1),
    TenDangNhap NVARCHAR(50) UNIQUE NOT NULL,
    MatKhau NVARCHAR(255) NOT NULL,
    HoTen NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100),
    SoDienThoai NVARCHAR(20),
    MaVaiTro INT,
    TrangThai BIT DEFAULT 1,
    NgayTao DATETIME DEFAULT GETDATE(),
    LanDangNhapCuoi DATETIME,
    FOREIGN KEY (MaVaiTro) REFERENCES VaiTro(MaVaiTro)
);

-- Bảng Danh Mục Sách
CREATE TABLE DanhMuc (
    MaDanhMuc INT PRIMARY KEY IDENTITY(1,1),
    TenDanhMuc NVARCHAR(100) NOT NULL,
    MoTa NVARCHAR(500),
    TrangThai NVARCHAR(50) DEFAULT N'Hoạt động',
    NgayTao DATETIME DEFAULT GETDATE()
);

-- Bảng Nhà Xuất Bản
CREATE TABLE NhaXuatBan (
    MaNXB INT PRIMARY KEY IDENTITY(1,1),
    TenNXB NVARCHAR(200) NOT NULL,
    DiaChi NVARCHAR(255),
    SoDienThoai NVARCHAR(20),
    Email NVARCHAR(100),
    Website NVARCHAR(200)
);

-- Bảng Tác Giả
CREATE TABLE TacGia (
    MaTacGia INT PRIMARY KEY IDENTITY(1,1),
    TenTacGia NVARCHAR(100) NOT NULL,
    GioiTinh NVARCHAR(10),
    NamSinh INT,
    QuocTich NVARCHAR(50),
    TieuSu NVARCHAR(MAX)
);

-- Bảng Kho
CREATE TABLE Kho (
    MaKho INT PRIMARY KEY IDENTITY(1,1),
    TenKho NVARCHAR(100) NOT NULL,
    DiaChi NVARCHAR(255),
    SucChua INT,
    SoLuongHienTai INT DEFAULT 0,
    MoTa NVARCHAR(255),
    TrangThai NVARCHAR(50) DEFAULT N'Hoạt động'
);

-- Bảng Kệ Sách
CREATE TABLE KeSach (
    MaKe INT PRIMARY KEY IDENTITY(1,1),
    TenKe NVARCHAR(50) NOT NULL,
    MaKho INT,
    ViTri NVARCHAR(100),
    SucChua INT,
    SoLuongHienTai INT DEFAULT 0,
    FOREIGN KEY (MaKho) REFERENCES Kho(MaKho)
);

-- Bảng Sách
CREATE TABLE Sach (
    MaSach INT PRIMARY KEY IDENTITY(1,1),
    TenSach NVARCHAR(255) NOT NULL,
    MaDanhMuc INT,
    MaTacGia INT,
    MaNXB INT,
    NamXuatBan INT,
    SoTrang INT,
    NgonNgu NVARCHAR(50) DEFAULT N'Tiếng Việt',
    GiaNhap DECIMAL(18,2),
    GiaBia DECIMAL(18,2),
MoTa NVARCHAR(MAX),
    TuKhoa NVARCHAR(500),
    AnhBia NVARCHAR(500),
    MaKe INT,
    SoLuong INT DEFAULT 0,
    SoLuongCoSan INT DEFAULT 0,
    LuotMuon INT DEFAULT 0,
    LuotXem INT DEFAULT 0,
    TinhTrang NVARCHAR(50) DEFAULT N'Còn hàng',
    NgayNhap DATETIME DEFAULT GETDATE(),
    NgayCapNhat DATETIME,
    FOREIGN KEY (MaDanhMuc) REFERENCES DanhMuc(MaDanhMuc),
    FOREIGN KEY (MaTacGia) REFERENCES TacGia(MaTacGia),
    FOREIGN KEY (MaNXB) REFERENCES NhaXuatBan(MaNXB),
    FOREIGN KEY (MaKe) REFERENCES KeSach(MaKe)
);

-- Bảng Độc Giả
CREATE TABLE DocGia (
    MaDocGia INT PRIMARY KEY IDENTITY(1,1),
    HoTen NVARCHAR(100) NOT NULL,
    MaSinhVien NVARCHAR(20),
    MaLop NVARCHAR(20),
    NgaySinh DATE,
    GioiTinh NVARCHAR(10),
    CMND NVARCHAR(20),
    DiaChi NVARCHAR(255),
    SoDienThoai NVARCHAR(20),
    Email NVARCHAR(100),
    NgayDangKy DATETIME DEFAULT GETDATE(),
    NgayHetHan DATETIME,
    TrangThai NVARCHAR(50) DEFAULT N'Hoạt động',
    SoSachDangMuon INT DEFAULT 0,
    TongSachDaMuon INT DEFAULT 0
);

-- Bảng Nhân Viên
CREATE TABLE NhanVien (
    MaNhanVien INT PRIMARY KEY IDENTITY(1,1),
    HoTen NVARCHAR(100) NOT NULL,
    NgaySinh DATE,
    GioiTinh NVARCHAR(10),
    CMND NVARCHAR(20),
    DiaChi NVARCHAR(255),
    SoDienThoai NVARCHAR(20),
    Email NVARCHAR(100),
    ChucVu NVARCHAR(50),
    PhongBan NVARCHAR(100),
    NgayVaoLam DATETIME DEFAULT GETDATE(),
    Luong DECIMAL(18,2),
    TrangThai NVARCHAR(50) DEFAULT N'Đang làm việc'
);

-- Bảng Phiếu Mượn
CREATE TABLE PhieuMuon (
    MaPhieuMuon INT PRIMARY KEY IDENTITY(1,1),
    MaDocGia INT,
    MaNhanVien INT,
    NgayMuon DATETIME DEFAULT GETDATE(),
    NgayHenTra DATETIME,
    NgayTraThucTe DATETIME,
    TrangThai NVARCHAR(50) DEFAULT N'Đang mượn',
    SoLuongSach INT DEFAULT 0,
    PhiPhat DECIMAL(18,2) DEFAULT 0,
    GhiChu NVARCHAR(500),
    FOREIGN KEY (MaDocGia) REFERENCES DocGia(MaDocGia),
    FOREIGN KEY (MaNhanVien) REFERENCES NhanVien(MaNhanVien)
);

-- Bảng Chi Tiết Phiếu Mượn
CREATE TABLE ChiTietPhieuMuon (
    MaChiTiet INT PRIMARY KEY IDENTITY(1,1),
    MaPhieuMuon INT,
    MaSach INT,
    SoLuong INT DEFAULT 1,
    TinhTrangMuon NVARCHAR(50) DEFAULT N'Tốt',
    TinhTrangTra NVARCHAR(50),
    GhiChu NVARCHAR(255),
    FOREIGN KEY (MaPhieuMuon) REFERENCES PhieuMuon(MaPhieuMuon),
    FOREIGN KEY (MaSach) REFERENCES Sach(MaSach)
);

-- Bảng Phiếu Trả
CREATE TABLE PhieuTra (
    MaPhieuTra INT PRIMARY KEY IDENTITY(1,1),
    MaPhieuMuon INT,
    NgayTra DATETIME DEFAULT GETDATE(),
    SoNgayTre INT DEFAULT 0,
    TienPhat DECIMAL(18,2) DEFAULT 0,
    LyDoPhiPhat NVARCHAR(500),
    GhiChu NVARCHAR(500),
    FOREIGN KEY (MaPhieuMuon) REFERENCES PhieuMuon(MaPhieuMuon)
);

-- Bảng Lịch Sử Nhập Kho
CREATE TABLE PhieuNhapKho (
MaPhieu INT PRIMARY KEY IDENTITY(1,1),
    NgayNhap DATETIME DEFAULT GETDATE(),
    NhaCungCap NVARCHAR(200),
    NguoiNhap NVARCHAR(100),
    MaNhanVien INT,
    SoLuongSach INT DEFAULT 0,
    TongTien DECIMAL(18,2) DEFAULT 0,
    GhiChu NVARCHAR(500),
    FOREIGN KEY (MaNhanVien) REFERENCES NhanVien(MaNhanVien)
);

-- Bảng Chi Tiết Phiếu Nhập
CREATE TABLE ChiTietPhieuNhap (
    MaChiTiet INT PRIMARY KEY IDENTITY(1,1),
    MaPhieu INT,
    MaSach INT,
    SoLuong INT,
    DonGia DECIMAL(18,2),
    ThanhTien DECIMAL(18,2),
    FOREIGN KEY (MaPhieu) REFERENCES PhieuNhapKho(MaPhieu),
    FOREIGN KEY (MaSach) REFERENCES Sach(MaSach)
);

-- =============================================
-- PHẦN 2: INSERT DỮ LIỆU MẪU
-- =============================================

-- Vai trò
INSERT INTO VaiTro (TenVaiTro, MoTa) VALUES
(N'Admin', N'Quản trị viên hệ thống - Toàn quyền'),
(N'Thủ thư', N'Nhân viên thư viện - Quản lý mượn trả, sách, độc giả'),
(N'Độc giả', N'Người mượn sách - Xem và mượn sách');

-- Người dùng
INSERT INTO NguoiDung (TenDangNhap, MatKhau, HoTen, Email, SoDienThoai, MaVaiTro, TrangThai) VALUES
('admin', '123456', N'Nguyễn Văn Admin', 'admin@library.com', '0901234567', 1, 1),
('thuthu1', '123456', N'Trần Thị Thủ Thư', 'thuthu1@library.com', '0912345678', 2, 1),
('thuthu2', '123456', N'Lê Văn Thư', 'thuthu2@library.com', '0923456789', 2, 1),
('docgia1', '123456', N'Phạm Thị Lan', 'docgia1@gmail.com', '0934567890', 3, 1);

-- Danh mục
INSERT INTO DanhMuc (TenDanhMuc, MoTa, TrangThai) VALUES
(N'Công nghệ thông tin', N'Lập trình, CNTT, AI, Blockchain', N'Hoạt động'),
(N'Văn học Việt Nam', N'Sách văn học trong nước', N'Hoạt động'),
(N'Văn học nước ngoài', N'Sách văn học dịch', N'Hoạt động'),
(N'Kinh tế', N'Kinh tế, quản trị, marketing', N'Hoạt động'),
(N'Tâm lý - Kỹ năng sống', N'Phát triển bản thân, kỹ năng mềm', N'Hoạt động'),
(N'Ngoại ngữ', N'Tiếng Anh, Nhật, Trung, Hàn', N'Hoạt động'),
(N'Khoa học tự nhiên', N'Toán, lý, hóa, sinh', N'Hoạt động'),
(N'Lịch sử - Địa lý', N'Lịch sử VN và thế giới', N'Hoạt động'),
(N'Thiếu nhi', N'Sách cho trẻ em', N'Hoạt động'),
(N'Y học - Sức khỏe', N'Y học, chăm sóc sức khỏe', N'Hoạt động');

-- Nhà xuất bản
INSERT INTO NhaXuatBan (TenNXB, DiaChi, SoDienThoai, Email) VALUES
(N'NXB Trẻ', N'161B Lý Chính Thắng, Q.3, TP.HCM', '028 39316211', 'nxbtre@nxbtre.com.vn'),
(N'NXB Kim Đồng', N'55 Quang Trung, Hai Bà Trưng, Hà Nội', '024 39434730', 'info@nxbkimdong.com.vn'),
(N'NXB Văn học', N'18 Nguyễn Trường Tộ, Ba Đình, Hà Nội', '024 38234560', 'info@nxbvanhoc.com.vn'),
(N'NXB Lao động', N'175 Giảng Võ, Đống Đa, Hà Nội', '024 38514018', 'nxbld@nxblaodong.com.vn'),
(N'NXB Thế Giới', N'7 Nguyễn Thị Minh Khai, Q.1, TP.HCM', '028 38225340', 'thegioi@thegioipublishers.vn'),
(N'NXB Tổng hợp TP.HCM', N'62 Nguyễn Thị Minh Khai, Q.3, TP.HCM', '028 39316211', 'info@nxbhcm.com.vn'),
(N'NXB Đại học Quốc gia', N'16 Hàng Chuối, Hai Bà Trưng, Hà Nội', '024 39719171', 'info@vnupress.vn'),
(N'NXB Thanh Niên', N'64B Nguyễn Thị Minh Khai, Q.3, TP.HCM', '028 38220160', 'thanhnien@nxbthanhnien.vn');

-- Tác giả
INSERT INTO TacGia (TenTacGia, GioiTinh, NamSinh, QuocTich, TieuSu) VALUES
(N'Nguyễn Nhật Ánh', N'Nam', 1955, N'Việt Nam', N'Nhà văn nổi tiếng với các tác phẩm thiếu nhi'),
(N'Paulo Coelho', N'Nam', 1947, N'Brazil', N'Tác giả "Nhà giả kim"'),
(N'Dale Carnegie', N'Nam', 1888, N'Mỹ', N'Tác giả "Đắc nhân tâm"'),
(N'Robert Kiyosaki', N'Nam', 1947, N'Mỹ', N'Tác giả "Dạy con làm giàu"'),
(N'Haruki Murakami', N'Nam', 1949, N'Nhật Bản', N'Nhà văn đương đại Nhật Bản'),
(N'Tony Buổi Sáng', N'Nam', 1975, N'Việt Nam', N'Blogger, tác giả sách kỹ năng sống'),
(N'Kazami Yuki', N'Nữ', 1985, N'Nhật Bản', N'Chuyên gia lập trình'),
(N'Nguyễn Đức Nghĩa', N'Nam', 1978, N'Việt Nam', N'Giảng viên CNTT'),
(N'J.K. Rowling', N'Nữ', 1965, N'Anh', N'Tác giả Harry Potter'),
(N'Dan Brown', N'Nam', 1964, N'Mỹ', N'Tác giả tiểu thuyết trinh thám');

-- Kho
INSERT INTO Kho (TenKho, DiaChi, SucChua, MoTa, TrangThai) VALUES
(N'Kho A - Tầng 1', N'Tầng 1, Thư viện Trung tâm', 5000, N'Kho chính', N'Hoạt động'),
(N'Kho B - Tầng 2', N'Tầng 2, Thư viện Trung tâm', 3000, N'Kho phụ', N'Hoạt động'),
(N'Kho C - Tầng 3', N'Tầng 3, Thư viện Trung tâm', 2000, N'Kho lưu trữ', N'Hoạt động');

-- Kệ sách
INSERT INTO KeSach (TenKe, MaKho, ViTri, SucChua) VALUES
(N'Kệ A1', 1, N'Góc trái', 200),
(N'Kệ A2', 1, N'Góc phải', 200),
(N'Kệ B1', 2, N'Giữa phòng', 150),
(N'Kệ B2', 2, N'Cạnh cửa sổ', 150),
(N'Kệ C1', 3, N'Khu vực A', 100),
(N'Kệ C2', 3, N'Khu vực B', 100);

-- Nhân viên
INSERT INTO NhanVien (HoTen, NgaySinh, GioiTinh, CMND, DiaChi, SoDienThoai, Email, ChucVu, PhongBan, Luong, TrangThai) VALUES
(N'Nguyễn Văn An', '1985-05-15', N'Nam', '001085012345', N'123 Lê Lợi, Q.1, TP.HCM', '0901234567', 'nvan@lib.vn', N'Thủ thư trưởng', N'Phòng Quản lý', 12000000, N'Đang làm việc'),
(N'Trần Thị Bình', '1990-08-20', N'Nữ', '001090054321', N'456 Nguyễn Huệ, Q.1, TP.HCM', '0912345678', 'ttbinh@lib.vn', N'Thủ thư', N'Phòng Mượn trả', 8000000, N'Đang làm việc'),
(N'Lê Văn Cường', '1988-12-10', N'Nam', '001088098765', N'789 Trần Hưng Đạo, Q.5, TP.HCM', '0923456789', 'lvcuong@lib.vn', N'Thủ thư', N'Phòng Mượn trả', 8000000, N'Đang làm việc');

-- Độc giả
INSERT INTO DocGia (HoTen, MaSinhVien, MaLop, NgaySinh, GioiTinh, DiaChi, SoDienThoai, Email, NgayHetHan, TrangThai) VALUES
(N'Nguyễn Văn Anh', '2324802010357', 'DH23TT01', '2000-03-15', N'Nam', N'12 Lý Tự Trọng, Q.1, TP.HCM', '0934567890', 'nvanh@gmail.com', DATEADD(YEAR, 1, GETDATE()), N'Hoạt động'),
(N'Trần Thị Bảo', '2324802010358', 'DH23TT01', '2000-07-22', N'Nữ', N'34 Điện Biên Phủ, Q.3, TP.HCM', '0945678901', 'ttbao@gmail.com', DATEADD(YEAR, 1, GETDATE()), N'Hoạt động'),
(N'Lê Văn Cường', '2324802010359', 'DH23TT02', '2001-11-30', N'Nam', N'56 Cách Mạng Tháng 8, Q.10, TP.HCM', '0956789012', 'lvcuong@gmail.com', DATEADD(YEAR, 1, GETDATE()), N'Hoạt động');

-- =============================================
-- PHẦN 3: INSERT 40 CUỐN SÁCH VỚI ẢNH THẬT
-- =============================================

INSERT INTO Sach (TenSach, MaDanhMuc, MaTacGia, MaNXB, NamXuatBan, SoTrang, NgonNgu, GiaNhap, GiaBia, MoTa, TuKhoa, AnhBia, MaKe, SoLuong, SoLuongCoSan, LuotMuon, LuotXem, TinhTrang) VALUES
-- CNTT
(N'Lập trình C# nâng cao', 1, 7, 7, 2021, 512, N'Tiếng Việt', 140000, 189000, N'Giáo trình lập trình C# từ cơ bản đến nâng cao, đầy đủ và chi tiết', N'C#, .NET, Lập trình', 'https://cdn0.fahasa.com/media/catalog/product/l/a/lap-trinh-c-sharp.jpg', 1, 50, 45, 156, 1234, N'Còn hàng'),
(N'Cấu trúc dữ liệu & Giải thuật', 1, 8, 7, 2020, 548, N'Tiếng Việt', 167000, 225000, N'DSA với C/C++, các thuật toán cơ bản và nâng cao', N'DSA, Thuật toán, Cấu trúc dữ liệu', 'https://cdn0.fahasa.com/media/catalog/product/c/a/cau-truc-du-lieu.jpg', 1, 30, 28, 189, 987, N'Còn hàng'),
(N'Python cho người mới bắt đầu', 1, 8, 7, 2021, 456, N'Tiếng Việt', 130000, 175000, N'Học Python từ đầu, dễ hiểu và thực tế', N'Python, Lập trình Python', 'https://cdn0.fahasa.com/media/catalog/product/p/y/python-cho-moi-nguoi.jpg', 1, 40, 35, 267, 1456, N'Còn hàng'),
(N'Lập trình Java Core', 1, 8, 7, 2020, 624, N'Tiếng Việt', 159000, 215000, N'Java cơ bản và nâng cao, OOP đầy đủ', N'Java, OOP, Lập trình', 'https://cdn0.fahasa.com/media/catalog/product/l/a/lap-trinh-java.jpg', 1, 35, 30, 198, 890, N'Còn hàng'),
(N'Lập trình Web với ASP.NET', 1, 7, 7, 2021, 486, N'Tiếng Việt', 147000, 198000, N'Xây dựng ứng dụng web với ASP.NET MVC', N'ASP.NET, Web, MVC', 'https://cdn0.fahasa.com/media/catalog/product/a/s/asp-net.jpg', 1, 25, 22, 156, 678, N'Còn hàng'),
(N'Cơ sở dữ liệu SQL Server', 1, 8, 7, 2020, 428, N'Tiếng Việt', 137000, 185000, N'Thiết kế và quản trị CSDL SQL Server', N'SQL Server, Database, CSDL', 'https://cdn0.fahasa.com/media/catalog/product/c/o/co-so-du-lieu.jpg', 1, 30, 25, 201, 756, N'Còn hàng'),
(N'React JS từ cơ bản đến nâng cao', 1, 8, 7, 2021, 428, N'Tiếng Việt', 137000, 185000, N'Lập trình React JS, hooks và Redux', N'React, Javascript, Frontend', 'https://cdn0.fahasa.com/media/catalog/product/r/e/react-js.jpg', 1, 28, 24, 189, 834, N'Còn hàng'),
(N'Node.js Backend Development', 1, 8, 7, 2021, 456, N'Tiếng Việt', 145000, 195000, N'Lập trình backend với Node.js và Express', N'Node.js, Backend, Express', 'https://cdn0.fahasa.com/media/catalog/product/n/o/nodejs.jpg', 1, 22, 19, 178, 623, N'Còn hàng'),
(N'AI - Trí tuệ nhân tạo', 1, 8, 7, 2021, 468, N'Tiếng Việt', 145000, 195000, N'Machine Learning và Deep Learning cơ bản', N'AI, Machine Learning, Deep Learning', 'https://cdn0.fahasa.com/media/catalog/product/a/i/ai-tri-tue-nhan-tao.jpg', 1, 20, 18, 167, 945, N'Còn hàng'),
(N'Blockchain và Cryptocurrency', 1, 8, 7, 2021, 386, N'Tiếng Việt', 130000, 175000, N'Công nghệ Blockchain và ứng dụng', N'Blockchain, Bitcoin, Crypto', 'https://cdn0.fahasa.com/media/catalog/product/b/l/blockchain.jpg', 1, 18, 16, 145, 534, N'Còn hàng'),

-- Văn học Việt Nam
(N'Tôi thấy hoa vàng trên cỏ xanh', 2, 1, 1, 2010, 368, N'Tiếng Việt', 70000, 95000, N'Truyện dài của Nguyễn Nhật Ánh về tuổi thơ miền Trung', N'Văn học, Nguyễn Nhật Ánh', 'https://cdn0.fahasa.com/media/catalog/product/8/9/8934974157373.jpg', 2, 25, 20, 256, 2134, N'Còn hàng'),
(N'Mắt biếc', 2, 1, 1, 2007, 272, N'Tiếng Việt', 62000, 85000, N'Câu chuyện tình buồn của tuổi học trò', N'Văn học, Tình yêu', 'https://cdn0.fahasa.com/media/catalog/product/m/a/mat_biec_tb_2020.jpg', 2, 30, 25, 303, 2456, N'Còn hàng'),
(N'Cho tôi xin một vé đi tuổi thơ', 2, 1, 1, 2013, 284, N'Tiếng Việt', 66000, 90000, N'Hồi ức tuổi thơ đầy cảm xúc', N'Văn học, Tuổi thơ', 'https://cdn0.fahasa.com/media/catalog/product/c/h/cho-toi-xin-mot-ve-di-tuoi-tho.jpg', 2, 28, 23, 298, 1876, N'Còn hàng'),
(N'Tôi là Bêtô', 2, 1, 1, 2009, 368, N'Tiếng Việt', 62000, 85000, N'Truyện thiếu nhi đầy cảm động', N'Văn học, Thiếu nhi', 'https://cdn0.fahasa.com/media/catalog/product/t/o/toi_la_beto.jpg', 2, 20, 17, 234, 1456, N'Còn hàng'),

-- Văn học nước ngoài
(N'Nhà giả kim', 3, 2, 1, 2013, 227, N'Tiếng Việt', 59000, 79000, N'Hành trình tìm kiếm ước mơ của cậu bé chăn cừu', N'Văn học, Paulo Coelho, Triết lý', 'https://cdn0.fahasa.com/media/catalog/product/n/h/nha-gia-kim.jpg', 2, 40, 32, 512, 3456, N'Còn hàng'),
(N'Rừng Na Uy', 3, 5, 5, 2014, 420, N'Tiếng Việt', 92000, 125000, N'Tiểu thuyết tình yêu Nhật Bản', N'Văn học, Haruki Murakami, Nhật Bản', 'https://cdn0.fahasa.com/media/catalog/product/r/u/rung-na-uy.jpg', 2, 22, 18, 267, 1876, N'Còn hàng'),
(N'Mã Da Vinci', 3, 10, 5, 2015, 512, N'Tiếng Việt', 122000, 165000, N'Tiểu thuyết trinh thám ly kỳ', N'Văn học, Trinh thám, Dan Brown', 'https://cdn0.fahasa.com/media/catalog/product/m/a/ma-da-vinci.jpg', 2, 18, 15, 289, 1654, N'Còn hàng'),
(N'Harry Potter và hòn đá phù thủy', 3, 9, 1, 2017, 368, N'Tiếng Việt', 89000, 120000, N'Phần đầu tiên của series Harry Potter', N'Văn học, Phép thuật, Harry Potter', 'https://cdn0.fahasa.com/media/catalog/product/h/a/harry-potter-va-hon-da-phu-thuy.jpg', 2, 35, 28, 434, 2876, N'Còn hàng'),

-- Kinh tế
(N'Đắc nhân tâm', 5, 3, 4, 2018, 320, N'Tiếng Việt', 63000, 86000, N'Nghệ thuật giao tiếp và ứng xử', N'Kỹ năng, Giao tiếp, Dale Carnegie', 'https://cdn0.fahasa.com/media/catalog/product/d/a/dac-nhan-tam.jpg', 3, 50, 40, 645, 4567, N'Còn hàng'),
(N'Dạy con làm giàu', 4, 4, 4, 2019, 266, N'Tiếng Việt', 70000, 95000, N'Tài chính cá nhân cho người trẻ', N'Tài chính, Làm giàu, Robert Kiyosaki', 'https://cdn0.fahasa.com/media/catalog/product/d/a/day-con-lam-giau-tap-1.jpg', 3, 35, 28, 434, 2987, N'Còn hàng'),
(N'Nghĩ giàu làm giàu', 4, 4, 4, 2017, 356, N'Tiếng Việt', 92000, 125000, N'Tư duy làm giàu từ Napoleon Hill', N'Làm giàu, Tư duy, Thành công', 'https://cdn0.fahasa.com/media/catalog/product/n/g/nghi-giau-lam-giau.jpg', 3, 28, 23, 345, 2134, N'Còn hàng'),
(N'Từ tốt đến vĩ đại', 4, 4, 4, 2019, 428, N'Tiếng Việt', 122000, 165000, N'Quản trị doanh nghiệp xuất sắc', N'Quản trị, Doanh nghiệp, Lãnh đạo', 'https://cdn0.fahasa.com/media/catalog/product/t/u/tu-tot-den-vi-dai.jpg', 3, 22, 18, 289, 1765, N'Còn hàng'),
(N'Marketing 4.0', 4, 4, 7, 2020, 386, N'Tiếng Việt', 115000, 155000, N'Marketing thời đại số', N'Marketing, Digital, Kinh doanh', 'https://cdn0.fahasa.com/media/catalog/product/m/a/marketing-4.0.jpg', 3, 20, 17, 256, 1543, N'Còn hàng'),

-- Tâm lý - Kỹ năng sống
(N'Quẳng gánh lo đi và vui sống', 5, 3, 4, 2017, 368, N'Tiếng Việt', 68000, 92000, N'Kỹ năng sống tích cực', N'Kỹ năng sống, Tích cực, Hạnh phúc', 'https://cdn0.fahasa.com/media/catalog/product/q/u/quang-ganh-lo-di-va-vui-song.jpg', 3, 38, 32, 478, 3234, N'Còn hàng'),
(N'Đời ngắn đừng ngủ dài', 5, 6, 6, 2016, 312, N'Tiếng Việt', 70000, 95000, N'Động lực thành công cho giới trẻ', N'Động lực, Thành công, Tony Buổi Sáng', 'https://cdn0.fahasa.com/media/catalog/product/d/o/doi-ngan-dung-ngu-dai.jpg', 3, 32, 27, 423, 2876, N'Còn hàng'),
(N'Cà phê cùng Tony', 5, 6, 6, 2017, 356, N'Tiếng Việt', 78000, 105000, N'Kỹ năng sống và tư duy', N'Kỹ năng, Tư duy, Cuộc sống', 'https://cdn0.fahasa.com/media/catalog/product/c/a/ca-phe-cung-tony.jpg', 3, 26, 22, 367, 2345, N'Còn hàng'),
(N'999 lá thư gửi cho chính mình', 5, 6, 1, 2017, 456, N'Tiếng Việt', 87000, 118000, N'Thư tâm tình cho giới trẻ', N'Tâm lý, Tuổi trẻ, Tâm sự', 'https://cdn0.fahasa.com/media/catalog/product/9/9/999-la-thu.jpg', 3, 24, 20, 387, 2543, N'Còn hàng'),
(N'Sức mạnh của thói quen', 5, 3, 4, 2018, 428, N'Tiếng Việt', 110000, 148000, N'Xây dựng thói quen tốt', N'Thói quen, Kỷ luật, Thành công', 'https://cdn0.fahasa.com/media/catalog/product/s/u/suc-manh-cua-thoi-quen.jpg', 3, 28, 24, 423, 2765, N'Còn hàng'),
(N'Tư duy nhanh và chậm', 5, 3, 4, 2019, 512, N'Tiếng Việt', 130000, 175000, N'Tâm lý học nhận thức', N'Tâm lý học, Tư duy, Nhận thức', 'https://cdn0.fahasa.com/media/catalog/product/t/u/tu-duy-nhanh-va-cham.jpg', 3, 22, 19, 356, 2234, N'Còn hàng'),

-- Ngoại ngữ
(N'Tiếng Anh giao tiếp cơ bản', 6, 3, 4, 2019, 368, N'Song ngữ', 92000, 125000, N'Học tiếng Anh giao tiếp hiệu quả', N'Tiếng Anh, Giao tiếp, Học ngoại ngữ', 'https://cdn0.fahasa.com/media/catalog/product/t/i/tieng-anh-giao-tiep.jpg', 4, 45, 38, 512, 3876, N'Còn hàng'),
(N'Grammar in Use', 6, 3, 4, 2020, 428, N'Tiếng Anh', 107000, 145000, N'Ngữ pháp tiếng Anh toàn diện', N'Tiếng Anh, Ngữ pháp, Grammar', 'https://cdn0.fahasa.com/media/catalog/product/g/r/grammar-in-use.jpg', 4, 40, 34, 489, 3234, N'Còn hàng'),
(N'TOEIC Listening & Reading', 6, 3, 4, 2021, 512, N'Song ngữ', 132000, 178000, N'Luyện thi TOEIC hiệu quả', N'TOEIC, Tiếng Anh, Luyện thi', 'https://cdn0.fahasa.com/media/catalog/product/t/o/toeic.jpg', 4, 35, 30, 456, 2987, N'Còn hàng'),
(N'Tiếng Nhật sơ cấp', 6, 3, 4, 2020, 386, N'Song ngữ', 100000, 135000, N'Học tiếng Nhật từ cơ bản', N'Tiếng Nhật, Nhật ngữ, N5', 'https://cdn0.fahasa.com/media/catalog/product/t/i/tieng-nhat-so-cap.jpg', 4, 30, 26, 378, 2456, N'Còn hàng'),

-- Khoa học
(N'Sapiens: Lược sử loài người', 7, 2, 3, 2019, 543, N'Tiếng Việt', 140000, 189000, N'Lịch sử nhân loại từ thời tiền sử', N'Lịch sử, Nhân loại, Tiến hóa', 'https://cdn0.fahasa.com/media/catalog/product/s/a/sapiens.jpg', 5, 25, 21, 467, 3456, N'Còn hàng'),
(N'Lược sử thời gian', 7, 2, 3, 2018, 256, N'Tiếng Việt', 100000, 135000, N'Vũ trụ và vật lý lý thuyết', N'Vũ trụ, Vật lý, Khoa học', 'https://cdn0.fahasa.com/media/catalog/product/l/u/luoc-su-thoi-gian.jpg', 5, 18, 15, 298, 2134, N'Còn hàng'),

-- Thiếu nhi
(N'Doraemon tập 1', 9, 1, 2, 2020, 196, N'Tiếng Việt', 18000, 25000, N'Truyện tranh thiếu nhi nổi tiếng', N'Truyện tranh, Thiếu nhi, Doraemon', 'https://cdn0.fahasa.com/media/catalog/product/d/o/doremon-tap-1.jpg', 6, 60, 50, 656, 4567, N'Còn hàng'),
(N'Thần đồng đất Việt', 9, 1, 2, 2018, 224, N'Tiếng Việt', 38000, 52000, N'Truyện cổ tích Việt Nam', N'Cổ tích, Thiếu nhi, Việt Nam', 'https://cdn0.fahasa.com/media/catalog/product/t/h/than-dong-dat-viet.jpg', 6, 35, 30, 478, 2987, N'Còn hàng'),

-- Y học
(N'Sống khỏe mỗi ngày', 10, 3, 3, 2021, 268, N'Tiếng Việt', 72000, 98000, N'Lối sống lành mạnh và khoa học', N'Sức khỏe, Y học, Sống khỏe', 'https://cdn0.fahasa.com/media/catalog/product/s/o/song-khoe-moi-ngay.jpg', 5, 22, 19, 356, 2234, N'Còn hàng'),
(N'Yoga cho sức khỏe', 10, 3, 3, 2018, 298, N'Tiếng Việt', 92000, 125000, N'Tập yoga hiệu quả cho mọi lứa tuổi', N'Yoga, Sức khỏe, Thể dục', 'https://cdn0.fahasa.com/media/catalog/product/y/o/yoga.jpg', 5, 20, 17, 334, 1987, N'Còn hàng');

-- =============================================
-- PHẦN 4: TẠO STORED PROCEDURES VÀ VIEWS
-- =============================================

-- SP Tìm kiếm sách
GO
CREATE PROCEDURE sp_TimKiemSach
    @TuKhoa NVARCHAR(255) = NULL,
    @MaDanhMuc INT = NULL,
    @MaTacGia INT = NULL
AS
BEGIN
    SELECT s.*, dm.TenDanhMuc, tg.TenTacGia, nxb.TenNXB, ks.TenKe, k.TenKho
    FROM Sach s
    LEFT JOIN DanhMuc dm ON s.MaDanhMuc = dm.MaDanhMuc
    LEFT JOIN TacGia tg ON s.MaTacGia = tg.MaTacGia
    LEFT JOIN NhaXuatBan nxb ON s.MaNXB = nxb.MaNXB
    LEFT JOIN KeSach ks ON s.MaKe = ks.MaKe
    LEFT JOIN Kho k ON ks.MaKho = k.MaKho
    WHERE (@TuKhoa IS NULL OR s.TenSach LIKE N'%' + @TuKhoa + '%' 
           OR tg.TenTacGia LIKE N'%' + @TuKhoa + '%'
           OR s.TuKhoa LIKE N'%' + @TuKhoa + '%')
      AND (@MaDanhMuc IS NULL OR s.MaDanhMuc = @MaDanhMuc)
      AND (@MaTacGia IS NULL OR s.MaTacGia = @MaTacGia)
    ORDER BY s.LuotXem DESC, s.LuotMuon DESC;
END;
GO

-- SP Mượn sách
CREATE PROCEDURE sp_MuonSach
    @MaDocGia INT,
    @MaNhanVien INT,
    @MaSach INT,
    @SoLuong INT = 1,
    @NgayHenTra DATETIME
AS
BEGIN
    BEGIN TRANSACTION;
    BEGIN TRY
        -- Kiểm tra số lượng sách
        DECLARE @SoLuongCoSan INT;
        SELECT @SoLuongCoSan = SoLuongCoSan FROM Sach WHERE MaSach = @MaSach;
        
        IF @SoLuongCoSan >= @SoLuong
        BEGIN
            -- Tạo phiếu mượn
            DECLARE @MaPhieuMuon INT;
            INSERT INTO PhieuMuon (MaDocGia, MaNhanVien, NgayHenTra, TrangThai, SoLuongSach)
            VALUES (@MaDocGia, @MaNhanVien, @NgayHenTra, N'Đang mượn', @SoLuong);
            
            SET @MaPhieuMuon = SCOPE_IDENTITY();
            
            -- Thêm chi tiết
            INSERT INTO ChiTietPhieuMuon (MaPhieuMuon, MaSach, SoLuong, TinhTrangMuon)
            VALUES (@MaPhieuMuon, @MaSach, @SoLuong, N'Tốt');
            
            -- Cập nhật sách
            UPDATE Sach 
            SET SoLuongCoSan = SoLuongCoSan - @SoLuong,
                LuotMuon = LuotMuon + 1
            WHERE MaSach = @MaSach;
            
            -- Cập nhật độc giả
            UPDATE DocGia
            SET SoSachDangMuon = SoSachDangMuon + @SoLuong,
                TongSachDaMuon = TongSachDaMuon + @SoLuong
            WHERE MaDocGia = @MaDocGia;
            
            COMMIT TRANSACTION;
            SELECT @MaPhieuMuon AS MaPhieuMuon, 1 AS Success;
        END
        ELSE
        BEGIN
            ROLLBACK TRANSACTION;
            SELECT -1 AS MaPhieuMuon, 0 AS Success;
        END
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        SELECT -1 AS MaPhieuMuon, 0 AS Success;
    END CATCH
END;
GO

-- SP Trả sách
CREATE PROCEDURE sp_TraSach
    @MaPhieuMuon INT,
    @TinhTrangSach NVARCHAR(50) = N'Tốt',
    @TienPhat DECIMAL(18,2) = 0
AS
BEGIN
    BEGIN TRANSACTION;
    BEGIN TRY
        -- Cập nhật phiếu mượn
        UPDATE PhieuMuon
        SET TrangThai = N'Đã trả',
            NgayTraThucTe = GETDATE(),
            PhiPhat = @TienPhat
        WHERE MaPhieuMuon = @MaPhieuMuon;
        
        -- Tạo phiếu trả
        DECLARE @SoNgayTre INT = 0;
        SELECT @SoNgayTre = DATEDIFF(DAY, NgayHenTra, GETDATE())
        FROM PhieuMuon WHERE MaPhieuMuon = @MaPhieuMuon AND GETDATE() > NgayHenTra;
        
        INSERT INTO PhieuTra (MaPhieuMuon, SoNgayTre, TienPhat)
        VALUES (@MaPhieuMuon, ISNULL(@SoNgayTre, 0), @TienPhat);
        
        -- Cập nhật sách
        UPDATE Sach
        SET SoLuongCoSan = SoLuongCoSan + ct.SoLuong
        FROM Sach s
        INNER JOIN ChiTietPhieuMuon ct ON s.MaSach = ct.MaSach
        WHERE ct.MaPhieuMuon = @MaPhieuMuon;
        
        -- Cập nhật tình trạng trả
        UPDATE ChiTietPhieuMuon
        SET TinhTrangTra = @TinhTrangSach
        WHERE MaPhieuMuon = @MaPhieuMuon;
        
        -- Cập nhật độc giả
        UPDATE DocGia
        SET SoSachDangMuon = SoSachDangMuon - (SELECT SUM(SoLuong) FROM ChiTietPhieuMuon WHERE MaPhieuMuon = @MaPhieuMuon)
        WHERE MaDocGia = (SELECT MaDocGia FROM PhieuMuon WHERE MaPhieuMuon = @MaPhieuMuon);
        
        COMMIT TRANSACTION;
        SELECT 1 AS Success;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        SELECT 0 AS Success;
    END CATCH
END;
GO

-- View thống kê sách
CREATE VIEW vw_ThongKeSach AS
SELECT 
    dm.TenDanhMuc,
    COUNT(s.MaSach) AS TongSoSach,
    SUM(s.SoLuong) AS TongSoLuong,
    SUM(s.SoLuongCoSan) AS TongCoSan,
    SUM(s.LuotMuon) AS TongLuotMuon,
    SUM(s.LuotXem) AS TongLuotXem
FROM Sach s
INNER JOIN DanhMuc dm ON s.MaDanhMuc = dm.MaDanhMuc
GROUP BY dm.TenDanhMuc;
GO

-- View sách phổ biến
CREATE VIEW vw_SachPhoBien AS
SELECT TOP 20
    s.MaSach, s.TenSach, s.AnhBia, s.LuotMuon, s.LuotXem,
    tg.TenTacGia, dm.TenDanhMuc, nxb.TenNXB,
    s.GiaBia, s.SoLuongCoSan, s.TinhTrang
FROM Sach s
LEFT JOIN TacGia tg ON s.MaTacGia = tg.MaTacGia
LEFT JOIN DanhMuc dm ON s.MaDanhMuc = dm.MaDanhMuc
LEFT JOIN NhaXuatBan nxb ON s.MaNXB = nxb.MaNXB
ORDER BY s.LuotMuon DESC, s.LuotXem DESC;
GO

-- View phiếu mượn quá hạn
CREATE VIEW vw_PhieuMuonQuaHan AS
SELECT 
    pm.MaPhieuMuon, pm.NgayMuon, pm.NgayHenTra,
    DATEDIFF(DAY, pm.NgayHenTra, GETDATE()) AS SoNgayTre,
    dg.HoTen AS TenDocGia, dg.SoDienThoai, dg.Email,
    pm.SoLuongSach
FROM PhieuMuon pm
INNER JOIN DocGia dg ON pm.MaDocGia = dg.MaDocGia
WHERE pm.TrangThai = N'Đang mượn' AND pm.NgayHenTra < GETDATE();
GO

-- =============================================
-- PHẦN 5: TẠO PHÂN QUYỀN SQL SERVER
-- =============================================

-- Role Admin
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = 'AdminRole')
    CREATE ROLE AdminRole;
GO
GRANT CONTROL ON DATABASE::DoAnThuVienNhom12 TO AdminRole;
GO

-- Role Thủ thư
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = 'ThuThuRole')
    CREATE ROLE ThuThuRole;
GO
GRANT SELECT, INSERT, UPDATE ON Sach TO ThuThuRole;
GRANT SELECT, INSERT, UPDATE ON DocGia TO ThuThuRole;
GRANT SELECT, INSERT, UPDATE ON PhieuMuon TO ThuThuRole;
GRANT SELECT, INSERT, UPDATE ON ChiTietPhieuMuon TO ThuThuRole;
GRANT SELECT, INSERT, UPDATE ON PhieuTra TO ThuThuRole;
GRANT SELECT ON DanhMuc TO ThuThuRole;
GRANT SELECT ON TacGia TO ThuThuRole;
GRANT SELECT ON NhaXuatBan TO ThuThuRole;
GRANT SELECT ON KeSach TO ThuThuRole;
GRANT SELECT ON Kho TO ThuThuRole;
GRANT EXECUTE ON sp_MuonSach TO ThuThuRole;
GRANT EXECUTE ON sp_TraSach TO ThuThuRole;
GRANT EXECUTE ON sp_TimKiemSach TO ThuThuRole;
GO

-- Role Độc giả
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = 'DocGiaRole')
    CREATE ROLE DocGiaRole;
GO
GRANT SELECT ON Sach TO DocGiaRole;
GRANT SELECT ON DanhMuc TO DocGiaRole;
GRANT SELECT ON TacGia TO DocGiaRole;
GRANT SELECT ON NhaXuatBan TO DocGiaRole;
GRANT SELECT ON vw_SachPhoBien TO DocGiaRole;
GRANT EXECUTE ON sp_TimKiemSach TO DocGiaRole;
GO

-- Tạo login mẫu (chạy trên master database)
USE master;
GO

-- Admin
IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = 'admin_user')
    CREATE LOGIN admin_user WITH PASSWORD = 'Admin@123';
GO

-- Thủ thư
IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = 'thuthu_user')
    CREATE LOGIN thuthu_user WITH PASSWORD = 'ThuThu@123';
GO

-- Độc giả
IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = 'docgia_user')
    CREATE LOGIN docgia_user WITH PASSWORD = 'DocGia@123';
GO

-- Gán user vào database
USE DoAnThuVienNhom12;
GO

IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = 'admin_user')
    CREATE USER admin_user FOR LOGIN admin_user;
GO
ALTER ROLE AdminRole ADD MEMBER admin_user;
GO

IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = 'thuthu_user')
    CREATE USER thuthu_user FOR LOGIN thuthu_user;
GO
ALTER ROLE ThuThuRole ADD MEMBER thuthu_user;
GO

IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = 'docgia_user')
    CREATE USER docgia_user FOR LOGIN docgia_user;
GO
ALTER ROLE DocGiaRole ADD MEMBER docgia_user;
GO
PRINT N'--- THỐNG KÊ DỮ LIỆU ---';
SELECT 'Tổng sách' AS Muc, COUNT(*) AS SoLuong FROM Sach
UNION ALL SELECT 'Danh mục', COUNT(*) FROM DanhMuc
UNION ALL SELECT 'Độc giả', COUNT(*) FROM DocGia
UNION ALL SELECT 'Nhân viên', COUNT(*) FROM NhanVien;
PRINT N'===============================================';
GO

-- =============================================
-- PHẦN 5: TẠO PHÂN QUYỀN SQL SERVER (CHỈ CÒN ADMIN + THỦ THƯ)
-- =============================================

-- Xóa role cũ nếu tồn tại
IF EXISTS (SELECT * FROM sys.database_principals WHERE name = 'DocGiaRole')
    DROP ROLE DocGiaRole;
IF EXISTS (SELECT * FROM sys.database_principals WHERE name = 'ThuThuRole')
    DROP ROLE ThuThuRole;
IF EXISTS (SELECT * FROM sys.database_principals WHERE name = 'AdminRole')
    DROP ROLE AdminRole;
GO

-- Role Admin
CREATE ROLE AdminRole;
GRANT CONTROL ON DATABASE::DoAnThuVienNhom12 TO AdminRole;
GO

-- Role Thủ thư
CREATE ROLE ThuThuRole;
GRANT SELECT, INSERT, UPDATE ON Sach TO ThuThuRole;
GRANT SELECT, INSERT, UPDATE ON DocGia TO ThuThuRole;
GRANT SELECT, INSERT, UPDATE ON PhieuMuon TO ThuThuRole;
GRANT SELECT, INSERT, UPDATE ON ChiTietPhieuMuon TO ThuThuRole;
GRANT SELECT, INSERT, UPDATE ON PhieuTra TO ThuThuRole;
GRANT SELECT ON DanhMuc TO ThuThuRole;
GRANT SELECT ON TacGia TO ThuThuRole;
GRANT SELECT ON NhaXuatBan TO ThuThuRole;
GRANT SELECT ON KeSach TO ThuThuRole;
GRANT SELECT ON Kho TO ThuThuRole;
GRANT EXECUTE ON sp_MuonSach TO ThuThuRole;
GRANT EXECUTE ON sp_TraSach TO ThuThuRole;
GRANT EXECUTE ON sp_TimKiemSach TO ThuThuRole;
GO

-- =============================================
-- PHẦN 6: TẠO LOGIN & GÁN USER
-- =============================================

USE master;
GO

-- Xóa login cũ nếu có (để tránh trùng)
IF EXISTS (SELECT * FROM sys.server_principals WHERE name = 'docgia_user')
    DROP LOGIN docgia_user;
GO

-- Tạo login mới
IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = 'admin_user')
    CREATE LOGIN admin_user WITH PASSWORD = 'Admin@123';
GO

IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = 'thuthu_user')
    CREATE LOGIN thuthu_user WITH PASSWORD = 'ThuThu@123';
GO

-- Gán vào database
USE DoAnThuVienNhom12;
GO

-- Tạo user từ login
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = 'admin_user')
    CREATE USER admin_user FOR LOGIN admin_user;
ALTER ROLE AdminRole ADD MEMBER admin_user;
GO

IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = 'thuthu_user')
    CREATE USER thuthu_user FOR LOGIN thuthu_user;
ALTER ROLE ThuThuRole ADD MEMBER thuthu_user;
GO

-- =============================================
-- PHẦN 7: THỐNG KÊ KIỂM TRA
-- =============================================
PRINT N'✅ PHÂN QUYỀN HOÀN TẤT: Chỉ còn 2 vai trò (Admin, Thủ thư).';
PRINT N'Tài khoản mẫu:';
PRINT N'   - admin_user  (Admin@123)';
PRINT N'   - thuthu_user (ThuThu@123)';
PRINT N'===============================================';
SELECT 'Tổng sách' AS Mục, COUNT(*) AS SốLượng FROM Sach
UNION ALL SELECT 'Danh mục', COUNT(*) FROM DanhMuc
UNION ALL SELECT 'Độc giả', COUNT(*) FROM DocGia
UNION ALL SELECT 'Nhân viên', COUNT(*) FROM NhanVien;
GO

SELECT * FROM DoAnThuVienNhom12.dbo.NguoiDung;



