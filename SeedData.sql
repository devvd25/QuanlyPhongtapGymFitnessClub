USE QuanlyPhongtapGymFitnessClub;
GO

-- 1. Xoa du lieu cu theo dung thu tu rang buoc khoa ngoai
DELETE FROM InvoiceItems;
DELETE FROM Invoices;
DELETE FROM Users;
DELETE FROM Trainers;
DELETE FROM Staffs;
DELETE FROM Products;
GO

-- Reset Identity counters
DBCC CHECKIDENT ('InvoiceItems', RESEED, 0);
DBCC CHECKIDENT ('Invoices',     RESEED, 0);
DBCC CHECKIDENT ('Users',        RESEED, 0);
DBCC CHECKIDENT ('Trainers',     RESEED, 0);
DBCC CHECKIDENT ('Staffs',       RESEED, 0);
DBCC CHECKIDENT ('Products',     RESEED, 0);
GO

-- 2. Them 3 Huan luyen vien (Trainers) truoc de lay Id lien ket
INSERT INTO Trainers (Username, FullName, Email, Phone, Gender, DateOfBirth, Specialty, Certifications, Rating, HourlyRate, MaxMembers, IsActive, CreatedAt)
VALUES 
('hlv_tuan',  N'Phan Tuấn Anh',    'tuanpt@gmail.com',    '0922333444', 'Male',   '1990-03-15', 'Gym',          'NASM, ACE', 4.8, 500000, 10, 1, GETDATE()),
('hlv_ly',    N'Vũ Cẩm Ly',        'lycampt@gmail.com',   '0922333555', 'Female', '1997-08-08', 'Yoga',         'RYT 200',   4.9, 400000,  8, 1, GETDATE()),
('hlv_phong', N'Bùi Thanh Phong',  'phongbody@gmail.com', '0922333666', 'Male',   '1992-11-20', 'Bodybuilding', 'IFBB Pro',  5.0, 800000,  6, 1, GETDATE());

-- Lay Id cua 3 Huan luyen vien vua them
DECLARE @TuanId  INT = (SELECT TOP 1 Id FROM Trainers WHERE Username = 'hlv_tuan');
DECLARE @LyId    INT = (SELECT TOP 1 Id FROM Trainers WHERE Username = 'hlv_ly');
DECLARE @PhongId INT = (SELECT TOP 1 Id FROM Trainers WHERE Username = 'hlv_phong');

-- Salt va Hash cua mat khau 'password123'
DECLARE @PwdSalt VARBINARY(128) = 0xCF251B124FFF5574E1842EA96C8FA6BA1D49711C4EF24506275910F842F18C9920535687397D8DE17174C9E85DE4E617E73AA6AA87710CA9CFD5937500BE275607CA25C0B1F940CE7A826850358934805118ACF8D15804AAD11E36627273E78E12B4B057489E6E70D0298B40CAF95F4110FD470D217DE47115ECA2AC9BCE3850;
DECLARE @PwdHash VARBINARY(64)  = 0xE9D578713A7283E7FA377EE070BD9746EAFF69EB9DDBFB60EB4CD0F4BC679B954B1BFF33856DCB62C57AFB506C26AE0A0CF3D53F9F46E2531DD303A4DF7CA5C7;

-- 3. Them 5 Hoi vien (Users) - co lien ket Khoa ngoai AssignedTrainerId, mat khau mac dinh la 'password123'
INSERT INTO Users (Email, Username, PasswordHash, PasswordSalt, FullName, Phone, Gender, MembershipPackage, MembershipStatus, Address, RemainingPtSessions, AssignedTrainerId, IsDeleted, CreatedAt, IsActive)
VALUES 
('nguyenvana@gmail.com', 'nguyenvana', @PwdHash, @PwdSalt, N'Nguyễn Văn A', '0987654321', 'Male',   'Basic',   'Active', N'123 Lê Duẩn, Đà Nẵng',              10, @TuanId,  0, GETDATE(), 1),
('tranthib@gmail.com',   'tranthib',   @PwdHash, @PwdSalt, N'Trần Thị B',   '0987654322', 'Female', 'VIP',     'Active', N'456 Nguyễn Văn Linh, Đà Nẵng',      15, @LyId,    0, GETDATE(), 1),
('lehoangc@gmail.com',   'lehoangc',   @PwdHash, @PwdSalt, N'Lê Hoàng C',   '0987654323', 'Male',   'Diamond', 'Active', N'789 Điện Biên Phủ, Đà Nẵng',        20, @PhongId, 0, GETDATE(), 1),
('phamthid@gmail.com',   'phamthid',   @PwdHash, @PwdSalt, N'Phạm Thị D',   '0987654324', 'Female', 'Basic',   'Active', N'12 Hùng Vương, Đà Nẵng',             0, NULL,     0, GETDATE(), 1),
('hoangvane@gmail.com',  'hoangvane',  @PwdHash, @PwdSalt, N'Hoàng Văn E',  '0987654325', 'Male',   'VIP',     'Active', N'34 Bạch Đằng, Đà Nẵng',              5, @TuanId,  0, GETDATE(), 1);

-- 4. Them 3 Nhan vien (Staffs) - WorkShift: Morning, Afternoon, Evening
INSERT INTO Staffs (Username, FullName, Email, Phone, Gender, DateOfBirth, WorkShift, CounterNumber, Department, HireDate, IsOnDuty, IsActive, CreatedAt)
VALUES 
('nv_tuan', N'Lê Tuấn',     'tuan.le@gym.com',    '0912333444', 'Male',   '1995-02-10', 'Morning',   1, 'Reception', '2023-01-15', 1, 1, GETDATE()),
('nv_hoa',  N'Nguyễn Hoa',  'hoa.nguyen@gym.com', '0912333555', 'Female', '1998-05-20', 'Afternoon', 2, 'Sales',     '2023-03-10', 0, 1, GETDATE()),
('nv_dat',  N'Trần Đạt',    'dat.tran@gym.com',   '0912333666', 'Male',   '1997-12-05', 'Evening',   1, 'Reception', '2023-06-01', 1, 1, GETDATE());

-- 5. Them 6 San pham Gym (Products)
INSERT INTO Products (Name, Description, Price, StockQuantity, Category, ImageUrl, IsActive, CreatedAt)
VALUES 
(N'Nước khoáng Lavie 500ml',          N'Nước khoáng thiên nhiên bổ nước khi tập',           10000,   100, 'Drink',      '', 1, GETDATE()),
(N'Nước tăng lực Monster Energy',     N'Nước tăng lực giải khát và tỉnh táo trước buổi tập', 35000,    50, 'Drink',      '', 1, GETDATE()),
(N'Whey Protein Gold Standard 5lbs',  N'Sữa tăng cơ cao cấp vị Socola thơm ngon',         1850000,    20, 'Supplement', '', 1, GETDATE()),
(N'Creatine Monohydrate 300g',        N'Bổ sung sức mạnh và tăng cường kích thước cơ bắp',  450000,    30, 'Supplement', '', 1, GETDATE()),
(N'Bình lắc Shaker 700ml',            N'Bình pha whey và bcaa cao cấp chống tràn',           120000,    40, 'Accessory',  '', 1, GETDATE()),
(N'Găng tay tập Gym Pro',             N'Găng tay da êm mát chống chai tay khi nâng tạ',      250000,    25, 'Accessory',  '', 1, GETDATE());

-- 6. Them 3 Hoa don mau (Invoices)
DECLARE @Staff1 INT = (SELECT TOP 1 Id FROM Staffs WHERE Username = 'nv_tuan');
DECLARE @Staff2 INT = (SELECT TOP 1 Id FROM Staffs WHERE Username = 'nv_hoa');
DECLARE @User1  INT = (SELECT TOP 1 Id FROM Users  WHERE Username = 'nguyenvana');
DECLARE @User2  INT = (SELECT TOP 1 Id FROM Users  WHERE Username = 'tranthib');
DECLARE @User3  INT = (SELECT TOP 1 Id FROM Users  WHERE Username = 'lehoangc');
DECLARE @P1 INT = (SELECT TOP 1 Id FROM Products WHERE Name = N'Nước khoáng Lavie 500ml');
DECLARE @P2 INT = (SELECT TOP 1 Id FROM Products WHERE Name = N'Nước tăng lực Monster Energy');
DECLARE @P3 INT = (SELECT TOP 1 Id FROM Products WHERE Name = N'Whey Protein Gold Standard 5lbs');
DECLARE @P4 INT = (SELECT TOP 1 Id FROM Products WHERE Name = N'Creatine Monohydrate 300g');
DECLARE @P5 INT = (SELECT TOP 1 Id FROM Products WHERE Name = N'Bình lắc Shaker 700ml');

-- Hoa don 1
INSERT INTO Invoices (InvoiceCode, UserId, StaffId, TotalAmount, PaymentMethod, Status, Notes, CreatedAt)
VALUES ('INV-20260930-0001', @User1, @Staff1, 1870000, 'Cash', 'Paid', N'Hội viên mua combo', GETDATE());
DECLARE @Inv1 INT = SCOPE_IDENTITY();
INSERT INTO InvoiceItems (InvoiceId, ProductId, ProductName, Quantity, UnitPrice, SubTotal)
VALUES 
(@Inv1, @P1, N'Nước khoáng Lavie 500ml',         2, 10000,   20000),
(@Inv1, @P3, N'Whey Protein Gold Standard 5lbs',  1, 1850000, 1850000);

-- Hoa don 2
INSERT INTO Invoices (InvoiceCode, UserId, StaffId, TotalAmount, PaymentMethod, Status, Notes, CreatedAt)
VALUES ('INV-20260930-0002', @User2, @Staff2, 485000, 'Card', 'Paid', NULL, GETDATE());
DECLARE @Inv2 INT = SCOPE_IDENTITY();
INSERT INTO InvoiceItems (InvoiceId, ProductId, ProductName, Quantity, UnitPrice, SubTotal)
VALUES 
(@Inv2, @P2, N'Nước tăng lực Monster Energy', 1, 35000,  35000),
(@Inv2, @P4, N'Creatine Monohydrate 300g',    1, 450000, 450000);

-- Hoa don 3
INSERT INTO Invoices (InvoiceCode, UserId, StaffId, TotalAmount, PaymentMethod, Status, Notes, CreatedAt)
VALUES ('INV-20260930-0003', @User3, @Staff1, 120000, 'Transfer', 'Paid', NULL, GETDATE());
DECLARE @Inv3 INT = SCOPE_IDENTITY();
INSERT INTO InvoiceItems (InvoiceId, ProductId, ProductName, Quantity, UnitPrice, SubTotal)
VALUES 
(@Inv3, @P5, N'Bình lắc Shaker 700ml', 1, 120000, 120000);

PRINT N'✅ Đã nạp thành công toàn bộ dữ liệu mẫu!';
