using System;
using System.ComponentModel.DataAnnotations;

namespace Catkaa.MicroPms.Api.DTOs
{
    public class CheckInRecordUpdateDto
    {
        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public string IdentityNumber { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }

        public string? RoomId { get; set; }

        public string? ImageUrl { get; set; }

        public int? BookingId { get; set; }
    }
}
