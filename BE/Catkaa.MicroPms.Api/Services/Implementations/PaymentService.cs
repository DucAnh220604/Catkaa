using Catkaa.MicroPms.Api.Data;
using Catkaa.MicroPms.Api.Helpers;
using Catkaa.MicroPms.Api.Models;
using Catkaa.MicroPms.Api.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace Catkaa.MicroPms.Api.Services.Implementations
{
    public class PaymentService : IPaymentService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public PaymentService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<ServiceResult<string>> CreatePaymentUrlAsync(int bookingId, int? currentUserId, HttpContext context)
        {
            var booking = await _context.Bookings
                .Include(b => b.Room)
                .FirstOrDefaultAsync(b => b.Id == bookingId);

            if (booking == null) return ServiceResult<string>.Fail("Booking not found");

            if (currentUserId != null && booking.UserId != currentUserId && booking.UserId != null)
            {
                return ServiceResult<string>.Fail("Unauthorized Access");
            }

            if (booking.Status != "Pending")
            {
                return ServiceResult<string>.Fail("Booking is not in Pending status");
            }

            var days = (booking.CheckOutDate.Date - booking.CheckInDate.Date).Days;
            if (days <= 0) days = 1;

            decimal totalAmount = days * (booking.Room?.Price ?? 0);
            if (totalAmount <= 0)
            {
                return ServiceResult<string>.Fail("Invalid booking amount");
            }

            var timeNow = DateTime.Now;
            var vnpay = new VnPayLibrary();
            var vnpSettings = _configuration.GetSection("VnPaySettings");

            vnpay.AddRequestData("vnp_Version", vnpSettings["Version"]!);
            vnpay.AddRequestData("vnp_Command", vnpSettings["Command"]!);
            vnpay.AddRequestData("vnp_TmnCode", vnpSettings["TmnCode"]!);
            vnpay.AddRequestData("vnp_Amount", (totalAmount * 100).ToString("0")); 
            vnpay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", vnpay.GetIpAddress(context));
            vnpay.AddRequestData("vnp_Locale", "vn");
            vnpay.AddRequestData("vnp_OrderInfo", $"Thanh toan booking {booking.BookingCode}");
            vnpay.AddRequestData("vnp_OrderType", "other"); 
            vnpay.AddRequestData("vnp_ReturnUrl", vnpSettings["ReturnUrl"]!);
            vnpay.AddRequestData("vnp_TxnRef", booking.Id.ToString() + "_" + timeNow.Ticks.ToString()); 

            var paymentUrl = vnpay.CreateRequestUrl(vnpSettings["BaseUrl"]!, vnpSettings["HashSecret"]!);

            return ServiceResult<string>.Ok("Success", paymentUrl);
        }

        public async Task<ServiceResult<object>> PaymentExecuteIpnAsync(IQueryCollection collections)
        {
            var vnpay = new VnPayLibrary();
            foreach (var (key, value) in collections)
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                {
                    vnpay.AddResponseData(key, value.ToString());
                }
            }

            var vnp_orderIdStr = vnpay.GetResponseData("vnp_TxnRef");
            var vnp_SecureHash = collections["vnp_SecureHash"].ToString();
            var vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
            var vnp_TransactionNo = vnpay.GetResponseData("vnp_TransactionNo");
            var vnp_AmountStr = vnpay.GetResponseData("vnp_Amount");

            var vnpSettings = _configuration.GetSection("VnPaySettings");
            bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnpSettings["HashSecret"]!);

            if (!checkSignature)
            {
                return ServiceResult<object>.Fail("Invalid signature");
            }

            var bookingIdStr = vnp_orderIdStr.Split('_')[0];
            if (!int.TryParse(bookingIdStr, out int bookingId))
            {
                return ServiceResult<object>.Fail("Invalid order ID");
            }

            var booking = await _context.Bookings.FindAsync(bookingId);
            if (booking == null)
            {
                return ServiceResult<object>.Fail("Booking not found");
            }

            if (booking.Status == "Confirmed")
            {
                return ServiceResult<object>.Ok("Order already confirmed");
            }

            decimal amount = 0;
            if (decimal.TryParse(vnp_AmountStr, out decimal rawAmount))
            {
                amount = rawAmount / 100;
            }

            if (vnp_ResponseCode == "00")
            {
                booking.Status = "Confirmed";
                
                var payment = new Payment
                {
                    BookingId = booking.Id,
                    TransactionId = vnp_TransactionNo,
                    Amount = amount,
                    Status = "Success",
                    PaymentDate = DateTime.Now,
                    PaymentMethod = "VNPay"
                };
                _context.Payments.Add(payment);
            }
            else
            {
                var payment = new Payment
                {
                    BookingId = booking.Id,
                    TransactionId = vnp_TransactionNo,
                    Amount = amount,
                    Status = "Failed",
                    PaymentDate = DateTime.Now,
                    PaymentMethod = "VNPay"
                };
                _context.Payments.Add(payment);
            }

            await _context.SaveChangesAsync();
            return ServiceResult<object>.Ok("IPN Handled");
        }
    }
}
