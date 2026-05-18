using Catkaa.MicroPms.Api.Data;
using Catkaa.MicroPms.Api.DTOs;
using Catkaa.MicroPms.Api.Helpers;
using Catkaa.MicroPms.Api.Models;
using Catkaa.MicroPms.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catkaa.MicroPms.Api.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResult<List<UserResponseDto>>> GetAllUsersAsync(string role, int? currentUserId)
        {
            if (role != "Admin" && role != "Host")
                return ServiceResult<List<UserResponseDto>>.Fail("Unauthorized Access");

            var query = _context.Users.Include(u => u.Hotels).AsQueryable();
            if (role == "Host")
                query = query.Where(u => u.Role == "Guest");

            var users = await query.Select(u => new UserResponseDto
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                Role = u.Role,
                Hotels = u.Hotels.Select(h => new HotelDto
                {
                    Id = h.Id,
                    Name = h.Name,
                    Address = h.Address
                }).ToList()
            }).ToListAsync();

            return ServiceResult<List<UserResponseDto>>.Ok("Success", users);
        }

        public async Task<ServiceResult<UserResponseDto>> GetUserByIdAsync(int id, string role, int? currentUserId)
        {
            if (role != "Admin" && role != "Host")
                return ServiceResult<UserResponseDto>.Fail("Unauthorized Access");

            var u = await _context.Users
                .Include(u => u.Hotels)
                .FirstOrDefaultAsync(u => u.Id == id);
            if (u == null) return ServiceResult<UserResponseDto>.Fail("User not found");
            if (role == "Host" && u.Role != "Guest")
                return ServiceResult<UserResponseDto>.Fail("Unauthorized Access");

            return ServiceResult<UserResponseDto>.Ok("Success", new UserResponseDto
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                Role = u.Role,
                Hotels = u.Hotels.Select(h => new HotelDto
                {
                    Id = h.Id,
                    Name = h.Name,
                    Address = h.Address
                }).ToList()
            });
        }

        public async Task<ServiceResult<UserResponseDto>> CreateUserAsync(UserCreateDto dto, string role)
        {
            if (role != "Admin" && role != "Host")
                return ServiceResult<UserResponseDto>.Fail("Unauthorized Access");
            if (role == "Host" && dto.Role != "Guest")
                return ServiceResult<UserResponseDto>.Fail("Host chỉ được tạo tài khoản Guest");

            var user = new User
            {
                Username = dto.Username,
                PasswordHash = dto.Password,
                Email = dto.Email,
                Role = dto.Role
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Load hotels
            var hotels = await _context.Hotels
                .Where(h => h.HostId == user.Id)
                .ToListAsync();

            return ServiceResult<UserResponseDto>.Ok("Created", new UserResponseDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role,
                Hotels = hotels.Select(h => new HotelDto 
                { 
                    Id = h.Id, 
                    Name = h.Name, 
                    Address = h.Address 
                }).ToList()
            });
        }

        public async Task<ServiceResult<object>> UpdateUserAsync(int id, UserUpdateDto dto, string role, int? currentUserId)
        {
            if (role != "Admin" && role != "Host")
                return ServiceResult<object>.Fail("Unauthorized Access");

            var user = await _context.Users.FindAsync(id);
            if (user == null) return ServiceResult<object>.Fail("User not found");
            if (role == "Host" && user.Role != "Guest")
                return ServiceResult<object>.Fail("Unauthorized Access");

            user.Username = dto.Username;
            user.Email = dto.Email;
            user.Role = dto.Role;

            if (!string.IsNullOrEmpty(dto.Password))
            {
                user.PasswordHash = dto.Password;
            }

            await _context.SaveChangesAsync();
            return ServiceResult<object>.Ok("Updated");
        }

        public async Task<ServiceResult<object>> DeleteUserAsync(int id, string role, int? currentUserId)
        {
            if (role != "Admin" && role != "Host")
                return ServiceResult<object>.Fail("Unauthorized Access");

            var user = await _context.Users.FindAsync(id);
            if (user == null) return ServiceResult<object>.Fail("User not found");
            if (role == "Host" && user.Role != "Guest")
                return ServiceResult<object>.Fail("Unauthorized Access");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return ServiceResult<object>.Ok("Deleted");
        }
    }
}
