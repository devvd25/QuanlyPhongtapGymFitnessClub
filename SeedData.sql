USE QuanlyPhongtapGymFitnessClub;
GO

-- Xóa dữ liệu cũ (nếu có) để tránh lỗi trùng lặp khi chạy lại nhiều lần
DELETE FROM Trainers;
DELETE FROM Staffs;
DELETE FROM Users;
GO

-- 1. Thêm 5 Hội viên (Users)
INSERT INTO Users (Email, Username, PasswordHash, PasswordSalt, FullName, Phone, Gender, Role, MembershipPackage, MembershipStatus, Address, CreatedAt, IsActive)
VALUES 
('nguyenvana@gmail.com', 'nguyenvana', 0x01, 0x01, N'Nguyễn Văn A', '0987654321', 'Male', 'Member', 'Basic', 'Active', '', GETDATE(), 1),
('tranthib@gmail.com', 'tranthib', 0x01, 0x01, N'Trần Thị B', '0987654322', 'Female', 'Member', 'VIP', 'Active', '', GETDATE(), 1),
('lehoangc@gmail.com', 'lehoangc', 0x01, 0x01, N'Lê Hoàng C', '0987654323', 'Male', 'Member', 'Diamond', 'Active', '', GETDATE(), 1),
('phamthid@gmail.com', 'phamthid', 0x01, 0x01, N'Phạm Thị D', '0987654324', 'Female', 'Member', 'Basic', 'Active', '', GETDATE(), 1),
('hoangvane@gmail.com', 'hoangvane', 0x01, 0x01, N'Hoàng Văn E', '0987654325', 'Male', 'Member', 'VIP', 'Active', '', GETDATE(), 1);

-- 2. Thêm 3 Nhân viên (Staffs)
INSERT INTO Staffs (Username, FullName, Email, Phone, Gender, DateOfBirth, WorkShift, CounterNumber, Department, HireDate, IsOnDuty, IsActive, CreatedAt)
VALUES 
('nv_tuan', N'Lê Tuấn', 'tuan.le@gym.com', '0912333444', 'Male', '1995-02-10', 'Sáng', 1, 'Lễ tân', '2023-01-15', 1, 1, GETDATE()),
('nv_hoa', N'Nguyễn Hoa', 'hoa.nguyen@gym.com', '0912333555', 'Female', '1998-05-20', 'Chiều', 2, 'Sale', '2023-03-10', 0, 1, GETDATE()),
('nv_dat', N'Trần Đạt', 'dat.tran@gym.com', '0912333666', 'Male', '1997-12-05', 'Tối', 1, 'Lễ tân', '2023-06-01', 1, 1, GETDATE());

-- 3. Thêm 3 Huấn luyện viên (Trainers)
INSERT INTO Trainers (Username, FullName, Email, Phone, Gender, DateOfBirth, Specialty, ExperienceYears, Certifications, Rating, HourlyRate, MaxMembers, AssignedMemberIds, IsActive, CreatedAt)
VALUES 
('hlv_tuan', N'Phan Tuấn Anh', 'tuanpt@gmail.com', '0922333444', 'Male', '1990-03-15', 'Gym', 5, 'NASM, ACE', 4.8, 500000, 10, '[]', 1, GETDATE()),
('hlv_ly', N'Vũ Cẩm Ly', 'lycampt@gmail.com', '0922333555', 'Female', '1997-08-08', 'Yoga', 3, 'RYT 200', 4.9, 400000, 8, '[]', 1, GETDATE()),
('hlv_phong', N'Bùi Thanh Phong', 'phongbody@gmail.com', '0922333666', 'Male', '1992-11-20', 'Bodybuilding', 8, 'IFBB Pro', 5.0, 800000, 6, '[]', 1, GETDATE());

PRINT N'✅ Đã chèn dữ liệu mẫu thành công!';
