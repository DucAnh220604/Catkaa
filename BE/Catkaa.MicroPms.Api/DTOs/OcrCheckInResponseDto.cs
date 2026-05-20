using System;

namespace Catkaa.MicroPms.Api.DTOs
{
    public class OcrCheckInResponseDto
    {
        public int CheckInRecordId { get; set; }
        public int BookingId { get; set; }
        public string BookingCode { get; set; } = string.Empty;
        public string GuestName { get; set; } = string.Empty;
        public int HotelId { get; set; }
        public int RoomId { get; set; }
        public DateTime CheckInTime { get; set; }
        public string? PaymentUrl { get; set; }
    }
}
