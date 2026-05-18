using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Catkaa.MicroPms.Api.Models
{
    public class CheckInRecord
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string IdentityNumber { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }

        public int HotelId { get; set; }
        
        [ForeignKey(nameof(HotelId))]
        public Hotel? Hotel { get; set; }

        public DateTime CheckInTime { get; set; }
        
        public DateTime? CheckOutTime { get; set; }

        [MaxLength(50)]
        public string? RoomId { get; set; }

        [MaxLength(500)]
        public string? ImageUrl { get; set; }

        public int? BookingId { get; set; }

        [ForeignKey(nameof(BookingId))]
        public Booking? Booking { get; set; }
    }
}
