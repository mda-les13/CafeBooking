using CafeBooking.BusinessLogic.DTOs;

namespace CafeBooking.BusinessLogic.Services
{
    public interface ITableService
    {
        Task<IEnumerable<TableDto>> GetAllTablesAsync();
        Task<IEnumerable<TableDto>> GetAvailableTablesAsync(DateTime start, int durationHours);
    }
}
