using AutoMapper;
using FluentValidation;
using CafeBooking.DataAccess.Repositories;
using CafeBooking.DataAccess.Entities;
using CafeBooking.BusinessLogic.DTOs;

namespace CafeBooking.BusinessLogic.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly ITableRepository _tableRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<ReservationCreateDto> _validator;

        public ReservationService(
            IReservationRepository reservationRepository,
            ITableRepository tableRepository,
            IMapper mapper,
            IValidator<ReservationCreateDto> validator)
        {
            _reservationRepository = reservationRepository;
            _tableRepository = tableRepository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<ReservationDto> CreateReservationAsync(ReservationCreateDto dto)
        {
            var validationResult = await _validator.ValidateAsync(dto);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var availableTables = await _tableRepository.GetAvailableTablesAsync(
                dto.StartDateTime, dto.DurationHours);

            if (!availableTables.Any(t => t.Id == dto.TableId))
                throw new InvalidOperationException("Стол недоступен в указанное время");

            var reservation = _mapper.Map<Reservation>(dto);
            await _reservationRepository.AddAsync(reservation);

            return _mapper.Map<ReservationDto>(reservation);
        }

        public async Task<bool> DeleteReservationByPhoneAsync(string phoneNumber)
        {
            await _reservationRepository.DeleteByPhoneNumberAsync(phoneNumber);
            return true;
        }

        public async Task<IEnumerable<ReservationDto>> GetAllReservationsAsync()
        {
            var reservations = await _reservationRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ReservationDto>>(reservations);
        }

        public async Task<IEnumerable<ReservationDto>> GetActiveReservationsAsync(DateTime currentTime)
        {
            var activeReservations = await _reservationRepository.GetActiveReservationsAsync(currentTime);
            return _mapper.Map<IEnumerable<ReservationDto>>(activeReservations);
        }

        public async Task DeleteExpiredReservationsAsync(DateTime currentTime)
        {
            await _reservationRepository.DeleteExpiredReservationsAsync(currentTime);
        }
    }
}
