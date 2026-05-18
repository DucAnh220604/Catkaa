using Catkaa.MicroPms.Api.DTOs;
using Catkaa.MicroPms.Api.Helpers;
using System.Threading.Tasks;

namespace Catkaa.MicroPms.Api.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ServiceResult<string>> LoginAsync(LoginRequestDto request);
        Task<ServiceResult<object?>> RegisterAsync(RegisterDto request);
    }
}
