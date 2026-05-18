using Catkaa.MicroPms.Api.Helpers;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Catkaa.MicroPms.Api.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<ServiceResult<string>> CreatePaymentUrlAsync(int bookingId, int? currentUserId, HttpContext context);
        Task<ServiceResult<object>> PaymentExecuteIpnAsync(IQueryCollection collections);
    }
}
