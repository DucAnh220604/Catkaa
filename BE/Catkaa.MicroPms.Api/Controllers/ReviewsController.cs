using Catkaa.MicroPms.Api.Data;
using Catkaa.MicroPms.Api.DTOs;
using Catkaa.MicroPms.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Catkaa.MicroPms.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ReviewsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateReview([FromBody] ReviewCreateDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int currentUserId))
            {
                return Unauthorized(new { message = "Không xác định được người dùng." });
            }

            // Validate if the booking code is valid
            var booking = await _context.Bookings.FirstOrDefaultAsync(b => b.BookingCode == request.BookingCode);
            if (booking == null)
            {
                return BadRequest(new { message = "Booking không hợp lệ." });
            }
            if (booking.Status != "CheckOut")
            {
                return BadRequest(new { message = "Phòng phải ở trạng thái CheckOut mới được phép đánh giá." });
            }

            // Anti-spam: Kiểm tra xem Booking này đã được đánh giá chưa
            var existingReview = await _context.Reviews.AnyAsync(r => r.BookingCode == request.BookingCode);
            if (existingReview)
            {
                return BadRequest(new { message = "Booking này đã được đánh giá!" });
            }

            var review = new Review
            {
                HotelId = booking.HotelId, // Lấy trực tiếp từ Booking thay vì trust Frontend (do Frontend thiếu HotelId)
                BookingCode = request.BookingCode,
                UserId = currentUserId,
                Rating = request.Rating,
                Comment = request.Comment,
                CreatedAt = DateTime.UtcNow
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Đánh giá thành công!", data = review });
        }

        [HttpGet("hotel/{hotelId}")]
        public async Task<IActionResult> GetReviewsByHotel(int hotelId)
        {
            var reviews = await _context.Reviews
                .Include(r => r.User)
                .Where(r => r.HotelId == hotelId)
                .OrderByDescending(r => r.CreatedAt)
                .Select(r => new
                {
                    r.Id,
                    r.Rating,
                    r.Comment,
                    r.CreatedAt,
                    Username = r.User != null ? r.User.Username : "Guest"
                })
                .ToListAsync();

            return Ok(new { data = reviews });
        }
    }
}
