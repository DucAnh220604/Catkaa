using Catkaa.MicroPms.Api.Data;
using Catkaa.MicroPms.Api.DTOs;
using Catkaa.MicroPms.Api.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Catkaa.MicroPms.Api.Controllers
{
    [Route("api/pricing-plans")]
    public class PricingPlansController : BaseApiController
    {
        private readonly ApplicationDbContext _context;

        public PricingPlansController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetPricingPlans()
        {
            var plans = await _context.PricingPlans
                .Where(p => p.IsActive)
                .Select(p => new PricingPlanDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Subtitle = p.Subtitle,
                    Price = p.Price,
                    BtnText = p.BtnText,
                    IsPopular = p.IsPopular,
                    Features = JsonSerializer.Deserialize<System.Collections.Generic.List<object>>(p.FeaturesJson, (JsonSerializerOptions)null!)
                })
                .ToListAsync();

            return Ok(ServiceResult<object>.Ok("Success", plans));
        }

        [HttpPost]
        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreatePlan([FromBody] PricingPlanCreateUpdateDto dto)
        {
            var plan = new Catkaa.MicroPms.Api.Models.PricingPlan
            {
                Name = dto.Name,
                Subtitle = dto.Subtitle,
                Price = dto.Price,
                BtnText = dto.BtnText,
                IsPopular = dto.IsPopular,
                FeaturesJson = JsonSerializer.Serialize(dto.Features),
                IsActive = true
            };

            _context.PricingPlans.Add(plan);
            await _context.SaveChangesAsync();

            return Ok(ServiceResult<object>.Ok("Tạo gói dịch vụ thành công", new { plan.Id }));
        }

        [HttpPut("{id}")]
        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdatePlan(int id, [FromBody] PricingPlanCreateUpdateDto dto)
        {
            var plan = await _context.PricingPlans.FindAsync(id);
            if (plan == null || !plan.IsActive) return NotFound(ServiceResult<object>.Fail("Không tìm thấy gói dịch vụ."));

            plan.Name = dto.Name;
            plan.Subtitle = dto.Subtitle;
            plan.Price = dto.Price;
            plan.BtnText = dto.BtnText;
            plan.IsPopular = dto.IsPopular;
            plan.FeaturesJson = JsonSerializer.Serialize(dto.Features);

            await _context.SaveChangesAsync();
            return Ok(ServiceResult<object>.Ok("Cập nhật gói dịch vụ thành công", null));
        }

        [HttpDelete("{id}")]
        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletePlan(int id)
        {
            var plan = await _context.PricingPlans.FindAsync(id);
            if (plan == null || !plan.IsActive) return NotFound(ServiceResult<object>.Fail("Không tìm thấy gói dịch vụ."));

            plan.IsActive = false;
            await _context.SaveChangesAsync();

            return Ok(ServiceResult<object>.Ok("Xóa gói dịch vụ thành công", null));
        }
    }
}
