SET NOCOUNT ON;
PRINT N'--- BẮT ĐẦU TẠO DỮ LIỆU ĐỜI THỰC CATKAA PMS ---';

BEGIN TRANSACTION; -- Gói vào Transaction để chạy nhanh và an toàn hơn trong VS
BEGIN TRY

    -- [1] TẠO 3 NGƯỜI DÙNG BÌNH THƯỜNG (Khách du lịch)
    INSERT INTO Users (Username, PasswordHash, Email, Role)
    VALUES 
    ('namnguyen.99', '123456', 'namnguyen.it@gmail.com', 'Guest'),
    ('lan_pham_vp', '123456', 'lanpham.marketing@yahoo.com', 'Guest'),
    ('hoangtran_travel', '123456', 'hoang.tran.business@outlook.com', 'Guest');

    -- [2] TẠO 10 CHỦ ĐẦU TƯ / TẬP ĐOÀN (Chủ khách sạn)
    INSERT INTO Users (Username, PasswordHash, Email, Role)
    VALUES 
    ('minhtuan_ceo', '123456', 'minhtuan@sunshine-invest.com', 'Host'),
    ('ngocbich_79', '123456', 'bichngoc.realestate@gmail.com', 'Host'),
    ('quocbao_group', '123456', 'contact@quocbaogroup.vn', 'Host'),
    ('thuha_boutique', '123456', 'thuha.biz@hotmail.com', 'Host'),
    ('thanhson_re', '123456', 'thanhson.re@vin-property.com', 'Host'),
    ('maianh_vacation', '123456', 'maianh.vacation@gmail.com', 'Host'),
    ('duykhanh_corp', '123456', 'admin@duykhanhcorp.vn', 'Host'),
    ('linhchi_stay', '123456', 'linhchi.homestay@yahoo.com', 'Host'),
    ('hoangviet_invest', '123456', 'hoangviet.vp@gmail.com', 'Host'),
    ('baotram_vn', '123456', 'baotram.vn@outlook.com', 'Host');

    -- [3] TẠO KHÁCH SẠN VÀ PHÒNG BẰNG CURSOR
    DECLARE @HostId INT;
    DECLARE @HostUsername NVARCHAR(50);

    DECLARE host_cursor CURSOR FOR
    SELECT Id, Username FROM Users WHERE Role = 'Host';

    OPEN host_cursor;
    FETCH NEXT FROM host_cursor INTO @HostId, @HostUsername;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        DECLARE @NumHotels INT = ABS(CHECKSUM(NEWID())) % 3 + 1;
        DECLARE @h INT = 1;

        WHILE @h <= @NumHotels
        BEGIN
            DECLARE @NameIdx INT = ABS(CHECKSUM(NEWID())) % 10 + 1;
            DECLARE @BaseName NVARCHAR(100) = CHOOSE(@NameIdx, 
                (1, '101', 'Standard', 500000.00, 'Available', 'Phòng tiêu chuẩn ấm cúng', '/images/room-standard.jpg', '["/images/room1.jpg"]'),
                (1, '102', 'Deluxe', 800000.00, 'Available', 'Phòng Deluxe view đồi núi', '/images/room-deluxe.jpg', '["/images/room2.jpg"]'),
                (1, '201', 'Suite', 1500000.00, 'Available', 'Phòng Suite cao cấp', '/images/room-suite.jpg', '["/images/room3.jpg"]'),
                (2, 'A1', 'Double', 400000.00, 'Available', 'Phòng đôi rộng rãi', '/images/room-double.jpg', '["/images/room4.jpg"]'),
                (2, 'A2', 'Family', 1000000.00, 'Available', 'Phòng gia đình cho 4 người', '/images/room-family.jpg', '["/images/room5.jpg"]'));
            
            DECLARE @HotelName NVARCHAR(100) = @BaseName + N' - Chi nhánh ' + CAST(@h AS VARCHAR);
            
            DECLARE @StreetIdx INT = ABS(CHECKSUM(NEWID())) % 5 + 1;
            DECLARE @Street NVARCHAR(50) = CHOOSE(@StreetIdx, N'Nguyễn Huệ', N'Trần Phú', N'Lê Lợi', N'Bạch Đằng', N'Phan Châu Trinh');
            DECLARE @HotelAddress NVARCHAR(255) = N'Số ' + CAST(ABS(CHECKSUM(NEWID()) % 500) + 1 AS VARCHAR) + N' ' + @Street + N', Khu vực Trung tâm';
            
            DECLARE @HotelDesc NVARCHAR(2000) = N'Khách sạn tiêu chuẩn quốc tế với view siêu đẹp, đầy đủ tiện nghi, thiết kế hiện đại phù hợp cho cả nghỉ dưỡng và công tác. Dịch vụ lễ tân 24/7.';
            DECLARE @MainImg NVARCHAR(500) = 'https://images.unsplash.com/photo-1566073771259-6a8506099945?w=500&q=80';
            DECLARE @ImgGallery NVARCHAR(MAX) = '["https://images.unsplash.com/photo-1582719478250-c89cae4dc85b?w=500", "https://images.unsplash.com/photo-1542314831-c6a4d27ece1f?w=500"]';

            INSERT INTO Hotels (Name, Address, Description, MainImageUrl, ImageGallery, HostId)
            VALUES (@HotelName, @HotelAddress, @HotelDesc, @MainImg, @ImgGallery, @HostId);

            DECLARE @NewHotelId INT = SCOPE_IDENTITY();

            DECLARE @NumRooms INT = ABS(CHECKSUM(NEWID())) % 6 + 5;
            DECLARE @r INT = 1;

            WHILE @r <= @NumRooms
            BEGIN
                DECLARE @Floor INT = ABS(CHECKSUM(NEWID())) % 5 + 1;
                DECLARE @RoomNum NVARCHAR(20) = CAST(@Floor AS VARCHAR) + '0' + CAST(@r AS VARCHAR);
                
                DECLARE @TypeInt INT = ABS(CHECKSUM(NEWID())) % 4 + 1;
                DECLARE @RoomType NVARCHAR(50) = CHOOSE(@TypeInt, 'Standard', 'Deluxe', 'Suite', 'Family');
                
                DECLARE @Price DECIMAL(18,2) = 300000 + (ABS(CHECKSUM(NEWID())) % 22) * 100000;
                
                -- [CẬP NHẬT] Sinh Trạng thái (Status)
                DECLARE @AvailInt INT = ABS(CHECKSUM(NEWID())) % 5 + 1;
                DECLARE @Status NVARCHAR(20) = CHOOSE(@AvailInt, 'Available', 'Available', 'Available', 'Occupied', 'Cleaning'); 
                
                -- [CẬP NHẬT] Sinh random Passcode 8 chữ số
                DECLARE @RoomPassword NVARCHAR(8) = RIGHT('00000000' + CAST(ABS(CHECKSUM(NEWID())) % 100000000 AS VARCHAR), 8);
                
                -- [CẬP NHẬT] Đặt LastCleanedAt nếu phòng đang dọn dẹp
                DECLARE @LastCleanedAt DATETIME = NULL;
                IF @Status = 'Cleaning'
                BEGIN
                    SET @LastCleanedAt = GETDATE();
                END
                
                DECLARE @RoomDesc NVARCHAR(2000) = N'Phòng ' + @RoomType + N' view toàn cảnh, nội thất thông minh, vệ sinh khép kín, miễn phí minibar và dọn phòng hàng ngày.';
                DECLARE @RoomImg NVARCHAR(500) = 'https://images.unsplash.com/photo-1611892440504-42a792e24d32?w=500';
                DECLARE @RoomGallery NVARCHAR(MAX) = '["https://images.unsplash.com/photo-1584622650111-993a426fbf0a?w=500", "https://images.unsplash.com/photo-1578683010236-d716f9a3f461?w=500"]';

                -- [CẬP NHẬT] Insert vào cấu trúc bảng mới
                INSERT INTO Rooms (HotelId, RoomNumber, RoomType, Price, Status, Description, MainImageUrl, ImageGallery, RoomPassword, LastCleanedAt)
                VALUES (@NewHotelId, @RoomNum, @RoomType, @Price, @Status, @RoomDesc, @RoomImg, @RoomGallery, @RoomPassword, @LastCleanedAt);

                SET @r = @r + 1;
            END

            SET @h = @h + 1;
        END

        FETCH NEXT FROM host_cursor INTO @HostId, @HostUsername;
    END

    CLOSE host_cursor;
    DEALLOCATE host_cursor;

    COMMIT TRANSACTION;
    PRINT N'--- HOÀN TẤT DỮ LIỆU ĐỜI THỰC ĐÃ CẬP NHẬT STATUS & PASSCODE! ---';

END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION;
    PRINT N'CÓ LỖI XẢY RA: ' + ERROR_MESSAGE();
END CATCH