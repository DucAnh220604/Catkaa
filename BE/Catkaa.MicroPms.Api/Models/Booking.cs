using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Catkaa.MicroPms.Api.Models
{
    public class Booking
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string BookingCode { get; set; } = string.Empty;

        public int? UserId { get; set; } 

        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }

        [MaxLength(100)]
        public string? GuestName { get; set; }

        [MaxLength(20)]
        public string? GuestCccd { get; set; }

        [EmailAddress]
        [MaxLength(100)]
        public string? GuestEmail { get; set; }

        public int HotelId { get; set; }

        [ForeignKey(nameof(HotelId))]
        public Hotel? Hotel { get; set; }

        public int RoomId { get; set; }

        [ForeignKey(nameof(RoomId))]
        public Room? Room { get; set; }

        public DateTime CheckInDate { get; set; }
        
        public DateTime CheckOutDate { get; set; }

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "Pending"; 
    }
}
