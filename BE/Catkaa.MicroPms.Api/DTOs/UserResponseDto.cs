using System.Collections.Generic;

namespace Catkaa.MicroPms.Api.DTOs
{
    public class UserResponseDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Role { get; set; }
        public List<HotelDto> Hotels { get; set; } = new List<HotelDto>();
    }

    public class HotelDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
    }
}
