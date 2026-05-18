using Catkaa.MicroPms.Api.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace Catkaa.MicroPms.Api.Services.Implementations
{
    public class MockEmailService : IEmailService
    {
        public Task SendBookingInfoAsync(string email, string bookingCode)
        {
            Console.WriteLine($"[MOCK EMAIL] To: {email} | Subject: Booking Confirmation | Body: Your booking code is {bookingCode}");
            return Task.CompletedTask;
        }
    }
}
