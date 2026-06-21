using System.ComponentModel.DataAnnotations;

namespace Catkaa.MicroPms.Api.DTOs
{
    public class ReviewCreateDto
    {
        [Required]
        public int HotelId { get; set; }

        [Required]
        [MaxLength(50)]
        public string BookingCode { get; set; } = string.Empty;

        [Required]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public int Rating { get; set; }

        [MaxLength(1000)]
        public string? Comment { get; set; }
    }
}
