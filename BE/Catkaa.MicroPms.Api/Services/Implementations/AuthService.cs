using Catkaa.MicroPms.Api.Data;
using Catkaa.MicroPms.Api.DTOs;
using Catkaa.MicroPms.Api.Helpers;
using Catkaa.MicroPms.Api.Models;
using Catkaa.MicroPms.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Catkaa.MicroPms.Api.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<ServiceResult<string>> LoginAsync(LoginRequestDto request)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == request.Username && u.PasswordHash == request.Password);

            if (user == null)
            {
                return ServiceResult<string>.Fail("Invalid username or password");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"] ?? "ThisIsMySuperSecretKeyForCatkaaMicroPmsMustBeAtLeast32BytesLong!");
            
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role ?? "Guest")
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(double.Parse(_configuration["JwtSettings:ExpiryMinutes"] ?? "1440")),
                Issuer = _configuration["JwtSettings:Issuer"],
                Audience = _configuration["JwtSettings:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return ServiceResult<string>.Ok("Login successful", tokenString);
        }

        public async Task<ServiceResult<object?>> RegisterAsync(RegisterDto request)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                if (await _context.Users.AnyAsync(u => u.Username == request.Username))
                {
                    return ServiceResult<object?>.Fail("Username already exists");
                }

                if (request.Role == "Host")
                {
                    if (string.IsNullOrWhiteSpace(request.HotelName) || string.IsNullOrWhiteSpace(request.HotelAddress))
                    {
                        return ServiceResult<object?>.Fail("HotelName and HotelAddress are required for Host registration.");
                    }
                }

                var user = new User
                {
                    Username = request.Username,
                    PasswordHash = request.Password,
                    Email = request.Email,
                    Role = request.Role == "Host" ? "Host" : "Guest"
                };
                
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                if (user.Role == "Host")
                {
                    var hotel = new Hotel
                    {
                        Name = request.HotelName!,
                        Address = request.HotelAddress!,
                        HostId = user.Id
                    };

                    _context.Hotels.Add(hotel);
                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();

                return ServiceResult<object?>.Ok("Registration successful", new { user.Id, user.Username, user.Role });
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
