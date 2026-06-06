using Catkaa.MicroPms.Api.Data;
using Catkaa.MicroPms.Api.DTOs;
using Catkaa.MicroPms.Api.Helpers;
using Catkaa.MicroPms.Api.Models;
using Catkaa.MicroPms.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Catkaa.MicroPms.Api.Services.Implementations
{
    public class HotelService : IHotelService
    {
        private readonly ApplicationDbContext _context;

        public HotelService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResult<List<HotelResponseDto>>> GetAllHotelsAsync(string role, int? currentUserId)
        {
            var query = _context.Hotels.AsQueryable();

            if (role == "Host")
            {
                if (currentUserId == null) return ServiceResult<List<HotelResponseDto>>.Fail("Unauthorized Access");
                query = query.Where(h => h.HostId == currentUserId);
            }

            var hotels = await query.ToListAsync();
            
            var response = hotels.Select(h => new HotelResponseDto
            {
                Id = h.Id,
                Name = h.Name,
                Address = h.Address,
                Description = h.Description,
                MainImageUrl = h.MainImageUrl,
                ImageGallery = h.ImageGallery != null ? JsonSerializer.Deserialize<List<string>>(h.ImageGallery) : new List<string>(),
                HostId = h.HostId
            }).ToList();

            return ServiceResult<List<HotelResponseDto>>.Ok("Success", response);
        }

        public async Task<ServiceResult<HotelResponseDto>> GetHotelByIdAsync(int id, string role, int? currentUserId)
        {
            var h = await _context.Hotels
                .Include(x => x.Rooms)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (h == null) return ServiceResult<HotelResponseDto>.Fail("Hotel not found");

            if (role == "Host" && currentUserId != h.HostId)
                return ServiceResult<HotelResponseDto>.Fail("Unauthorized Access");

            return ServiceResult<HotelResponseDto>.Ok("Success", new HotelResponseDto
            {
                Id = h.Id,
                Name = h.Name,
                Address = h.Address,
                Description = h.Description,
                MainImageUrl = h.MainImageUrl,
                ImageGallery = h.ImageGallery != null ? JsonSerializer.Deserialize<List<string>>(h.ImageGallery) : new List<string>(),
                HostId = h.HostId,
                Rooms = h.Rooms.Select(r => new RoomResponseDto
                {
                    Id = r.Id,
                    HotelId = r.HotelId,
                    RoomNumber = r.RoomNumber,
                    RoomType = r.RoomType,
                    Price = r.Price,
                    Status = r.Status,
                    Description = r.Description,
                    MainImageUrl = r.MainImageUrl,
                    ImageGallery = r.ImageGallery != null ? JsonSerializer.Deserialize<List<string>>(r.ImageGallery) : new List<string>()
                }).ToList()
            });
        }

        public async Task<ServiceResult<HotelResponseDto>> CreateHotelAsync(HotelCreateDto dto, int currentUserId)
        {
            var hotel = new Hotel
            {
                Name = dto.Name,
                Address = dto.Address,
                Description = dto.Description,
                MainImageUrl = dto.MainImageUrl,
                ImageGallery = dto.ImageGallery != null ? JsonSerializer.Serialize(dto.ImageGallery) : null,
                HostId = currentUserId
            };
            _context.Hotels.Add(hotel);
            await _context.SaveChangesAsync();

            return ServiceResult<HotelResponseDto>.Ok("Created", new HotelResponseDto
            {
                Id = hotel.Id,
                Name = hotel.Name,
                Address = hotel.Address,
                HostId = hotel.HostId
            });
        }

        public async Task<ServiceResult<object>> UpdateHotelAsync(int id, HotelUpdateDto dto, string role, int? currentUserId)
        {
            var hotel = await _context.Hotels.FindAsync(id);
            if (hotel == null) return ServiceResult<object>.Fail("Hotel not found");

            if (role != "Admin" && currentUserId != hotel.HostId)
                return ServiceResult<object>.Fail("Unauthorized Access");

            hotel.Name = dto.Name;
            hotel.Address = dto.Address;
            hotel.Description = dto.Description;
            hotel.MainImageUrl = dto.MainImageUrl;
            hotel.ImageGallery = dto.ImageGallery != null ? JsonSerializer.Serialize(dto.ImageGallery) : null;

            await _context.SaveChangesAsync();
            return ServiceResult<object>.Ok("Updated");
        }

        public async Task<ServiceResult<object>> DeleteHotelAsync(int id, string role, int? currentUserId)
        {
            var hotel = await _context.Hotels.FindAsync(id);
            if (hotel == null) return ServiceResult<object>.Fail("Hotel not found");

            if (role != "Admin" && currentUserId != hotel.HostId)
                return ServiceResult<object>.Fail("Unauthorized Access");

            _context.Hotels.Remove(hotel);
            await _context.SaveChangesAsync();
            return ServiceResult<object>.Ok("Deleted");
        }
    }
}
