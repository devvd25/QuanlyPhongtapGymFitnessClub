USE QuanlyPhongtapGymFitnessClub;
GO

-- Xoa du lieu cu
DELETE FROM InvoiceItems;
DELETE FROM Invoices;
DBCC CHECKIDENT ('Invoices',     RESEED, 0);
DBCC CHECKIDENT ('InvoiceItems', RESEED, 0);
GO

-- Lay Id cac ban ghi hien co
DECLARE @Staff1 INT = (SELECT TOP 1 Id FROM Staffs WHERE Username = 'nv_tuan');
DECLARE @Staff2 INT = (SELECT TOP 1 Id FROM Staffs WHERE Username = 'nv_hoa');
DECLARE @User1  INT = (SELECT TOP 1 Id FROM Users  WHERE Username = 'nguyenvana');
DECLARE @User2  INT = (SELECT TOP 1 Id FROM Users  WHERE Username = 'tranthib');
DECLARE @User3  INT = (SELECT TOP 1 Id FROM Users  WHERE Username = 'lehoangc');
DECLARE @P1 INT = (SELECT TOP 1 Id FROM Products WHERE Name LIKE N'%Lavie%');
DECLARE @P2 INT = (SELECT TOP 1 Id FROM Products WHERE Name LIKE N'%Monster%');
DECLARE @P3 INT = (SELECT TOP 1 Id FROM Products WHERE Name LIKE N'%Whey%');
DECLARE @P4 INT = (SELECT TOP 1 Id FROM Products WHERE Name LIKE N'%Creatine%');
DECLARE @P5 INT = (SELECT TOP 1 Id FROM Products WHERE Name LIKE N'%Shaker%');

-- Kiem tra du lieu ton tai chua
SELECT 'Staff1=' + CAST(ISNULL(@Staff1,0) AS NVARCHAR), 
       'Staff2=' + CAST(ISNULL(@Staff2,0) AS NVARCHAR),
       'User1='  + CAST(ISNULL(@User1,0)  AS NVARCHAR),
       'P1='     + CAST(ISNULL(@P1,0)     AS NVARCHAR);

-- Hoa don 1: Nguyen Van A mua Nuoc Lavie + Whey Protein (nhan vien: nv_tuan)
INSERT INTO Invoices (InvoiceCode, UserId, StaffId, TotalAmount, PaymentMethod, Status, Notes, CreatedAt)
VALUES ('INV-20260930-0001', @User1, @Staff1, 1870000, 'Cash', 'Paid', N'Hoi vien mua nhieu - tang nuoc', GETDATE());
DECLARE @Inv1 INT = SCOPE_IDENTITY();
INSERT INTO InvoiceItems (InvoiceId, ProductId, ProductName, Quantity, UnitPrice, SubTotal)
VALUES 
(@Inv1, @P1, N'Nuoc khoang Lavie 500ml',         2, 10000,   20000),
(@Inv1, @P3, N'Whey Protein Gold Standard 5lbs',  1, 1850000, 1850000);

-- Hoa don 2: Tran Thi B mua Monster Energy + Creatine (nhan vien: nv_hoa)
INSERT INTO Invoices (InvoiceCode, UserId, StaffId, TotalAmount, PaymentMethod, Status, Notes, CreatedAt)
VALUES ('INV-20260930-0002', @User2, @Staff2, 485000, 'Card', 'Paid', NULL, GETDATE());
DECLARE @Inv2 INT = SCOPE_IDENTITY();
INSERT INTO InvoiceItems (InvoiceId, ProductId, ProductName, Quantity, UnitPrice, SubTotal)
VALUES 
(@Inv2, @P2, N'Nuoc tang luc Monster Energy', 1, 35000,  35000),
(@Inv2, @P4, N'Creatine Monohydrate 300g',    1, 450000, 450000);

-- Hoa don 3: Le Hoang C mua Shaker (nhan vien: nv_tuan)
INSERT INTO Invoices (InvoiceCode, UserId, StaffId, TotalAmount, PaymentMethod, Status, Notes, CreatedAt)
VALUES ('INV-20260930-0003', @User3, @Staff1, 120000, 'Transfer', 'Paid', NULL, GETDATE());
DECLARE @Inv3 INT = SCOPE_IDENTITY();
INSERT INTO InvoiceItems (InvoiceId, ProductId, ProductName, Quantity, UnitPrice, SubTotal)
VALUES 
(@Inv3, @P5, N'Binh lac Shaker 700ml', 1, 120000, 120000);

-- Ket qua
SELECT 'Invoices' AS Bang, COUNT(*) AS SoLuong FROM Invoices
UNION ALL
SELECT 'InvoiceItems', COUNT(*) FROM InvoiceItems;

PRINT N'Da nap xong 3 hoa don va 6 dong chi tiet!';
