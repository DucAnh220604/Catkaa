# CATKAA MicroPMS

Hệ thống quản lý khách sạn / homestay tích hợp thanh toán VNPay và nhận diện CCCD qua OCR (FPT AI).

## Tech Stack

| Layer     | Công nghệ                              |
|-----------|----------------------------------------|
| Backend   | .NET 8 Web API, EF Core, SQLite (dev)  |
| Frontend  | React 18 + TypeScript + Vite           |
| Thanh toán| VNPay Sandbox                          |
| OCR       | FPT AI Vision API                      |
| Tunnel    | ngrok (dev local)                      |

---

## Yêu cầu cài đặt

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Node.js 18+](https://nodejs.org/)
- [ngrok](https://ngrok.com/download) (chỉ cần cho VNPay local)
- Tài khoản VNPay Sandbox ([đăng ký tại đây](https://sandbox.vnpayment.vn/devreg/))

---

## Cấu hình `appsettings.json`

File `BE/Catkaa.MicroPms.Api/appsettings.json` **không được commit** (đã có trong `.gitignore`).  
Tạo file này thủ công từ template dưới đây:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=catkaa-dev.db"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "JwtSettings": {
    "SecretKey": "ThisIsMySuperSecretKeyForCatkaaMicroPmsMustBeAtLeast32BytesLong!",
    "Issuer": "CatkaaMicroPms",
    "Audience": "CatkaaMicroPmsUsers",
    "ExpiryMinutes": 1440
  },
  "VnPaySettings": {
    "TmnCode": "YOUR_TMN_CODE",
    "HashSecret": "YOUR_HASH_SECRET",
    "BaseUrl": "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html",
    "ReturnUrl": "https://YOUR_NGROK_DOMAIN/api/payments/vnpay-return",
    "IpnUrl": "https://YOUR_NGROK_DOMAIN/api/payments/vnpay-ipn",
    "Version": "2.1.0",
    "Command": "pay"
  },
  "FptAi": {
    "BaseAddress": "https://api.fpt.ai/vision/idr/vnm",
    "ApiKey": "YOUR_FPT_AI_KEY"
  }
}
```

> **Lấy TmnCode và HashSecret**: Đăng nhập [VNPay Sandbox Portal](https://sandbox.vnpayment.vn/merchantv2/) → Thông tin tài khoản → Thông tin thanh toán.

---

## Thiết lập ngrok cho VNPay (từng bước)

VNPay yêu cầu một URL **HTTPS công khai** (không chấp nhận `localhost`) cho `ReturnUrl` và `IpnUrl`.  
ngrok tạo một đường hầm (tunnel) từ internet vào máy local của bạn.

### Bước 1 — Cài ngrok

Tải về tại [https://ngrok.com/download](https://ngrok.com/download), giải nén và thêm vào PATH.  
Kiểm tra: `ngrok version`

### Bước 2 — Đăng nhập và lấy Authtoken

1. Đăng ký tài khoản miễn phí tại [ngrok.com](https://ngrok.com).
2. Vào **Dashboard → Your Authtoken**, copy token.
3. Chạy lệnh:
   ```bash
   ngrok config add-authtoken <YOUR_AUTHTOKEN>
   ```

### Bước 3 — Lấy Static Domain (bắt buộc với free plan)

Free plan chỉ cho 1 domain, nhưng domain đó **cố định** — không đổi mỗi lần restart.

1. Vào [ngrok Dashboard → Domains](https://dashboard.ngrok.com/cloud-edge/domains).
2. Click **New Domain** → ngrok tạo cho bạn một domain dạng `xxxx-yyyy-zzzz.ngrok-free.app`.
3. Copy domain đó (ví dụ: `unadored-candy-intervascular.ngrok-free.dev`).

### Bước 4 — Chạy ngrok trỏ vào cổng HTTP của BE

> **QUAN TRỌNG**: Trỏ vào **HTTP** (`http://localhost:5096`), **KHÔNG** dùng HTTPS (`https://localhost:7191`).  
> Lý do: ngrok không tin tưởng chứng chỉ tự ký của .NET dev server → lỗi `ERR_NGROK_8012`.

```bash
ngrok http --domain=unadored-candy-intervascular.ngrok-free.dev http://localhost:5096
```

Khi chạy thành công, terminal hiển thị:
```
Forwarding  https://unadored-candy-intervascular.ngrok-free.dev -> http://localhost:5096
```

### Bước 5 — Cập nhật `appsettings.json`

Thay `YOUR_NGROK_DOMAIN` bằng domain ngrok của bạn:

```json
"ReturnUrl": "https://unadored-candy-intervascular.ngrok-free.dev/api/payments/vnpay-return",
"IpnUrl":    "https://unadored-candy-intervascular.ngrok-free.dev/api/payments/vnpay-ipn"
```

### Bước 6 — Cập nhật VNPay Sandbox Portal

1. Đăng nhập [VNPay Sandbox Portal](https://sandbox.vnpayment.vn/merchantv2/).
2. Vào **Quản lý website** → chọn website của bạn.
3. Cập nhật **URL thanh toán** = `https://YOUR_NGROK_DOMAIN`.
4. Cập nhật **URL IPN** = `https://YOUR_NGROK_DOMAIN/api/payments/vnpay-ipn`.
5. Lưu lại.

> **Lưu ý**: Nếu không cấu hình được IPN trên portal, hệ thống vẫn hoạt động bình thường vì `vnpay-return` tự xử lý như một fallback.

### Kiểm tra ngrok hoạt động

Mở trình duyệt: `https://YOUR_NGROK_DOMAIN/api/health` (hoặc bất kỳ endpoint nào của API).  
Nếu trả về dữ liệu JSON → ngrok đang hoạt động đúng.

### Lỗi thường gặp

| Lỗi | Nguyên nhân | Cách sửa |
|-----|-------------|----------|
| `ERR_NGROK_8012` | Trỏ vào HTTPS port | Đổi sang `http://localhost:5096` |
| `invalid signature` (VNPay) | HashSecret sai hoặc có khoảng trắng | Copy lại từ email/portal, không có dấu cách |
| Trang cảnh báo ngrok | Chưa thêm header `ngrok-skip-browser-warning` | Chỉ ảnh hưởng khi mở trực tiếp trên trình duyệt, không ảnh hưởng API |
| `502 Bad Gateway` | BE chưa chạy hoặc chạy sai port | Đảm bảo BE đang chạy trên `http://localhost:5096` |

---

## Chạy dự án

### Backend

```bash
cd BE/Catkaa.MicroPms.Api
dotnet run --launch-profile "http"
```

API sẽ chạy tại `http://localhost:5096`.

> Lần đầu chạy, EF Core tự tạo database `catkaa-dev.db` trong thư mục dự án.

### Frontend

```bash
cd FE
npm install
npm run dev
```

App sẽ chạy tại `http://localhost:5173`.

### Thứ tự khởi động đúng

1. Chạy **Backend** (`dotnet run`)
2. Chạy **ngrok** (tunnel vào `http://localhost:5096`)
3. Cập nhật `ReturnUrl` / `IpnUrl` trong `appsettings.json` nếu domain thay đổi
4. Restart Backend để load config mới
5. Chạy **Frontend** (`npm run dev`)

---

## Luồng thanh toán VNPay

```
Guest chọn thanh toán
       │
       ▼
POST /api/payments/create-payment-url
       │   (BE tạo URL có chữ ký HMAC-SHA512)
       ▼
Mở tab mới → VNPay Sandbox
       │   (Guest nhập thông tin thẻ test)
       ▼
VNPay gọi ReturnUrl (ngrok → BE)
       │   GET /api/payments/vnpay-return
       │   (BE xác thực chữ ký, cập nhật DB)
       ▼
BE redirect → http://localhost:5173/payment-result
       │
       ▼
Tab kết quả gửi postMessage → Tab GuestFlow
       │   (thông báo thanh toán thành công)
       ▼
GuestFlow cập nhật UI "Thanh toán thành công"
```

### Thẻ test VNPay Sandbox

| Thông tin | Giá trị |
|-----------|---------|
| Ngân hàng | NCB |
| Số thẻ    | `9704198526191432198` |
| Tên chủ thẻ | `NGUYEN VAN A` |
| Ngày phát hành | `07/15` |
| Mật khẩu OTP | `123456` |

---

## Cấu trúc dự án

```
Catkaa/
├── BE/
│   └── Catkaa.MicroPms.Api/
│       ├── Controllers/        # API endpoints
│       ├── DTOs/               # Request/Response models
│       ├── Helpers/            # VnPayLibrary (HMAC-SHA512)
│       ├── Models/             # EF Core entities
│       ├── Services/           # Business logic
│       ├── Data/               # DbContext
│       └── appsettings.json    # ← KHÔNG commit (có trong .gitignore)
└── FE/
    └── src/
        ├── pages/              # GuestFlow, OwnerDashboard, GuestHistory...
        ├── services/           # API calls
        ├── components/         # Header, Layout...
        └── router/             # AppRouter
```

---

## Roles & Phân quyền

| Role  | Quyền |
|-------|-------|
| Admin | Xem tất cả booking, payment của mọi hotel |
| Host  | Chỉ xem booking, payment của hotel mình quản lý |
| Guest | Đặt phòng, check-in OCR, thanh toán, xem lịch sử cá nhân |
