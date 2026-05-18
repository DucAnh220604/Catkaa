using Catkaa.MicroPms.Api.DTOs;
using Catkaa.MicroPms.Api.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catkaa.MicroPms.Api.Services.Interfaces
{
    public interface IHotelService
    {
        Task<ServiceResult<List<HotelResponseDto>>> GetAllHotelsAsync(string role, int? currentUserId);
        Task<ServiceResult<HotelResponseDto>> GetHotelByIdAsync(int id, string role, int? currentUserId);
        Task<ServiceResult<HotelResponseDto>> CreateHotelAsync(HotelCreateDto dto, int currentUserId);
        Task<ServiceResult<object>> UpdateHotelAsync(int id, HotelUpdateDto dto, string role, int? currentUserId);
        Task<ServiceResult<object>> DeleteHotelAsync(int id, string role, int? currentUserId);
    }
}
