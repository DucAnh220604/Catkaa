using System;

namespace Catkaa.MicroPms.Api.DTOs
{
    public class CheckInRecordResponseDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string IdentityNumber { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; }
        public int HotelId { get; set; }
        public string? RoomId { get; set; }
        public DateTime CheckInTime { get; set; }
        public string? ImageUrl { get; set; }
        public int? BookingId { get; set; }
    }
}
