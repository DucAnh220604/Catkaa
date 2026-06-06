using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Catkaa.MicroPms.Api.Data;

namespace Catkaa.MicroPms.Api.Services.BackgroundWorkers
{
    public class RoomCleaningWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<RoomCleaningWorker> _logger;
        private readonly int _durationMinutes;

        public RoomCleaningWorker(IServiceProvider serviceProvider, ILogger<RoomCleaningWorker> logger, IConfiguration config)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _durationMinutes = config.GetValue<int>("CleaningConfig:DurationMinutes", 60); // Default to 60 if not configured
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"RoomCleaningWorker started. Cleaning duration configured to {_durationMinutes} minutes.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await ProcessCleaningRoomsAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred executing RoomCleaningWorker.");
                }

                // Check every 1 minute
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }

        private async Task ProcessCleaningRoomsAsync(CancellationToken stoppingToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var thresholdTime = DateTime.Now.AddMinutes(-_durationMinutes);

            var roomsToClean = await context.Rooms
                .Where(r => r.Status == "Cleaning" && r.LastCleanedAt != null && r.LastCleanedAt <= thresholdTime)
                .ToListAsync(stoppingToken);

            if (!roomsToClean.Any()) return;

            foreach (var room in roomsToClean)
            {
                room.Status = "Available";
                room.LastCleanedAt = null;
                room.RoomPassword = GenerateRandomPasscode();
                _logger.LogInformation($"Room {room.RoomNumber} (ID: {room.Id}) has finished cleaning. Status set to Available with new passcode.");
            }

            await context.SaveChangesAsync(stoppingToken);
        }

        private string GenerateRandomPasscode()
        {
            // 8 digits numeric password
            return Random.Shared.Next(10000000, 99999999).ToString();
        }
    }
}
