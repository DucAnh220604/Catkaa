using System;
using System.ComponentModel.DataAnnotations;

namespace Catkaa.MicroPms.Api.DTOs
{
    public class OcrCheckInDto
    {
        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public string IdNumber { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }

        public string? ImageUrl { get; set; }
    }
}
