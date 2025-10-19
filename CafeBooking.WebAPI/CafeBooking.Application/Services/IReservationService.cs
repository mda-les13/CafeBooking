using CafeBooking.BusinessLogic.DTOs;

namespace CafeBooking.BusinessLogic.Services
{
    public interface IReservationService
    {
        Task<ReservationDto> CreateReservationAsync(ReservationCreateDto dto);
        Task<bool> DeleteReservationByPhoneAsync(string phoneNumber);
        Task<IEnumerable<ReservationDto>> GetAllReservationsAsync();
        Task<IEnumerable<ReservationDto>> GetActiveReservationsAsync(DateTime currentTime);
        Task DeleteExpiredReservationsAsync(DateTime currentTime);
    }
}
