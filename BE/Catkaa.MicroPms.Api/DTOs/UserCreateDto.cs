using System.ComponentModel.DataAnnotations;

namespace Catkaa.MicroPms.Api.DTOs
{
    public class UserCreateDto
    {
        [Required]
        [MaxLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [MaxLength(256)]
        public string Password { get; set; } = string.Empty;

        [EmailAddress]
        [MaxLength(100)]
        public string? Email { get; set; }

        [MaxLength(20)]
        public string? Role { get; set; }
    }
}
