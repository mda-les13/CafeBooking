using CafeBooking.DataAccess.Context;
using CafeBooking.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace CafeBooking.DataAccess.Repositories
{
    public class TableRepository : ITableRepository
    {
        private readonly CafeDbContext _context;

        public TableRepository(CafeDbContext context)
        {
            _context = context;
        }

        public async Task<Table> GetByIdAsync(int id)
        {
            return await _context.Tables.FindAsync(id);
        }

        public async Task<IEnumerable<Table>> GetAllAsync()
        {
            return await _context.Tables.ToListAsync();
        }

        public async Task<IEnumerable<Table>> GetAvailableTablesAsync(DateTime start, int durationHours)
        {
            var end = start.AddHours(durationHours);

            // Найти ID забронированных столов в указанный период
            var reservedTableIds = await _context.Reservations
                .Where(r =>
                    start < r.StartDateTime.AddHours(r.DurationHours) &&
                    end > r.StartDateTime
                )
                .Select(r => r.TableId)
                .ToListAsync();

            // Вернуть свободные столы
            return await _context.Tables
                .Where(t => !reservedTableIds.Contains(t.Id))
                .ToListAsync();
        }
    }
}
