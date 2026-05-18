using Catkaa.MicroPms.Api.DTOs;
using Catkaa.MicroPms.Api.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catkaa.MicroPms.Api.Services.Interfaces
{
    public interface IRoomService
    {
        Task<ServiceResult<List<RoomResponseDto>>> GetAllRoomsAsync(string role, int? currentUserId, int? filterHotelId = null);
        Task<ServiceResult<RoomResponseDto>> GetRoomByIdAsync(int id, string role, int? currentUserId);
        Task<ServiceResult<RoomResponseDto>> CreateRoomAsync(int hotelId, RoomCreateDto dto, int currentUserId);
        Task<ServiceResult<object>> UpdateRoomAsync(int id, RoomUpdateDto dto, string role, int? currentUserId);
        Task<ServiceResult<object>> DeleteRoomAsync(int id, string role, int? currentUserId);
    }
}
