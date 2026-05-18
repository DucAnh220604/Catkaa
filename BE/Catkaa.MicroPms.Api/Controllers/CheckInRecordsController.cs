using Catkaa.MicroPms.Api.DTOs;
using Catkaa.MicroPms.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Catkaa.MicroPms.Api.Controllers
{
    [Route("api/checkinrecords")]
    [Microsoft.AspNetCore.Http.Tags("4. CheckInRecords (Lịch sử lưu trú)")]
    [Authorize(Roles = "Admin, Host")]
    public class CheckInRecordsController : BaseApiController
    {
        private readonly ICheckInRecordService _checkInService;
        private readonly IFptOcrService _fptOcrService;

        public CheckInRecordsController(ICheckInRecordService service, IFptOcrService fptOcrService)
        {
            _checkInService = service;
            _fptOcrService = fptOcrService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int? filterHotelId = null)
        {
            try
            {
                var result = await _checkInService.GetAllAsync(CurrentUserRole, CurrentUserId, filterHotelId);
                if (!result.Success) return Unauthorized(new { message = result.Message });
                return Ok(result.Data);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", details = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _checkInService.GetByIdAsync(id, CurrentUserRole, CurrentUserId);
                if (!result.Success) return NotFound(new { message = result.Message });
                return Ok(result.Data);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", details = ex.Message });
            }
        }

        // ✅ OCR Check-in: Tự động dò Booking bằng CCCD + Hotel + Ngày hôm nay
        [HttpPost("/api/hotels/{hotelId}/checkin-ocr")]
        [Consumes("multipart/form-data")]
        [AllowAnonymous] // Kiosk IoT (Guest) có thể tự gửi ảnh
        public async Task<IActionResult> OcrCheckIn([FromRoute] int hotelId, IFormFile image)
        {
            if (image == null || image.Length == 0) return BadRequest(new { message = "Vui lòng tải lên ảnh CCCD hợp lệ." });


            try
            {
                // Bước 1: Gọi FPT.AI để bóc tách dữ liệu CCCD
                var ocrData = await _fptOcrService.ExtractIdInfoAsync(image);
                if (ocrData == null) return BadRequest(new { message = "Không thể nhận diện CCCD từ ảnh này. Vui lòng thử lại." });

                // Bước 2: Truyền dữ liệu sang Service để dò tìm Booking & tạo CheckInRecord
                var result = await _checkInService.ProcessOcrCheckInAsync(hotelId, ocrData, CurrentUserId ?? 0);
                if (!result.Success) return BadRequest(new { message = result.Message });
                
                return Ok(new { message = result.Message, data = result.Data, ocrRaw = ocrData });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", details = ex.Message });
            }
        }

        // Walk-in manual check-in record
        [HttpPost("/api/hotels/{hotelId}/checkinrecords")]
        public async Task<IActionResult> Create(int hotelId, [FromBody] CheckInRecordCreateDto request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (CurrentUserId == null) return Unauthorized(new { message = "Invalid token: missing user identity" });

            try
            {
                var result = await _checkInService.CreateAsync(hotelId, request, CurrentUserId.Value);
                if (!result.Success) return Unauthorized(new { message = result.Message });

                return Ok(new { message = result.Message, data = result.Data });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", details = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CheckInRecordUpdateDto request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var result = await _checkInService.UpdateAsync(id, request, CurrentUserRole, CurrentUserId);
                if (!result.Success) return Unauthorized(new { message = result.Message });

                return Ok(new { message = result.Message });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", details = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _checkInService.DeleteAsync(id, CurrentUserRole, CurrentUserId);
                if (!result.Success) return Unauthorized(new { message = result.Message });

                return Ok(new { message = result.Message });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", details = ex.Message });
            }
        }
    }
}
