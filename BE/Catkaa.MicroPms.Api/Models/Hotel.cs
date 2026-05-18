using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Catkaa.MicroPms.Api.Models
{
    public class Hotel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string Address { get; set; } = string.Empty;

        [MaxLength(2000)]
        public string? Description { get; set; }

        [MaxLength(500)]
        public string? MainImageUrl { get; set; }

        // Lưu dưới dạng JSON array: ["url1","url2",...]
        public string? ImageGallery { get; set; }

        public int HostId { get; set; }
        
        [ForeignKey(nameof(HostId))]
        public User? Host { get; set; }

        public ICollection<CheckInRecord> CheckInRecords { get; set; } = new List<CheckInRecord>();
        public ICollection<Room> Rooms { get; set; } = new List<Room>();
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
