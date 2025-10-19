using CafeBooking.DataAccess.Entities;

namespace CafeBooking.DataAccess.Repositories
{
    public interface IReservationRepository
    {
        Task<Reservation> GetByIdAsync(int id);
        Task<IEnumerable<Reservation>> GetAllAsync();
        Task AddAsync(Reservation reservation);
        Task DeleteAsync(Reservation reservation);
        Task DeleteByPhoneNumberAsync(string phoneNumber);
        Task<IEnumerable<Reservation>> GetActiveReservationsAsync(DateTime currentTime);
        Task DeleteExpiredReservationsAsync(DateTime currentTime);
    }
}
