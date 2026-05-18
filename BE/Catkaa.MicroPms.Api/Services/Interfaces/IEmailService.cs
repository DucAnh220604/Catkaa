using System.Threading.Tasks;

namespace Catkaa.MicroPms.Api.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendBookingInfoAsync(string email, string bookingCode);
    }
}
