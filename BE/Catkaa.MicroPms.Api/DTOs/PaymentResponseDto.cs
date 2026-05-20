using System;

namespace Catkaa.MicroPms.Api.DTOs
{
    public class PaymentResponseDto
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public string BookingCode { get; set; } = string.Empty;
        public string? GuestName { get; set; }
        public int HotelId { get; set; }
        public string? HotelName { get; set; }
        public int RoomId { get; set; }
        public string? RoomNumber { get; set; }
        public string TransactionId { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime PaymentDate { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
    }
}
