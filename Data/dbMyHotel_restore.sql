-- ============================================================
-- dbMyHotel - Script tuong thich SQL Server 2019+
-- Generated: 2026-05-23 12:45:49
-- ============================================================

USE master;
GO
IF DB_ID('dbMyHotel') IS NOT NULL
    ALTER DATABASE dbMyHotel SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
IF DB_ID('dbMyHotel') IS NOT NULL DROP DATABASE dbMyHotel;
CREATE DATABASE dbMyHotel COLLATE Vietnamese_CI_AS;
GO
USE dbMyHotel;
GO
SET QUOTED_IDENTIFIER ON; SET ANSI_NULLS ON;
GO

CREATE TABLE roles (
    rid         INT IDENTITY(1,1) PRIMARY KEY,
    roleName    NVARCHAR(100) NOT NULL,
    description NVARCHAR(250) NULL
);
GO
CREATE TABLE rooms (
    roomid   INT IDENTITY(1,1) PRIMARY KEY,
    roomNo   VARCHAR(250) NOT NULL,
    roomType VARCHAR(250) NOT NULL,
    bed      VARCHAR(250) NOT NULL,
    price    BIGINT NOT NULL,
    booked   VARCHAR(50) NULL,
    status   NVARCHAR(50) NOT NULL
);
GO
CREATE TABLE services (
    sid         INT IDENTITY(1,1) PRIMARY KEY,
    serviceName NVARCHAR(250) NOT NULL,
    price       BIGINT NOT NULL,
    status      NVARCHAR(50) NOT NULL
);
GO
CREATE TABLE employee (
    eid      INT IDENTITY(1,1) PRIMARY KEY,
    ename    NVARCHAR(250) NOT NULL,
    mobile   NVARCHAR(20) NULL,
    gender   NVARCHAR(50) NOT NULL,
    emailid  NVARCHAR(120) NOT NULL,
    username NVARCHAR(150) NOT NULL,
    pass     NVARCHAR(150) NOT NULL,
    rid      INT NULL REFERENCES roles(rid)
);
GO
CREATE TABLE guests (
    gid         INT IDENTITY(1,1) PRIMARY KEY,
    cname       NVARCHAR(250) NOT NULL,
    mobile      NVARCHAR(20) NULL,
    nationality NVARCHAR(250) NOT NULL,
    gender      NVARCHAR(50) NOT NULL,
    dob         NVARCHAR(50) NOT NULL,
    idproof     NVARCHAR(250) NOT NULL,
    address     NVARCHAR(350) NOT NULL
);
GO
CREATE TABLE bookings (
    bid      INT IDENTITY(1,1) PRIMARY KEY,
    gid      INT NOT NULL REFERENCES guests(gid),
    roomid   INT NOT NULL REFERENCES rooms(roomid),
    checkin  NVARCHAR(250) NULL,
    checkout NVARCHAR(250) NULL,
    chekout  NVARCHAR(250) NOT NULL
);
GO
CREATE TABLE invoices (
    invoiceId   INT IDENTITY(1,1) PRIMARY KEY,
    invoiceNo   NVARCHAR(20) NOT NULL,
    bid         INT NOT NULL REFERENCES bookings(bid),
    createdDate DATETIME NOT NULL,
    totalAmount BIGINT NOT NULL,
    status      NVARCHAR(50) NOT NULL,
    eid         INT NULL REFERENCES employee(eid)
);
GO
CREATE TABLE customer_services (
    id        INT IDENTITY(1,1) PRIMARY KEY,
    bid       INT NOT NULL REFERENCES bookings(bid),
    sid       INT NOT NULL REFERENCES services(sid),
    quantity  INT NOT NULL,
    used_date NVARCHAR(50) NOT NULL
);
GO
-- ----- roles -----
SET IDENTITY_INSERT roles ON;
INSERT INTO roles (rid, roleName, description) VALUES (1, N'Admin', N'Quản trị hệ thống');
INSERT INTO roles (rid, roleName, description) VALUES (2, N'Lễ Tân', N'Nhân viên lễ tân');
INSERT INTO roles (rid, roleName, description) VALUES (3, N'Kế Toán', N'Nhân viên kế toán');
INSERT INTO roles (rid, roleName, description) VALUES (4, N'Nhân viên', N'NhÃ¢n viÃªn thÃ´ng thÆ°á»ng');
SET IDENTITY_INSERT roles OFF;
GO

-- ----- rooms -----
SET IDENTITY_INSERT rooms ON;
INSERT INTO rooms (roomid, roomNo, roomType, bed, price, booked, status) VALUES (1, '101', 'Ac', 'Single', 50000, 'NO', N'Có khách');
INSERT INTO rooms (roomid, roomNo, roomType, bed, price, booked, status) VALUES (2, '102', 'Ac', 'Double', 20000, 'NO', N'Bẩn');
INSERT INTO rooms (roomid, roomNo, roomType, bed, price, booked, status) VALUES (3, '201', 'Ac', 'Double', 10000000, 'NO', N'Bảo trì');
INSERT INTO rooms (roomid, roomNo, roomType, bed, price, booked, status) VALUES (4, '202', 'Ac', 'Double', 550000, 'NO', N'Có khách');
INSERT INTO rooms (roomid, roomNo, roomType, bed, price, booked, status) VALUES (5, '203', 'Ac', 'Double', 550000, 'NO', N'Có khách');
INSERT INTO rooms (roomid, roomNo, roomType, bed, price, booked, status) VALUES (6, '204', 'Non-Ac', 'Single', 350000, 'NO', N'Có khách');
INSERT INTO rooms (roomid, roomNo, roomType, bed, price, booked, status) VALUES (7, '205', 'Ac', 'Triple', 750000, 'NO', N'Bẩn');
INSERT INTO rooms (roomid, roomNo, roomType, bed, price, booked, status) VALUES (8, '301', 'Ac', 'Single', 400000, 'NO', N'Có khách');
INSERT INTO rooms (roomid, roomNo, roomType, bed, price, booked, status) VALUES (9, '302', 'Ac', 'Double', 600000, 'NO', N'Có khách');
INSERT INTO rooms (roomid, roomNo, roomType, bed, price, booked, status) VALUES (10, '303', 'Non-Ac', 'Double', 500000, 'NO', N'Bảo trì');
INSERT INTO rooms (roomid, roomNo, roomType, bed, price, booked, status) VALUES (11, '304', 'Ac', 'Double', 600000, 'NO', N'Có khách');
INSERT INTO rooms (roomid, roomNo, roomType, bed, price, booked, status) VALUES (12, '305', 'Non-Ac', 'Single', 400000, 'NO', N'Có khách');
INSERT INTO rooms (roomid, roomNo, roomType, bed, price, booked, status) VALUES (13, '401', 'Ac', 'Double', 650000, 'NO', N'Có khách');
INSERT INTO rooms (roomid, roomNo, roomType, bed, price, booked, status) VALUES (14, '402', 'Ac', 'Triple', 850000, 'NO', N'Bẩn');
INSERT INTO rooms (roomid, roomNo, roomType, bed, price, booked, status) VALUES (15, '403', 'Ac', 'Double', 650000, 'NO', N'Bẩn');
INSERT INTO rooms (roomid, roomNo, roomType, bed, price, booked, status) VALUES (16, '404', 'Non-Ac', 'Single', 450000, 'NO', N'Bẩn');
INSERT INTO rooms (roomid, roomNo, roomType, bed, price, booked, status) VALUES (17, '405', 'Ac', 'Double', 650000, 'NO', N'Bảo trì');
INSERT INTO rooms (roomid, roomNo, roomType, bed, price, booked, status) VALUES (18, '501', 'Ac', 'Triple', 900000, 'NO', N'Có khách');
INSERT INTO rooms (roomid, roomNo, roomType, bed, price, booked, status) VALUES (19, '502', 'Ac', 'Double', 700000, 'NO', N'Bẩn');
INSERT INTO rooms (roomid, roomNo, roomType, bed, price, booked, status) VALUES (20, '503', 'Ac', 'Triple', 900000, 'NO', N'Bẩn');
INSERT INTO rooms (roomid, roomNo, roomType, bed, price, booked, status) VALUES (21, '504', 'Non-Ac', 'Single', 500000, 'NO', N'Bẩn');
INSERT INTO rooms (roomid, roomNo, roomType, bed, price, booked, status) VALUES (22, '505', 'Ac', 'Double', 700000, 'NO', N'Bẩn');
SET IDENTITY_INSERT rooms OFF;
GO

-- ----- services -----
SET IDENTITY_INSERT services ON;
INSERT INTO services (sid, serviceName, price, status) VALUES (1, N'Nước suối', 20000, N'Đang cung cấp');
INSERT INTO services (sid, serviceName, price, status) VALUES (2, N'Bữa sáng', 80000, N'Đang cung cấp');
INSERT INTO services (sid, serviceName, price, status) VALUES (3, N'Giặt ủi', 50000, N'Đang cung cấp');
INSERT INTO services (sid, serviceName, price, status) VALUES (4, N'Đưa đón sân bay', 200000, N'Đang cung cấp');
INSERT INTO services (sid, serviceName, price, status) VALUES (5, N'Thuê xe máy', 150000, N'Đang cung cấp');
INSERT INTO services (sid, serviceName, price, status) VALUES (6, N'Spa & Massage', 300000, N'Đang cung cấp');
INSERT INTO services (sid, serviceName, price, status) VALUES (7, N'Ăn tối (Set menu)', 120000, N'Ngừng cung cấp');
INSERT INTO services (sid, serviceName, price, status) VALUES (8, N'Dịch vụ phòng 24h', 100000, N'Đang cung cấp');
SET IDENTITY_INSERT services OFF;
GO

-- ----- employee -----
SET IDENTITY_INSERT employee ON;
INSERT INTO employee (eid, ename, mobile, gender, emailid, username, pass, rid) VALUES (1, N'Lê Linh', N'968447441', N'Nu', N'lelinh@gmail.com', N'LeLinh', N'123', 2);
INSERT INTO employee (eid, ename, mobile, gender, emailid, username, pass, rid) VALUES (2, N'Administrator', N'978785678', N'Nam', N'admin@hotel.com', N'admin', N'123', 1);
INSERT INTO employee (eid, ename, mobile, gender, emailid, username, pass, rid) VALUES (3, N'Nguyễn Văn An', N'912345678', N'Nam', N'nva.hotel@gmail.com', N'letannv01', N'123456', 2);
INSERT INTO employee (eid, ename, mobile, gender, emailid, username, pass, rid) VALUES (4, N'Trần Thị Bình', N'987654321', N'Nu', N'ttb.hotel@gmail.com', N'letannv02', N'123456', 2);
INSERT INTO employee (eid, ename, mobile, gender, emailid, username, pass, rid) VALUES (5, N'Lê Văn Cường', N'934567890', N'Nam', N'lvc.hotel@gmail.com', N'letannv03', N'123456', 2);
INSERT INTO employee (eid, ename, mobile, gender, emailid, username, pass, rid) VALUES (6, N'Phạm Thị Dung', N'965432109', N'Nu', N'ptd.hotel@gmail.com', N'letannv04', N'123456', 2);
INSERT INTO employee (eid, ename, mobile, gender, emailid, username, pass, rid) VALUES (7, N'Hoàng Văn Đức', N'978901234', N'Nam', N'hvd.hotel@gmail.com', N'letannv05', N'123456', 2);
INSERT INTO employee (eid, ename, mobile, gender, emailid, username, pass, rid) VALUES (8, N'Võ Thị Phương', N'943210987', N'Nu', N'vtp.hotel@gmail.com', N'nhanvien01', N'123456', 4);
INSERT INTO employee (eid, ename, mobile, gender, emailid, username, pass, rid) VALUES (9, N'Đặng Văn Giang', N'956789012', N'Nam', N'dvg.hotel@gmail.com', N'nhanvien02', N'123456', 4);
INSERT INTO employee (eid, ename, mobile, gender, emailid, username, pass, rid) VALUES (10, N'Bùi Thị Hoa', N'921098765', N'Nu', N'bth.hotel@gmail.com', N'nhanvien03', N'123456', 4);
INSERT INTO employee (eid, ename, mobile, gender, emailid, username, pass, rid) VALUES (11, N'Đỗ Văn Khải', N'969876543', N'Nam', N'dvk.hotel@gmail.com', N'nhanvien04', N'123456', 4);
INSERT INTO employee (eid, ename, mobile, gender, emailid, username, pass, rid) VALUES (12, N'Ngô Thị Lan', N'932109876', N'Nu', N'ntl.hotel@gmail.com', N'nhanvien05', N'123456', 4);
SET IDENTITY_INSERT employee OFF;
GO

-- ----- guests -----
SET IDENTITY_INSERT guests ON;
INSERT INTO guests (gid, cname, mobile, nationality, gender, dob, idproof, address) VALUES (1, N'Nguyễn Hoàng Anh', N'0978785678', N'Việt Nam', N'Nam', N'2025-12-15', N'02424541512', N'Thanh Xuân - Hà Nội');
INSERT INTO guests (gid, cname, mobile, nationality, gender, dob, idproof, address) VALUES (2, N'Bùi Tiến Dũng', N'0978784578', N'Việt Nam', N'Nam', N'2025-12-15', N'1875151122122', N'Hà Nam');
INSERT INTO guests (gid, cname, mobile, nationality, gender, dob, idproof, address) VALUES (3, N'Mai Nguyễn', N'98745541522', N'Việt Nam', N'Nu', N'2006-05-21', N'1545465465456', N'Ninh Bình');
INSERT INTO guests (gid, cname, mobile, nationality, gender, dob, idproof, address) VALUES (4, N'Nguyễn Minh Tuấn', N'0901234560', N'Việt Nam', N'Nam', N'01/01/1990', N'CCCD', N'22 Trần Phú, Hà Nội');
INSERT INTO guests (gid, cname, mobile, nationality, gender, dob, idproof, address) VALUES (5, N'Trần Thị Mai', N'0912345670', N'Việt Nam', N'Nữ', N'01/01/1990', N'CCCD', N'45 Lê Lợi, TP.HCM');
INSERT INTO guests (gid, cname, mobile, nationality, gender, dob, idproof, address) VALUES (6, N'Lê Quang Hùng', N'0923456780', N'Việt Nam', N'Nam', N'01/01/1990', N'CCCD', N'8 Bạch Đằng, Đà Nẵng');
INSERT INTO guests (gid, cname, mobile, nationality, gender, dob, idproof, address) VALUES (7, N'Phạm Thị Thu', N'0934567891', N'Việt Nam', N'Nữ', N'01/01/1990', N'CCCD', N'12 Điện Biên Phủ, Hải Phòng');
INSERT INTO guests (gid, cname, mobile, nationality, gender, dob, idproof, address) VALUES (8, N'Hoàng Văn Bình', N'0945678902', N'Việt Nam', N'Nam', N'01/01/1990', N'CCCD', N'5 Hòa Bình, Cần Thơ');
INSERT INTO guests (gid, cname, mobile, nationality, gender, dob, idproof, address) VALUES (9, N'Võ Thị Lan', N'0956789013', N'Việt Nam', N'Nữ', N'01/01/1990', N'CCCD', N'33 Lý Tự Trọng, Nha Trang');
INSERT INTO guests (gid, cname, mobile, nationality, gender, dob, idproof, address) VALUES (10, N'Đặng Quốc Việt', N'0967890124', N'Việt Nam', N'Nam', N'01/01/1990', N'CCCD', N'18 Kim Mã, Hà Nội');
INSERT INTO guests (gid, cname, mobile, nationality, gender, dob, idproof, address) VALUES (11, N'Bùi Thị Nga', N'0978901235', N'Việt Nam', N'Nữ', N'01/01/1990', N'CCCD', N'90 Nguyễn Huệ, TP.HCM');
INSERT INTO guests (gid, cname, mobile, nationality, gender, dob, idproof, address) VALUES (12, N'Đỗ Văn Khoa', N'0989012346', N'Việt Nam', N'Nam', N'01/01/1990', N'CCCD', N'6 Hoàng Văn Thụ, Đà Lạt');
INSERT INTO guests (gid, cname, mobile, nationality, gender, dob, idproof, address) VALUES (13, N'Ngô Thị Hương', N'0990123457', N'Việt Nam', N'Nữ', N'01/01/1990', N'CCCD', N'14 Lê Duẩn, Huế');
INSERT INTO guests (gid, cname, mobile, nationality, gender, dob, idproof, address) VALUES (14, N'Dương Văn Tùng', N'0911234568', N'Việt Nam', N'Nam', N'01/01/1990', N'CCCD', N'27 Đội Cấn, Hà Nội');
INSERT INTO guests (gid, cname, mobile, nationality, gender, dob, idproof, address) VALUES (15, N'Lý Thị Phúc', N'0922345679', N'Việt Nam', N'Nữ', N'01/01/1990', N'CCCD', N'3 Phan Châu Trinh, Quảng Nam');
INSERT INTO guests (gid, cname, mobile, nationality, gender, dob, idproof, address) VALUES (16, N'Trịnh Quang Minh', N'0933456780', N'Việt Nam', N'Nam', N'01/01/1990', N'CCCD', N'55 Bà Triệu, Hà Nội');
INSERT INTO guests (gid, cname, mobile, nationality, gender, dob, idproof, address) VALUES (17, N'Vũ Thị Hà', N'0944567891', N'Việt Nam', N'Nữ', N'01/01/1990', N'CCCD', N'77 Cách Mạng Tháng 8, TP.HCM');
INSERT INTO guests (gid, cname, mobile, nationality, gender, dob, idproof, address) VALUES (18, N'Cao Văn Long', N'0955678902', N'Việt Nam', N'Nam', N'01/01/1990', N'CCCD', N'9 Quốc Lộ 13, Bình Dương');
INSERT INTO guests (gid, cname, mobile, nationality, gender, dob, idproof, address) VALUES (19, N'Đinh Thị Nhung', N'0966789013', N'Việt Nam', N'Nữ', N'01/01/1990', N'CCCD', N'41 Đồng Khởi, Đồng Nai');
INSERT INTO guests (gid, cname, mobile, nationality, gender, dob, idproof, address) VALUES (20, N'Phan Văn Đạt', N'0977890124', N'Việt Nam', N'Nam', N'01/01/1990', N'CCCD', N'16 Giảng Võ, Hà Nội');
INSERT INTO guests (gid, cname, mobile, nationality, gender, dob, idproof, address) VALUES (21, N'Mai Thị Liên', N'0988901235', N'Việt Nam', N'Nữ', N'01/01/1990', N'CCCD', N'30 Quang Trung, Nghệ An');
INSERT INTO guests (gid, cname, mobile, nationality, gender, dob, idproof, address) VALUES (22, N'Tăng Văn Phát', N'0999012346', N'Việt Nam', N'Nam', N'01/01/1990', N'CCCD', N'11 Núi Thành, Đà Nẵng');
INSERT INTO guests (gid, cname, mobile, nationality, gender, dob, idproof, address) VALUES (23, N'Lưu Thị Diễm', N'0911123457', N'Việt Nam', N'Nữ', N'01/01/1990', N'CCCD', N'7 Trần Hưng Đạo, Cần Thơ');
INSERT INTO guests (gid, cname, mobile, nationality, gender, dob, idproof, address) VALUES (24, N'Phạm Thị Hoa', N'0901234560', N'Việt Nam', N'Nữ', N'01/01/1992', N'082192001234', N'12 Lê Lợi, TP.HCM');
INSERT INTO guests (gid, cname, mobile, nationality, gender, dob, idproof, address) VALUES (25, N'Trần Văn Minh', N'0912345671', N'Việt Nam', N'Nam', N'01/01/1988', N'035088001235', N'45 Nguyễn Huệ, Đà Nẵng');
INSERT INTO guests (gid, cname, mobile, nationality, gender, dob, idproof, address) VALUES (26, N'Lê Thị Thu', N'0923456782', N'Việt Nam', N'Nữ', N'01/01/1995', N'001295001236', N'78 Trần Phú, Hà Nội');
INSERT INTO guests (gid, cname, mobile, nationality, gender, dob, idproof, address) VALUES (27, N'Nguyễn Văn Hùng', N'0934567893', N'Việt Nam', N'Nam', N'01/01/1985', N'036085001237', N'33 Phan Bội Châu, Huế');
INSERT INTO guests (gid, cname, mobile, nationality, gender, dob, idproof, address) VALUES (28, N'Đỗ Thị Lan', N'0945678904', N'Việt Nam', N'Nữ', N'01/01/1997', N'083197001238', N'5 Đinh Tiên Hoàng, Cần Thơ');
INSERT INTO guests (gid, cname, mobile, nationality, gender, dob, idproof, address) VALUES (29, N'Hoàng Văn Nam', N'0956789015', N'Việt Nam', N'Nam', N'01/01/1990', N'038090001239', N'22 Hai Bà Trưng, Hải Phòng');
INSERT INTO guests (gid, cname, mobile, nationality, gender, dob, idproof, address) VALUES (30, N'Vũ Thị Mai', N'0967890126', N'Việt Nam', N'Nữ', N'01/01/1993', N'082193001240', N'9 Lý Thường Kiệt, Nha Trang');
INSERT INTO guests (gid, cname, mobile, nationality, gender, dob, idproof, address) VALUES (31, N'Bùi Văn Đức', N'0978901237', N'Việt Nam', N'Nam', N'01/01/1987', N'037087001241', N'67 Nguyễn Trãi, Vũng Tàu');
SET IDENTITY_INSERT guests OFF;
GO

-- ----- bookings -----
SET IDENTITY_INSERT bookings ON;
INSERT INTO bookings (bid, gid, roomid, checkin, checkout, chekout) VALUES (1, 1, 1, N'2025-12-15', N'5/19/2026', N'YES');
INSERT INTO bookings (bid, gid, roomid, checkin, checkout, chekout) VALUES (2, 2, 1, N'2025-12-15', N'12/17/2025', N'YES');
INSERT INTO bookings (bid, gid, roomid, checkin, checkout, chekout) VALUES (3, 3, 1, N'2026-05-21', N'05/21/2026', N'YES');
INSERT INTO bookings (bid, gid, roomid, checkin, checkout, chekout) VALUES (4, 3, 1, N'2026-05-21', N'05/22/2026', N'YES');
INSERT INTO bookings (bid, gid, roomid, checkin, checkout, chekout) VALUES (5, 1, 2, N'2026-05-22', N'05/22/2026', N'YES');
INSERT INTO bookings (bid, gid, roomid, checkin, checkout, chekout) VALUES (6, 4, 5, N'2026-01-05', N'2026-01-08', N'YES');
INSERT INTO bookings (bid, gid, roomid, checkin, checkout, chekout) VALUES (7, 5, 12, N'2026-01-08', N'2026-01-11', N'YES');
INSERT INTO bookings (bid, gid, roomid, checkin, checkout, chekout) VALUES (8, 6, 16, N'2026-01-12', N'2026-01-15', N'YES');
INSERT INTO bookings (bid, gid, roomid, checkin, checkout, chekout) VALUES (9, 7, 21, N'2026-01-16', N'2026-01-20', N'YES');
INSERT INTO bookings (bid, gid, roomid, checkin, checkout, chekout) VALUES (10, 8, 22, N'2026-01-21', N'2026-01-24', N'YES');
INSERT INTO bookings (bid, gid, roomid, checkin, checkout, chekout) VALUES (11, 9, 5, N'2026-02-03', N'2026-02-07', N'YES');
INSERT INTO bookings (bid, gid, roomid, checkin, checkout, chekout) VALUES (12, 10, 12, N'2026-02-05', N'2026-02-09', N'YES');
INSERT INTO bookings (bid, gid, roomid, checkin, checkout, chekout) VALUES (13, 11, 16, N'2026-02-10', N'2026-02-13', N'YES');
INSERT INTO bookings (bid, gid, roomid, checkin, checkout, chekout) VALUES (14, 12, 21, N'2026-02-14', N'2026-02-18', N'YES');
INSERT INTO bookings (bid, gid, roomid, checkin, checkout, chekout) VALUES (15, 13, 22, N'2026-02-20', N'2026-02-23', N'YES');
INSERT INTO bookings (bid, gid, roomid, checkin, checkout, chekout) VALUES (16, 4, 11, N'2026-02-15', N'2026-02-19', N'YES');
INSERT INTO bookings (bid, gid, roomid, checkin, checkout, chekout) VALUES (17, 5, 5, N'2026-02-25', N'2026-02-28', N'YES');
INSERT INTO bookings (bid, gid, roomid, checkin, checkout, chekout) VALUES (18, 15, 5, N'2026-03-03', N'2026-03-07', N'YES');
INSERT INTO bookings (bid, gid, roomid, checkin, checkout, chekout) VALUES (19, 16, 12, N'2026-03-05', N'2026-03-09', N'YES');
INSERT INTO bookings (bid, gid, roomid, checkin, checkout, chekout) VALUES (20, 17, 16, N'2026-03-10', N'2026-03-14', N'YES');
INSERT INTO bookings (bid, gid, roomid, checkin, checkout, chekout) VALUES (21, 18, 21, N'2026-03-15', N'2026-03-19', N'YES');
INSERT INTO bookings (bid, gid, roomid, checkin, checkout, chekout) VALUES (22, 19, 22, N'2026-03-20', N'2026-03-24', N'YES');
INSERT INTO bookings (bid, gid, roomid, checkin, checkout, chekout) VALUES (23, 20, 11, N'2026-03-22', N'2026-03-26', N'YES');
INSERT INTO bookings (bid, gid, roomid, checkin, checkout, chekout) VALUES (24, 21, 5, N'2026-03-26', N'2026-03-30', N'YES');
INSERT INTO bookings (bid, gid, roomid, checkin, checkout, chekout) VALUES (25, 22, 12, N'2026-03-28', N'2026-04-01', N'YES');
INSERT INTO bookings (bid, gid, roomid, checkin, checkout, chekout) VALUES (26, 23, 16, N'2026-04-02', N'2026-04-06', N'YES');
INSERT INTO bookings (bid, gid, roomid, checkin, checkout, chekout) VALUES (27, 6, 5, N'2026-04-05', N'2026-04-09', N'YES');
INSERT INTO bookings (bid, gid, roomid, checkin, checkout, chekout) VALUES (28, 7, 21, N'2026-04-10', N'2026-04-14', N'YES');
INSERT INTO bookings (bid, gid, roomid, checkin, checkout, chekout) VALUES (29, 8, 22, N'2026-04-15', N'2026-04-19', N'YES');
INSERT INTO bookings (bid, gid, roomid, checkin, checkout, chekout) VALUES (30, 9, 11, N'2026-04-20', N'2026-04-24', N'YES');
INSERT INTO bookings (bid, gid, roomid, checkin, checkout, chekout) VALUES (31, 10, 5, N'2026-04-25', N'2026-04-29', N'YES');
INSERT INTO bookings (bid, gid, roomid, checkin, checkout, chekout) VALUES (32, 11, 16, N'2026-04-28', N'2026-05-02', N'YES');
INSERT INTO bookings (bid, gid, roomid, checkin, checkout, chekout) VALUES (33, 12, 21, N'2026-05-02', N'2026-05-06', N'YES');
INSERT INTO bookings (bid, gid, roomid, checkin, checkout, chekout) VALUES (34, 13, 22, N'2026-05-05', N'2026-05-09', N'YES');
INSERT INTO bookings (bid, gid, roomid, checkin, checkout, chekout) VALUES (35, 14, 5, N'2026-05-07', N'2026-05-11', N'YES');
INSERT INTO bookings (bid, gid, roomid, checkin, checkout, chekout) VALUES (36, 15, 12, N'2026-05-09', N'2026-05-13', N'YES');
INSERT INTO bookings (bid, gid, roomid, checkin, checkout, chekout) VALUES (37, 14, 16, N'2026-05-11', N'2026-05-14', N'YES');
INSERT INTO bookings (bid, gid, roomid, checkin, checkout, chekout) VALUES (38, 16, 5, N'2026-05-13', N'2026-05-16', N'YES');
INSERT INTO bookings (bid, gid, roomid, checkin, checkout, chekout) VALUES (39, 17, 16, N'2026-05-14', N'2026-05-16', N'YES');
INSERT INTO bookings (bid, gid, roomid, checkin, checkout, chekout) VALUES (40, 18, 21, N'2026-05-13', N'2026-05-16', N'YES');
INSERT INTO bookings (bid, gid, roomid, checkin, checkout, chekout) VALUES (41, 19, 5, N'2026-05-16', N'2026-05-17', N'YES');
INSERT INTO bookings (bid, gid, roomid, checkin, checkout, chekout) VALUES (42, 20, 12, N'2026-05-14', N'2026-05-17', N'YES');
INSERT INTO bookings (bid, gid, roomid, checkin, checkout, chekout) VALUES (43, 21, 22, N'2026-05-14', N'2026-05-17', N'YES');
INSERT INTO bookings (bid, gid, roomid, checkin, checkout, chekout) VALUES (44, 22, 5, N'2026-05-17', N'2026-05-18', N'YES');
INSERT INTO bookings (bid, gid, roomid, checkin, checkout, chekout) VALUES (45, 23, 11, N'2026-05-16', N'2026-05-18', N'YES');
INSERT INTO bookings (bid, gid, roomid, checkin, checkout, chekout) VALUES (46, 4, 16, N'2026-05-17', N'2026-05-19', N'YES');
INSERT INTO bookings (bid, gid, roomid, checkin, checkout, chekout) VALUES (47, 5, 21, N'2026-05-17', N'2026-05-19', N'YES');
INSERT INTO bookings (bid, gid, roomid, checkin, checkout, chekout) VALUES (48, 6, 5, N'2026-05-18', N'2026-05-19', N'YES');
INSERT INTO bookings (bid, gid, roomid, checkin, checkout, chekout) VALUES (49, 7, 22, N'2026-05-18', N'2026-05-20', N'YES');
INSERT INTO bookings (bid, gid, roomid, checkin, checkout, chekout) VALUES (50, 8, 16, N'2026-05-19', N'2026-05-20', N'YES');
INSERT INTO bookings (bid, gid, roomid, checkin, checkout, chekout) VALUES (51, 9, 5, N'2026-05-19', N'2026-05-21', N'YES');
INSERT INTO bookings (bid, gid, roomid, checkin, checkout, chekout) VALUES (52, 10, 11, N'2026-05-19', N'2026-05-21', N'YES');
INSERT INTO bookings (bid, gid, roomid, checkin, checkout, chekout) VALUES (53, 11, 12, N'2026-05-18', N'2026-05-21', N'YES');
INSERT INTO bookings (bid, gid, roomid, checkin, checkout, chekout) VALUES (54, 12, 21, N'2026-05-19', N'2026-05-21', N'YES');
INSERT INTO bookings (bid, gid, roomid, checkin, checkout, chekout) VALUES (55, 13, 7, N'2026-05-19', N'2026-05-22', N'YES');
INSERT INTO bookings (bid, gid, roomid, checkin, checkout, chekout) VALUES (56, 14, 14, N'2026-05-20', N'2026-05-22', N'YES');
INSERT INTO bookings (bid, gid, roomid, checkin, checkout, chekout) VALUES (57, 15, 20, N'2026-05-18', N'2026-05-22', N'YES');
INSERT INTO bookings (bid, gid, roomid, checkin, checkout, chekout) VALUES (58, 16, 4, N'2026-05-22', N'2026-05-25', N'NO');
INSERT INTO bookings (bid, gid, roomid, checkin, checkout, chekout) VALUES (59, 17, 8, N'2026-05-22', N'2026-05-24', N'NO');
INSERT INTO bookings (bid, gid, roomid, checkin, checkout, chekout) VALUES (60, 18, 13, N'2026-05-22', N'2026-05-26', N'NO');
INSERT INTO bookings (bid, gid, roomid, checkin, checkout, chekout) VALUES (61, 19, 18, N'2026-05-22', N'2026-05-27', N'NO');
INSERT INTO bookings (bid, gid, roomid, checkin, checkout, chekout) VALUES (62, 20, 6, N'2026-05-20', N'2026-05-23', N'NO');
INSERT INTO bookings (bid, gid, roomid, checkin, checkout, chekout) VALUES (63, 21, 9, N'2026-05-21', N'2026-05-24', N'NO');
INSERT INTO bookings (bid, gid, roomid, checkin, checkout, chekout) VALUES (64, 22, 15, N'2026-05-21', N'2026-05-25', N'NO');
INSERT INTO bookings (bid, gid, roomid, checkin, checkout, chekout) VALUES (65, 23, 19, N'2026-05-17', N'2026-05-23', N'NO');
INSERT INTO bookings (bid, gid, roomid, checkin, checkout, chekout) VALUES (66, 24, 5, N'2026-05-23', N'2026-05-25', N'NO');
INSERT INTO bookings (bid, gid, roomid, checkin, checkout, chekout) VALUES (67, 25, 11, N'2026-05-23', N'2026-05-26', N'NO');
INSERT INTO bookings (bid, gid, roomid, checkin, checkout, chekout) VALUES (68, 26, 12, N'2026-05-23', N'2026-05-25', N'NO');
INSERT INTO bookings (bid, gid, roomid, checkin, checkout, chekout) VALUES (69, 27, 16, N'2026-05-23', N'05/23/2026', N'YES');
INSERT INTO bookings (bid, gid, roomid, checkin, checkout, chekout) VALUES (70, 28, 21, N'2026-05-23', N'05/23/2026', N'YES');
INSERT INTO bookings (bid, gid, roomid, checkin, checkout, chekout) VALUES (71, 29, 22, N'2026-05-22', N'2026-05-23', N'YES');
INSERT INTO bookings (bid, gid, roomid, checkin, checkout, chekout) VALUES (72, 30, 15, N'2026-05-21', N'2026-05-23', N'YES');
INSERT INTO bookings (bid, gid, roomid, checkin, checkout, chekout) VALUES (73, 31, 19, N'2026-05-20', N'2026-05-23', N'YES');
SET IDENTITY_INSERT bookings OFF;
GO

-- ----- invoices -----
SET IDENTITY_INSERT invoices ON;
INSERT INTO invoices (invoiceId, invoiceNo, bid, createdDate, totalAmount, status, eid) VALUES (1, N'HD000001', 4, '2026-05-22 18:36:33', 231000, N'Đã thanh toán', 1);
INSERT INTO invoices (invoiceId, invoiceNo, bid, createdDate, totalAmount, status, eid) VALUES (2, N'HD000002', 5, '2026-05-22 18:44:59', 1122000, N'Đã thanh toán', 2);
INSERT INTO invoices (invoiceId, invoiceNo, bid, createdDate, totalAmount, status, eid) VALUES (3, N'HD20260114', 6, '2026-01-08 00:00:00', 1650000, N'Đã thanh toán', NULL);
INSERT INTO invoices (invoiceId, invoiceNo, bid, createdDate, totalAmount, status, eid) VALUES (4, N'HD20260124', 7, '2026-01-11 00:00:00', 1200000, N'Đã thanh toán', NULL);
INSERT INTO invoices (invoiceId, invoiceNo, bid, createdDate, totalAmount, status, eid) VALUES (5, N'HD20260134', 8, '2026-01-15 00:00:00', 1350000, N'Đã thanh toán', NULL);
INSERT INTO invoices (invoiceId, invoiceNo, bid, createdDate, totalAmount, status, eid) VALUES (6, N'HD20260144', 9, '2026-01-20 00:00:00', 2000000, N'Đã thanh toán', NULL);
INSERT INTO invoices (invoiceId, invoiceNo, bid, createdDate, totalAmount, status, eid) VALUES (7, N'HD20260154', 10, '2026-01-24 00:00:00', 2100000, N'Đã thanh toán', NULL);
INSERT INTO invoices (invoiceId, invoiceNo, bid, createdDate, totalAmount, status, eid) VALUES (8, N'HD20260264', 11, '2026-02-07 00:00:00', 2200000, N'Đã thanh toán', NULL);
INSERT INTO invoices (invoiceId, invoiceNo, bid, createdDate, totalAmount, status, eid) VALUES (9, N'HD20260274', 12, '2026-02-09 00:00:00', 1600000, N'Đã thanh toán', NULL);
INSERT INTO invoices (invoiceId, invoiceNo, bid, createdDate, totalAmount, status, eid) VALUES (10, N'HD20260284', 13, '2026-02-13 00:00:00', 1350000, N'Đã thanh toán', NULL);
INSERT INTO invoices (invoiceId, invoiceNo, bid, createdDate, totalAmount, status, eid) VALUES (11, N'HD20260294', 14, '2026-02-18 00:00:00', 2000000, N'Đã thanh toán', NULL);
INSERT INTO invoices (invoiceId, invoiceNo, bid, createdDate, totalAmount, status, eid) VALUES (12, N'HD202602104', 15, '2026-02-23 00:00:00', 2100000, N'Đã thanh toán', NULL);
INSERT INTO invoices (invoiceId, invoiceNo, bid, createdDate, totalAmount, status, eid) VALUES (13, N'HD202602114', 16, '2026-02-19 00:00:00', 2400000, N'Đã thanh toán', NULL);
INSERT INTO invoices (invoiceId, invoiceNo, bid, createdDate, totalAmount, status, eid) VALUES (14, N'HD202602124', 17, '2026-02-28 00:00:00', 1650000, N'Đã thanh toán', NULL);
INSERT INTO invoices (invoiceId, invoiceNo, bid, createdDate, totalAmount, status, eid) VALUES (15, N'HD202603134', 18, '2026-03-07 00:00:00', 2200000, N'Đã thanh toán', NULL);
INSERT INTO invoices (invoiceId, invoiceNo, bid, createdDate, totalAmount, status, eid) VALUES (16, N'HD202603144', 19, '2026-03-09 00:00:00', 1600000, N'Đã thanh toán', NULL);
INSERT INTO invoices (invoiceId, invoiceNo, bid, createdDate, totalAmount, status, eid) VALUES (17, N'HD202603154', 20, '2026-03-14 00:00:00', 1800000, N'Đã thanh toán', NULL);
INSERT INTO invoices (invoiceId, invoiceNo, bid, createdDate, totalAmount, status, eid) VALUES (18, N'HD202603164', 21, '2026-03-19 00:00:00', 2000000, N'Đã thanh toán', NULL);
INSERT INTO invoices (invoiceId, invoiceNo, bid, createdDate, totalAmount, status, eid) VALUES (19, N'HD202603174', 22, '2026-03-24 00:00:00', 2800000, N'Đã thanh toán', NULL);
INSERT INTO invoices (invoiceId, invoiceNo, bid, createdDate, totalAmount, status, eid) VALUES (20, N'HD202603184', 23, '2026-03-26 00:00:00', 2400000, N'Đã thanh toán', NULL);
INSERT INTO invoices (invoiceId, invoiceNo, bid, createdDate, totalAmount, status, eid) VALUES (21, N'HD202603194', 24, '2026-03-30 00:00:00', 2200000, N'Đã thanh toán', NULL);
INSERT INTO invoices (invoiceId, invoiceNo, bid, createdDate, totalAmount, status, eid) VALUES (22, N'HD202604204', 25, '2026-04-01 00:00:00', 1600000, N'Đã thanh toán', NULL);
INSERT INTO invoices (invoiceId, invoiceNo, bid, createdDate, totalAmount, status, eid) VALUES (23, N'HD202604214', 26, '2026-04-06 00:00:00', 1800000, N'Đã thanh toán', NULL);
INSERT INTO invoices (invoiceId, invoiceNo, bid, createdDate, totalAmount, status, eid) VALUES (24, N'HD202604224', 27, '2026-04-09 00:00:00', 2200000, N'Đã thanh toán', NULL);
INSERT INTO invoices (invoiceId, invoiceNo, bid, createdDate, totalAmount, status, eid) VALUES (25, N'HD202604234', 28, '2026-04-14 00:00:00', 2000000, N'Đã thanh toán', NULL);
INSERT INTO invoices (invoiceId, invoiceNo, bid, createdDate, totalAmount, status, eid) VALUES (26, N'HD202604244', 29, '2026-04-19 00:00:00', 2800000, N'Đã thanh toán', NULL);
INSERT INTO invoices (invoiceId, invoiceNo, bid, createdDate, totalAmount, status, eid) VALUES (27, N'HD202604254', 30, '2026-04-24 00:00:00', 2400000, N'Đã thanh toán', NULL);
INSERT INTO invoices (invoiceId, invoiceNo, bid, createdDate, totalAmount, status, eid) VALUES (28, N'HD202604264', 31, '2026-04-29 00:00:00', 2200000, N'Đã thanh toán', NULL);
INSERT INTO invoices (invoiceId, invoiceNo, bid, createdDate, totalAmount, status, eid) VALUES (29, N'HD202605274', 32, '2026-05-02 00:00:00', 1800000, N'Đã thanh toán', NULL);
INSERT INTO invoices (invoiceId, invoiceNo, bid, createdDate, totalAmount, status, eid) VALUES (30, N'HD202605284', 33, '2026-05-06 00:00:00', 2000000, N'Đã thanh toán', NULL);
INSERT INTO invoices (invoiceId, invoiceNo, bid, createdDate, totalAmount, status, eid) VALUES (31, N'HD202605294', 34, '2026-05-09 00:00:00', 2800000, N'Đã thanh toán', NULL);
INSERT INTO invoices (invoiceId, invoiceNo, bid, createdDate, totalAmount, status, eid) VALUES (32, N'HD202605304', 35, '2026-05-11 00:00:00', 2200000, N'Đã thanh toán', NULL);
INSERT INTO invoices (invoiceId, invoiceNo, bid, createdDate, totalAmount, status, eid) VALUES (33, N'HD202605314', 36, '2026-05-13 00:00:00', 1600000, N'Đã thanh toán', NULL);
INSERT INTO invoices (invoiceId, invoiceNo, bid, createdDate, totalAmount, status, eid) VALUES (34, N'HD202605324', 37, '2026-05-14 00:00:00', 1800000, N'Đã thanh toán', NULL);
INSERT INTO invoices (invoiceId, invoiceNo, bid, createdDate, totalAmount, status, eid) VALUES (35, N'HD202605354', 38, '2026-05-16 00:00:00', 1650000, N'Đã thanh toán', NULL);
INSERT INTO invoices (invoiceId, invoiceNo, bid, createdDate, totalAmount, status, eid) VALUES (36, N'HD202605364', 39, '2026-05-16 00:00:00', 1350000, N'Đã thanh toán', NULL);
INSERT INTO invoices (invoiceId, invoiceNo, bid, createdDate, totalAmount, status, eid) VALUES (37, N'HD202605374', 40, '2026-05-16 00:00:00', 1000000, N'Đã thanh toán', NULL);
INSERT INTO invoices (invoiceId, invoiceNo, bid, createdDate, totalAmount, status, eid) VALUES (38, N'HD202605384', 41, '2026-05-17 00:00:00', 1100000, N'Đã thanh toán', NULL);
INSERT INTO invoices (invoiceId, invoiceNo, bid, createdDate, totalAmount, status, eid) VALUES (39, N'HD202605394', 42, '2026-05-17 00:00:00', 1200000, N'Đã thanh toán', NULL);
INSERT INTO invoices (invoiceId, invoiceNo, bid, createdDate, totalAmount, status, eid) VALUES (40, N'HD202605404', 43, '2026-05-17 00:00:00', 1800000, N'Đã thanh toán', NULL);
INSERT INTO invoices (invoiceId, invoiceNo, bid, createdDate, totalAmount, status, eid) VALUES (41, N'HD202605414', 44, '2026-05-18 00:00:00', 1100000, N'Đã thanh toán', NULL);
INSERT INTO invoices (invoiceId, invoiceNo, bid, createdDate, totalAmount, status, eid) VALUES (42, N'HD202605424', 45, '2026-05-18 00:00:00', 1200000, N'Đã thanh toán', NULL);
INSERT INTO invoices (invoiceId, invoiceNo, bid, createdDate, totalAmount, status, eid) VALUES (43, N'HD202605434', 46, '2026-05-19 00:00:00', 1350000, N'Đã thanh toán', NULL);
INSERT INTO invoices (invoiceId, invoiceNo, bid, createdDate, totalAmount, status, eid) VALUES (44, N'HD202605444', 47, '2026-05-19 00:00:00', 1000000, N'Đã thanh toán', NULL);
INSERT INTO invoices (invoiceId, invoiceNo, bid, createdDate, totalAmount, status, eid) VALUES (45, N'HD202605454', 48, '2026-05-19 00:00:00', 1100000, N'Đã thanh toán', NULL);
INSERT INTO invoices (invoiceId, invoiceNo, bid, createdDate, totalAmount, status, eid) VALUES (46, N'HD202605464', 49, '2026-05-20 00:00:00', 1400000, N'Đã thanh toán', NULL);
INSERT INTO invoices (invoiceId, invoiceNo, bid, createdDate, totalAmount, status, eid) VALUES (47, N'HD202605474', 50, '2026-05-20 00:00:00', 1000000, N'Đã thanh toán', NULL);
INSERT INTO invoices (invoiceId, invoiceNo, bid, createdDate, totalAmount, status, eid) VALUES (48, N'HD202605484', 51, '2026-05-21 00:00:00', 1100000, N'Đã thanh toán', NULL);
INSERT INTO invoices (invoiceId, invoiceNo, bid, createdDate, totalAmount, status, eid) VALUES (49, N'HD202605494', 52, '2026-05-21 00:00:00', 1200000, N'Đã thanh toán', NULL);
INSERT INTO invoices (invoiceId, invoiceNo, bid, createdDate, totalAmount, status, eid) VALUES (50, N'HD202605504', 53, '2026-05-21 00:00:00', 1200000, N'Đã thanh toán', NULL);
INSERT INTO invoices (invoiceId, invoiceNo, bid, createdDate, totalAmount, status, eid) VALUES (51, N'HD202605514', 54, '2026-05-21 00:00:00', 1000000, N'Đã thanh toán', NULL);
INSERT INTO invoices (invoiceId, invoiceNo, bid, createdDate, totalAmount, status, eid) VALUES (52, N'HD202605524', 55, '2026-05-22 00:00:00', 2250000, N'Đã thanh toán', NULL);
INSERT INTO invoices (invoiceId, invoiceNo, bid, createdDate, totalAmount, status, eid) VALUES (53, N'HD202605534', 56, '2026-05-22 00:00:00', 1700000, N'Đã thanh toán', NULL);
INSERT INTO invoices (invoiceId, invoiceNo, bid, createdDate, totalAmount, status, eid) VALUES (54, N'HD202605544', 57, '2026-05-22 00:00:00', 3600000, N'Đã thanh toán', NULL);
INSERT INTO invoices (invoiceId, invoiceNo, bid, createdDate, totalAmount, status, eid) VALUES (55, N'HD20260523A', 71, '2026-05-23 08:15:00', 700000, N'Đã thanh toán', 1);
INSERT INTO invoices (invoiceId, invoiceNo, bid, createdDate, totalAmount, status, eid) VALUES (56, N'HD20260523B', 72, '2026-05-23 09:30:00', 1300000, N'Đã thanh toán', 2);
INSERT INTO invoices (invoiceId, invoiceNo, bid, createdDate, totalAmount, status, eid) VALUES (57, N'HD20260523C', 73, '2026-05-23 10:45:00', 2100000, N'Đã thanh toán', 1);
INSERT INTO invoices (invoiceId, invoiceNo, bid, createdDate, totalAmount, status, eid) VALUES (58, N'HD000058', 70, '2026-05-23 12:13:55', 1760000, N'Đã thanh toán', 2);
INSERT INTO invoices (invoiceId, invoiceNo, bid, createdDate, totalAmount, status, eid) VALUES (59, N'HD000059', 69, '2026-05-23 12:26:58', 715000, N'Đã thanh toán', 2);
SET IDENTITY_INSERT invoices OFF;
GO

-- ----- customer_services -----
SET IDENTITY_INSERT customer_services ON;
INSERT INTO customer_services (id, bid, sid, quantity, used_date) VALUES (1, 1, 1, 3, N'2026-05-10');
INSERT INTO customer_services (id, bid, sid, quantity, used_date) VALUES (2, 1, 2, 2, N'2026-05-10');
INSERT INTO customer_services (id, bid, sid, quantity, used_date) VALUES (3, 1, 3, 1, N'2026-05-11');
INSERT INTO customer_services (id, bid, sid, quantity, used_date) VALUES (4, 2, 6, 1, N'2026-05-12');
INSERT INTO customer_services (id, bid, sid, quantity, used_date) VALUES (5, 2, 4, 1, N'2026-05-14');
INSERT INTO customer_services (id, bid, sid, quantity, used_date) VALUES (6, 2, 7, 3, N'2026-05-13');
INSERT INTO customer_services (id, bid, sid, quantity, used_date) VALUES (7, 3, 1, 5, N'2026-05-18');
INSERT INTO customer_services (id, bid, sid, quantity, used_date) VALUES (8, 3, 5, 2, N'2026-05-19');
INSERT INTO customer_services (id, bid, sid, quantity, used_date) VALUES (9, 3, 8, 1, N'2026-05-20');
INSERT INTO customer_services (id, bid, sid, quantity, used_date) VALUES (10, 4, 2, 2, N'2026-05-21');
INSERT INTO customer_services (id, bid, sid, quantity, used_date) VALUES (11, 5, 4, 5, N'2026-05-22');
INSERT INTO customer_services (id, bid, sid, quantity, used_date) VALUES (12, 60, 2, 3, N'2026-05-22');
INSERT INTO customer_services (id, bid, sid, quantity, used_date) VALUES (13, 66, 2, 2, N'2026-05-23');
INSERT INTO customer_services (id, bid, sid, quantity, used_date) VALUES (14, 66, 1, 4, N'2026-05-23');
INSERT INTO customer_services (id, bid, sid, quantity, used_date) VALUES (15, 67, 6, 1, N'2026-05-23');
INSERT INTO customer_services (id, bid, sid, quantity, used_date) VALUES (16, 67, 2, 2, N'2026-05-23');
INSERT INTO customer_services (id, bid, sid, quantity, used_date) VALUES (17, 68, 5, 1, N'2026-05-23');
INSERT INTO customer_services (id, bid, sid, quantity, used_date) VALUES (18, 69, 4, 1, N'2026-05-23');
INSERT INTO customer_services (id, bid, sid, quantity, used_date) VALUES (19, 70, 6, 2, N'2026-05-23');
INSERT INTO customer_services (id, bid, sid, quantity, used_date) VALUES (20, 70, 8, 1, N'2026-05-23');
INSERT INTO customer_services (id, bid, sid, quantity, used_date) VALUES (21, 70, 4, 2, N'2026-05-23');
SET IDENTITY_INSERT customer_services OFF;
GO

