using System;
using System.ComponentModel.DataAnnotations;

namespace Catkaa.MicroPms.Api.DTOs
{
    public class BookingCreateDto
    {
        [Required]
        public string GuestName { get; set; } = string.Empty;
        
        [Required]
        public string GuestCccd { get; set; } = string.Empty;
        
        public string? GuestEmail { get; set; }
        
        [Required]
        public DateTime CheckInDate { get; set; }
        
        [Required]
        public DateTime CheckOutDate { get; set; }
    }
}
