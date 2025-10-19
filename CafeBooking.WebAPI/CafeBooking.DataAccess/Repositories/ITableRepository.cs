using CafeBooking.DataAccess.Entities;

namespace CafeBooking.DataAccess.Repositories
{
    public interface ITableRepository
    {
        Task<Table> GetByIdAsync(int id);
        Task<IEnumerable<Table>> GetAllAsync();
        Task<IEnumerable<Table>> GetAvailableTablesAsync(DateTime start, int durationHours);
    }
}
