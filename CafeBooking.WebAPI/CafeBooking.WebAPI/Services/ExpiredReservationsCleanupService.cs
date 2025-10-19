using CafeBooking.BusinessLogic.Services;

namespace CafeBooking.WebAPI.Services
{
    public class ExpiredReservationsCleanupService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ExpiredReservationsCleanupService> _logger;

        public ExpiredReservationsCleanupService(
            IServiceProvider serviceProvider,
            ILogger<ExpiredReservationsCleanupService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Фоновый сервис очистки просроченных броней запущен");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var reservationService = scope.ServiceProvider.GetRequiredService<IReservationService>();
                        await reservationService.DeleteExpiredReservationsAsync(DateTime.Now);
                        _logger.LogInformation("Проверка просроченных броней выполнена");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Ошибка при удалении просроченных броней");
                }

                await Task.Delay(TimeSpan.FromMinutes(30), stoppingToken);
            }
        }
    }
}
