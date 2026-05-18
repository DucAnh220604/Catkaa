using System.Collections.Generic;

namespace Catkaa.MicroPms.Api.DTOs
{
    public class HotelResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? MainImageUrl { get; set; }
        public List<string>? ImageGallery { get; set; }
        public int HostId { get; set; }
        public List<RoomResponseDto>? Rooms { get; set; }
    }
}
