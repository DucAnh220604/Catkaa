using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Catkaa.MicroPms.Api.DTOs
{
    public class RoomCreateDto
    {
        [Required]
        [MaxLength(20)]
        public string RoomNumber { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string RoomType { get; set; } = string.Empty;

        public decimal Price { get; set; }
        
        public bool IsAvailable { get; set; } = true;

        [MaxLength(2000)]
        public string? Description { get; set; }

        [MaxLength(500)]
        public string? MainImageUrl { get; set; }

        public List<string>? ImageGallery { get; set; } = new();
    }
}
