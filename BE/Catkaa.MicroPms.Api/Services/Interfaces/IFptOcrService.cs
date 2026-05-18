using Catkaa.MicroPms.Api.DTOs;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Catkaa.MicroPms.Api.Services.Interfaces
{
    public interface IFptOcrService
    {
        Task<OcrCheckInDto?> ExtractIdInfoAsync(IFormFile image);
    }
}
