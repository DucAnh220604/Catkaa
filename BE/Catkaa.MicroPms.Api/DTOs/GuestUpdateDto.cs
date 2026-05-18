using System;
using System.ComponentModel.DataAnnotations;

namespace Catkaa.MicroPms.Api.DTOs
{
    public class GuestUpdateDto
    {
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string IdentityNumber { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }

        public string? RoomId { get; set; }

        public string? ImageUrl { get; set; }

        public DateTime? CheckOutTime { get; set; }
    }
}
