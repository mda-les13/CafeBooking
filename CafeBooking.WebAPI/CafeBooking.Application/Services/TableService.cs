using AutoMapper;
using CafeBooking.DataAccess.Repositories;
using CafeBooking.BusinessLogic.DTOs;

namespace CafeBooking.BusinessLogic.Services
{
    public class TableService : ITableService
    {
        private readonly ITableRepository _tableRepository;
        private readonly IMapper _mapper;

        public TableService(ITableRepository tableRepository, IMapper mapper)
        {
            _tableRepository = tableRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TableDto>> GetAllTablesAsync()
        {
            var tables = await _tableRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<TableDto>>(tables);
        }

        public async Task<IEnumerable<TableDto>> GetAvailableTablesAsync(DateTime start, int durationHours)
        {
            var tables = await _tableRepository.GetAvailableTablesAsync(start, durationHours);
            return _mapper.Map<IEnumerable<TableDto>>(tables);
        }
    }
}
