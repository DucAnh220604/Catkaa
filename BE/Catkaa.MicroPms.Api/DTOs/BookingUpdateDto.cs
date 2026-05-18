using System;
using System.ComponentModel.DataAnnotations;

namespace Catkaa.MicroPms.Api.DTOs
{
    public class BookingUpdateDto
    {
        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = string.Empty;

        [Required]
        public DateTime CheckInDate { get; set; }

        [Required]
        public DateTime CheckOutDate { get; set; }

        public int RoomId { get; set; }
    }
}
