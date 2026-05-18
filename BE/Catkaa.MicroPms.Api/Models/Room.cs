using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Catkaa.MicroPms.Api.Models
{
    public class Room
    {
        [Key]
        public int Id { get; set; }

        public int HotelId { get; set; }

        [ForeignKey(nameof(HotelId))]
        public Hotel? Hotel { get; set; }

        [Required]
        [MaxLength(20)]
        public string RoomNumber { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string RoomType { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public bool IsAvailable { get; set; } = true;

        [MaxLength(2000)]
        public string? Description { get; set; }

        [MaxLength(500)]
        public string? MainImageUrl { get; set; }

        // Lưu dưới dạng JSON array: ["url1","url2",...]
        public string? ImageGallery { get; set; }
    }
}
