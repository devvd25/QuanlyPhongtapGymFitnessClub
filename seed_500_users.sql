USE [QuanlyPhongtapGymFitnessClub];
GO

-- 1. Lấy Id lớn nhất hiện tại để tạo tiếp 500 user không bị trùng
DECLARE @startId INT = ISNULL((SELECT MAX(Id) FROM [Users]), 0);
DECLARE @i INT = @startId + 1;
DECLARE @max INT = @startId + 500; -- Chèn đúng 500 user mới

DECLARE @Username NVARCHAR(50);
DECLARE @Email NVARCHAR(100);
DECLARE @FullName NVARCHAR(100);
DECLARE @Phone NVARCHAR(15);

WHILE @i <= @max
BEGIN
    SET @Username = CONCAT('user', @i);
    SET @Email = CONCAT('user', @i, '@example.com');
    SET @FullName = CONCAT(N'Hội viên ', @i);
    SET @Phone = CONCAT('09', RIGHT('00000000' + CAST(@i AS VARCHAR(8)), 8));

    -- Kiểm tra không trùng Username hoặc Email
    IF NOT EXISTS (SELECT 1 FROM [Users] WHERE Username = @Username OR Email = @Email)
    BEGIN
        INSERT INTO [Users] (
            [Username],
            [Email],
            [FullName],
            [Phone],
            [Gender],
            [Address],
            [MembershipPackage],
            [MembershipStatus],
            [RemainingPtSessions],
            [IsActive],
            [IsDeleted],
            [PasswordHash],
            [PasswordSalt],
            [CreatedAt]
        )
        VALUES (
            @Username, 
            @Email, 
            @FullName,
            @Phone,
            CASE WHEN @i % 2 = 0 THEN 'Male' ELSE 'Female' END,
            N'Đà Nẵng',
            CASE WHEN @i % 3 = 0 THEN 'VIP' ELSE 'Basic' END,
            'Active',
            0, -- Số buổi PT còn lại mặc định là 0
            1,
            0,
            HASHBYTES('SHA2_256', N'Password123'), 
            0x0102030405060708, 
            DATEADD(DAY, -(@i % 365), GETUTCDATE())
        );
    END

    SET @i = @i + 1;
END;
GO

-- 2. Kiểm tra tổng số record trong bảng Users sau khi chèn
SELECT COUNT(*) AS [TongSoUserTrongHeThong] FROM [Users];
GO
