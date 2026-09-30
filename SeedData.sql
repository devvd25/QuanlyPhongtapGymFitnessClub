USE QuanlyPhongtapGymFitnessClub;
GO

-- 1. Xóa dữ liệu cũ theo đúng thứ tự ràng buộc khóa ngoại (Users xóa trước Trainers)
DELETE FROM Users;
DELETE FROM Trainers;
DELETE FROM Staffs;
DELETE FROM Products;
GO

-- 2. Thêm 3 Huấn luyện viên (Trainers) trước để lấy Id liên kết
INSERT INTO Trainers (Username, FullName, Email, Phone, Gender, DateOfBirth, Specialty, ExperienceYears, Certifications, Rating, HourlyRate, MaxMembers, IsActive, CreatedAt)
VALUES 
('hlv_tuan', N'Phan Tuấn Anh', 'tuanpt@gmail.com', '0922333444', 'Male', '1990-03-15', 'Gym', 5, 'NASM, ACE', 4.8, 500000, 10, 1, GETDATE()),
('hlv_ly', N'Vũ Cẩm Ly', 'lycampt@gmail.com', '0922333555', 'Female', '1997-08-08', 'Yoga', 3, 'RYT 200', 4.9, 400000, 8, 1, GETDATE()),
('hlv_phong', N'Bùi Thanh Phong', 'phongbody@gmail.com', '0922333666', 'Male', '1992-11-20', 'Bodybuilding', 8, 'IFBB Pro', 5.0, 800000, 6, 1, GETDATE());

-- Lấy Id của 3 Huấn luyện viên vừa thêm
DECLARE @TuanId INT = (SELECT TOP 1 Id FROM Trainers WHERE Username = 'hlv_tuan');
DECLARE @LyId INT = (SELECT TOP 1 Id FROM Trainers WHERE Username = 'hlv_ly');
DECLARE @PhongId INT = (SELECT TOP 1 Id FROM Trainers WHERE Username = 'hlv_phong');

-- 3. Thêm 5 Hội viên (Users) - có liên kết Khóa ngoại AssignedTrainerId
INSERT INTO Users (Email, Username, PasswordHash, PasswordSalt, FullName, Phone, Gender, Role, MembershipPackage, MembershipStatus, Address, RemainingPtSessions, AssignedTrainerId, IsDeleted, CreatedAt, IsActive)
VALUES 
('nguyenvana@gmail.com', 'nguyenvana', 0x01, 0x01, N'Nguyễn Văn A', '0987654321', 'Male', 'Member', 'Basic', 'Active', N'123 Lê Duẩn, Đà Nẵng', 10, @TuanId, 0, GETDATE(), 1),
('tranthib@gmail.com', 'tranthib', 0x01, 0x01, N'Trần Thị B', '0987654322', 'Female', 'Member', 'VIP', 'Active', N'456 Nguyễn Văn Linh, Đà Nẵng', 15, @LyId, 0, GETDATE(), 1),
('lehoangc@gmail.com', 'lehoangc', 0x01, 0x01, N'Lê Hoàng C', '0987654323', 'Male', 'Member', 'Diamond', 'Active', N'789 Điện Biên Phủ, Đà Nẵng', 20, @PhongId, 0, GETDATE(), 1),
('phamthid@gmail.com', 'phamthid', 0x01, 0x01, N'Phạm Thị D', '0987654324', 'Female', 'Member', 'Basic', 'Active', N'12 Hùng Vương, Đà Nẵng', 0, NULL, 0, GETDATE(), 1),
('hoangvane@gmail.com', 'hoangvane', 0x01, 0x01, N'Hoàng Văn E', '0987654325', 'Male', 'Member', 'VIP', 'Active', N'34 Bạch Đằng, Đà Nẵng', 5, @TuanId, 0, GETDATE(), 1);

-- 4. Thêm 3 Nhân viên (Staffs)
INSERT INTO Staffs (Username, FullName, Email, Phone, Gender, DateOfBirth, WorkShift, CounterNumber, Department, HireDate, IsOnDuty, IsActive, CreatedAt)
VALUES 
('nv_tuan', N'Lê Tuấn', 'tuan.le@gym.com', '0912333444', 'Male', '1995-02-10', N'Sáng', 1, N'Lễ tân', '2023-01-15', 1, 1, GETDATE()),
('nv_hoa', N'Nguyễn Hoa', 'hoa.nguyen@gym.com', '0912333555', 'Female', '1998-05-20', N'Chiều', 2, N'Sale', '2023-03-10', 0, 1, GETDATE()),
('nv_dat', N'Trần Đạt', 'dat.tran@gym.com', '0912333666', 'Male', '1997-12-05', N'Tối', 1, N'Lễ tân', '2023-06-01', 1, 1, GETDATE());

-- 5. Thêm 6 Sản phẩm Gym (Products)
INSERT INTO Products (Name, Description, Price, StockQuantity, Category, ImageUrl, IsActive, CreatedAt)
VALUES 
(N'Nước khoáng Lavie 500ml', N'Nước khoáng thiên nhiên bù nước khi tập', 10000, 100, 'Drink', '', 1, GETDATE()),
(N'Nước tăng lực Monster Energy', N'Nước tăng lực giải khát và tỉnh táo trước buổi tập', 35000, 50, 'Drink', '', 1, GETDATE()),
(N'Whey Protein Gold Standard 5lbs', N'Sữa tăng cơ cao cấp vị Socola thơm ngon', 1850000, 20, 'Supplement', '', 1, GETDATE()),
(N'Creatine Monohydrate 300g', N'Bổ sung sức mạnh và tăng cường kích thước cơ bắp', 450000, 30, 'Supplement', '', 1, GETDATE()),
(N'Bình lắc Shaker 700ml', N'Bình pha whey và bcaa cao cấp chống tràn', 120000, 40, 'Accessory', '', 1, GETDATE()),
(N'Găng tay tập Gym Pro', N'Găng tay da đệm mút chống chai tay khi nâng tạ', 250000, 25, 'Accessory', '', 1, GETDATE());

PRINT N'✅ Đã nạp dữ liệu mẫu thành công cho cả 4 bảng (Users, Trainers, Staffs, Products) có đầy đủ Khóa ngoại!';