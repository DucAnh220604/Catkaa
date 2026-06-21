using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Catkaa.MicroPms.Api.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }

        public int HotelId { get; set; }

        public int UserId { get; set; }

        [Required]
        [MaxLength(50)]
        public string BookingCode { get; set; } = string.Empty;

        [Range(1, 5)]
        public int Rating { get; set; }

        public string? Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [ForeignKey("HotelId")]
        public Hotel? Hotel { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }
    }
}
