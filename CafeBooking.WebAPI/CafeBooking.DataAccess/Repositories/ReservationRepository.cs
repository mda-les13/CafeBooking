using CafeBooking.DataAccess.Context;
using CafeBooking.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace CafeBooking.DataAccess.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly CafeDbContext _context;

        public ReservationRepository(CafeDbContext context)
        {
            _context = context;
        }

        public async Task<Reservation> GetByIdAsync(int id)
        {
            return await _context.Reservations.FindAsync(id);
        }

        public async Task<IEnumerable<Reservation>> GetAllAsync()
        {
            return await _context.Reservations.ToListAsync();
        }

        public async Task AddAsync(Reservation reservation)
        {
            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Reservation reservation)
        {
            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteByPhoneNumberAsync(string phoneNumber)
        {
            var reservations = await _context.Reservations
                .Where(r => r.PhoneNumber == phoneNumber)
                .ToListAsync();

            _context.Reservations.RemoveRange(reservations);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Reservation>> GetActiveReservationsAsync(DateTime currentTime)
        {
            return await _context.Reservations
                .Where(r => r.EndDateTime > currentTime)
                .ToListAsync();
        }

        public async Task DeleteExpiredReservationsAsync(DateTime currentTime)
        {
            var expiredReservations = await _context.Reservations
                .Where(r => r.EndDateTime <= currentTime)
                .ToListAsync();

            _context.Reservations.RemoveRange(expiredReservations);
            await _context.SaveChangesAsync();
        }
    }
}
