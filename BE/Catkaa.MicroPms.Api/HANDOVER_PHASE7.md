# CATKAA Micro-PMS: Project Master Context (Phase 7.5 - Post-Refactor)

## 1. Tổng quan & Kiến trúc
- Hệ thống Quản lý Khách sạn đa chi nhánh (1 Host - N Hotels).
- Tech stack: .NET 8 Web API, Entity Framework Core, SQL Server, JWT Authentication, Service Layer Pattern.
- Data Modeling: Sử dụng `Primitive Collections` (JSON string) để lưu trữ danh sách hình ảnh trực tiếp trong bảng.

## 2. Trạng thái hiện tại (Đã hoàn thành Phase 7.5)
- **Vá IDOR Update:** Loại bỏ nhập `HostId` thủ công trong các DTO, lấy tự động từ JWT Token.
- **RESTful Routes:** Chuyển `HotelId`, `RoomId` lên URL parameters (VD: `/api/hotels/{hotelId}/rooms`).
- **Gộp luồng Check-in:** Xóa API cũ, tạo luồng thông minh `/api/hotels/{hotelId}/checkin-ocr` để tự động dò Booking.
- **Mở public API:** Guest/Anonymous dùng hàm GET xem danh sách toàn bộ khách sạn và phòng (không bị chặn bởi `HostId`).
- **Eager Loading:** Hàm GET Hotel trả về kèm danh sách các Room.

## 3. Cấu trúc Database Hình ảnh (Mới cập nhật)
- Bảng `Hotel` và `Room` đã có `MainImageUrl` (String) và `Description` (String).
- `ImageGallery` (Hotel) và `RoomImages` (Room) lưu dưới dạng `List<string>`. EF Core tự động serialize thành JSON.

## 4. Kế hoạch tiếp theo (Phase 8)
- Tích hợp gọi API OCR thực tế (Option 1) để trích xuất Name, IdNumber, và DateOfBirth từ ảnh CCCD.