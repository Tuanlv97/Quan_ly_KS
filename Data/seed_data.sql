-- ============================================================
-- SEED DATA: dbMyHotel
-- 20 phòng (201-505, 4 tầng x 5 phòng), 10 nhân viên
-- Chạy trong SQL Server Management Studio hoặc sqlcmd
-- Idempotent: chạy nhiều lần vẫn an toàn
-- ============================================================

USE [dbMyHotel];
GO

-- ── 1. Thêm role "Nhân viên" nếu chưa tồn tại ───────────────
IF NOT EXISTS (SELECT 1 FROM [roles] WHERE [roleName] = N'Nhân viên')
    INSERT INTO [roles] ([roleName],[description]) VALUES (N'Nhân viên', N'Nhân viên thông thường')
GO

-- ── 2. Seed phòng ────────────────────────────────────────────
-- Tổng: 15 Trống | 3 Bẩn | 2 Bảo trì  (4 tầng x 5 phòng = 201-205, 301-305, 401-405, 501-505)

-- ---- Tầng 2 ----
IF NOT EXISTS (SELECT 1 FROM [rooms] WHERE [roomNo] = N'201')
    INSERT INTO [rooms] ([roomNo],[roomType],[bed],[price],[status]) VALUES (N'201',N'Non-Ac',N'Single',  350000, N'Trống')

IF NOT EXISTS (SELECT 1 FROM [rooms] WHERE [roomNo] = N'202')
    INSERT INTO [rooms] ([roomNo],[roomType],[bed],[price],[status]) VALUES (N'202',N'Ac',    N'Double',  550000, N'Trống')

IF NOT EXISTS (SELECT 1 FROM [rooms] WHERE [roomNo] = N'203')
    INSERT INTO [rooms] ([roomNo],[roomType],[bed],[price],[status]) VALUES (N'203',N'Ac',    N'Double',  550000, N'Trống')

IF NOT EXISTS (SELECT 1 FROM [rooms] WHERE [roomNo] = N'204')
    INSERT INTO [rooms] ([roomNo],[roomType],[bed],[price],[status]) VALUES (N'204',N'Non-Ac',N'Single',  350000, N'Trống')

IF NOT EXISTS (SELECT 1 FROM [rooms] WHERE [roomNo] = N'205')
    INSERT INTO [rooms] ([roomNo],[roomType],[bed],[price],[status]) VALUES (N'205',N'Ac',    N'Triple',  750000, N'Bẩn')

-- ---- Tầng 3 ----
IF NOT EXISTS (SELECT 1 FROM [rooms] WHERE [roomNo] = N'301')
    INSERT INTO [rooms] ([roomNo],[roomType],[bed],[price],[status]) VALUES (N'301',N'Ac',    N'Single',  400000, N'Trống')

IF NOT EXISTS (SELECT 1 FROM [rooms] WHERE [roomNo] = N'302')
    INSERT INTO [rooms] ([roomNo],[roomType],[bed],[price],[status]) VALUES (N'302',N'Ac',    N'Double',  600000, N'Trống')

IF NOT EXISTS (SELECT 1 FROM [rooms] WHERE [roomNo] = N'303')
    INSERT INTO [rooms] ([roomNo],[roomType],[bed],[price],[status]) VALUES (N'303',N'Non-Ac',N'Double',  500000, N'Bảo trì')

IF NOT EXISTS (SELECT 1 FROM [rooms] WHERE [roomNo] = N'304')
    INSERT INTO [rooms] ([roomNo],[roomType],[bed],[price],[status]) VALUES (N'304',N'Ac',    N'Double',  600000, N'Trống')

IF NOT EXISTS (SELECT 1 FROM [rooms] WHERE [roomNo] = N'305')
    INSERT INTO [rooms] ([roomNo],[roomType],[bed],[price],[status]) VALUES (N'305',N'Non-Ac',N'Single',  400000, N'Trống')

-- ---- Tầng 4 ----
IF NOT EXISTS (SELECT 1 FROM [rooms] WHERE [roomNo] = N'401')
    INSERT INTO [rooms] ([roomNo],[roomType],[bed],[price],[status]) VALUES (N'401',N'Ac',    N'Double',  650000, N'Trống')

IF NOT EXISTS (SELECT 1 FROM [rooms] WHERE [roomNo] = N'402')
    INSERT INTO [rooms] ([roomNo],[roomType],[bed],[price],[status]) VALUES (N'402',N'Ac',    N'Triple',  850000, N'Bẩn')

IF NOT EXISTS (SELECT 1 FROM [rooms] WHERE [roomNo] = N'403')
    INSERT INTO [rooms] ([roomNo],[roomType],[bed],[price],[status]) VALUES (N'403',N'Ac',    N'Double',  650000, N'Trống')

IF NOT EXISTS (SELECT 1 FROM [rooms] WHERE [roomNo] = N'404')
    INSERT INTO [rooms] ([roomNo],[roomType],[bed],[price],[status]) VALUES (N'404',N'Non-Ac',N'Single',  450000, N'Trống')

IF NOT EXISTS (SELECT 1 FROM [rooms] WHERE [roomNo] = N'405')
    INSERT INTO [rooms] ([roomNo],[roomType],[bed],[price],[status]) VALUES (N'405',N'Ac',    N'Double',  650000, N'Bảo trì')

-- ---- Tầng 5 ----
IF NOT EXISTS (SELECT 1 FROM [rooms] WHERE [roomNo] = N'501')
    INSERT INTO [rooms] ([roomNo],[roomType],[bed],[price],[status]) VALUES (N'501',N'Ac',    N'Triple',  900000, N'Trống')

IF NOT EXISTS (SELECT 1 FROM [rooms] WHERE [roomNo] = N'502')
    INSERT INTO [rooms] ([roomNo],[roomType],[bed],[price],[status]) VALUES (N'502',N'Ac',    N'Double',  700000, N'Trống')

IF NOT EXISTS (SELECT 1 FROM [rooms] WHERE [roomNo] = N'503')
    INSERT INTO [rooms] ([roomNo],[roomType],[bed],[price],[status]) VALUES (N'503',N'Ac',    N'Triple',  900000, N'Bẩn')

IF NOT EXISTS (SELECT 1 FROM [rooms] WHERE [roomNo] = N'504')
    INSERT INTO [rooms] ([roomNo],[roomType],[bed],[price],[status]) VALUES (N'504',N'Non-Ac',N'Single',  500000, N'Trống')

IF NOT EXISTS (SELECT 1 FROM [rooms] WHERE [roomNo] = N'505')
    INSERT INTO [rooms] ([roomNo],[roomType],[bed],[price],[status]) VALUES (N'505',N'Ac',    N'Double',  700000, N'Trống')
GO

-- ── 3. Seed nhân viên ─────────────────────────────────────────
-- rid=2 → Lễ Tân  |  rid động → Nhân viên
-- gender: "Nam" / "Nu" (theo chuẩn GenderToStore trong UC_Emloyee.cs)

DECLARE @ridLeTan INT = (SELECT rid FROM [roles] WHERE [roleName] = N'Lễ Tân')
DECLARE @ridNV    INT = (SELECT rid FROM [roles] WHERE [roleName] = N'Nhân viên')

-- ---- 5 Lễ Tân ----
IF NOT EXISTS (SELECT 1 FROM [employee] WHERE [username] = 'letannv01')
    INSERT INTO [employee] ([ename],[mobile],[gender],[emailid],[username],[pass],[rid])
    VALUES (N'Nguyễn Văn An', N'0912345678', N'Nam', 'nva.hotel@gmail.com', 'letannv01', '123456', @ridLeTan)

IF NOT EXISTS (SELECT 1 FROM [employee] WHERE [username] = 'letannv02')
    INSERT INTO [employee] ([ename],[mobile],[gender],[emailid],[username],[pass],[rid])
    VALUES (N'Trần Thị Bình',  N'0987654321', N'Nu',  'ttb.hotel@gmail.com', 'letannv02', '123456', @ridLeTan)

IF NOT EXISTS (SELECT 1 FROM [employee] WHERE [username] = 'letannv03')
    INSERT INTO [employee] ([ename],[mobile],[gender],[emailid],[username],[pass],[rid])
    VALUES (N'Lê Văn Cường',   N'0934567890', N'Nam', 'lvc.hotel@gmail.com', 'letannv03', '123456', @ridLeTan)

IF NOT EXISTS (SELECT 1 FROM [employee] WHERE [username] = 'letannv04')
    INSERT INTO [employee] ([ename],[mobile],[gender],[emailid],[username],[pass],[rid])
    VALUES (N'Phạm Thị Dung',  N'0965432109', N'Nu',  'ptd.hotel@gmail.com', 'letannv04', '123456', @ridLeTan)

IF NOT EXISTS (SELECT 1 FROM [employee] WHERE [username] = 'letannv05')
    INSERT INTO [employee] ([ename],[mobile],[gender],[emailid],[username],[pass],[rid])
    VALUES (N'Hoàng Văn Đức',  N'0978901234', N'Nam', 'hvd.hotel@gmail.com', 'letannv05', '123456', @ridLeTan)

-- ---- 5 Nhân viên ----
IF NOT EXISTS (SELECT 1 FROM [employee] WHERE [username] = 'nhanvien01')
    INSERT INTO [employee] ([ename],[mobile],[gender],[emailid],[username],[pass],[rid])
    VALUES (N'Võ Thị Phương',  N'0943210987', N'Nu',  'vtp.hotel@gmail.com', 'nhanvien01', '123456', @ridNV)

IF NOT EXISTS (SELECT 1 FROM [employee] WHERE [username] = 'nhanvien02')
    INSERT INTO [employee] ([ename],[mobile],[gender],[emailid],[username],[pass],[rid])
    VALUES (N'Đặng Văn Giang', N'0956789012', N'Nam', 'dvg.hotel@gmail.com', 'nhanvien02', '123456', @ridNV)

IF NOT EXISTS (SELECT 1 FROM [employee] WHERE [username] = 'nhanvien03')
    INSERT INTO [employee] ([ename],[mobile],[gender],[emailid],[username],[pass],[rid])
    VALUES (N'Bùi Thị Hoa',    N'0921098765', N'Nu',  'bth.hotel@gmail.com', 'nhanvien03', '123456', @ridNV)

IF NOT EXISTS (SELECT 1 FROM [employee] WHERE [username] = 'nhanvien04')
    INSERT INTO [employee] ([ename],[mobile],[gender],[emailid],[username],[pass],[rid])
    VALUES (N'Đỗ Văn Khải',    N'0969876543', N'Nam', 'dvk.hotel@gmail.com', 'nhanvien04', '123456', @ridNV)

IF NOT EXISTS (SELECT 1 FROM [employee] WHERE [username] = 'nhanvien05')
    INSERT INTO [employee] ([ename],[mobile],[gender],[emailid],[username],[pass],[rid])
    VALUES (N'Ngô Thị Lan',    N'0932109876', N'Nu',  'ntl.hotel@gmail.com', 'nhanvien05', '123456', @ridNV)
GO

-- ── 4. Kiểm tra kết quả ──────────────────────────────────────
SELECT roomNo, roomType, bed, price, status FROM [rooms] ORDER BY roomNo
GO

SELECT
    e.eid,
    e.ename,
    e.mobile,
    e.gender,
    r.roleName AS quyen
FROM [employee] e
LEFT JOIN [roles] r ON r.rid = e.rid
ORDER BY e.eid
GO
