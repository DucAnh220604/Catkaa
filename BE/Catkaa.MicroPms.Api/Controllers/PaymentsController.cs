using Catkaa.MicroPms.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Catkaa.MicroPms.Api.Controllers
{
    [Route("api/payments")]
    [Microsoft.AspNetCore.Http.Tags("9. Payments (Thanh toán VNPay)")]
    public class PaymentsController : BaseApiController
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("create-url/{bookingId}")]
        [Authorize]
        public async Task<IActionResult> CreatePaymentUrl(int bookingId)
        {
            try
            {
                var result = await _paymentService.CreatePaymentUrlAsync(bookingId, CurrentUserId, HttpContext);
                if (!result.Success) return BadRequest(new { message = result.Message });

                return Ok(new { paymentUrl = result.Data });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", details = ex.Message });
            }
        }

        [HttpGet("vnpay-ipn")]
        [AllowAnonymous]
        public async Task<IActionResult> PaymentExecuteIpn()
        {
            try
            {
                var result = await _paymentService.PaymentExecuteIpnAsync(Request.Query);
                
                if (result.Success)
                {
                    return Ok(new { RspCode = "00", Message = "Confirm Success" });
                }
                else
                {
                    return Ok(new { RspCode = "97", Message = "Invalid signature or failed" });
                }
            }
            catch (System.Exception)
            {
                return Ok(new { RspCode = "99", Message = "Unknown error" });
            }
        }

        [HttpGet("vnpay-return")]
        [AllowAnonymous]
        public IActionResult PaymentReturn()
        {
            var vnp_ResponseCode = Request.Query["vnp_ResponseCode"];
            var vnp_TxnRef = Request.Query["vnp_TxnRef"];

            if (vnp_ResponseCode == "00")
            {
                return Ok(new { message = "Payment successful", bookingRef = vnp_TxnRef });
            }
            else
            {
                return BadRequest(new { message = "Payment failed", bookingRef = vnp_TxnRef });
            }
        }
    }
}
