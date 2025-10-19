using CafeBooking.BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;

namespace CafeBooking.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TablesController : ControllerBase
    {
        private readonly ITableService _tableService;

        public TablesController(ITableService tableService)
        {
            _tableService = tableService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTables()
        {
            var tables = await _tableService.GetAllTablesAsync();
            return Ok(tables);
        }

        [HttpGet("available")]
        public async Task<IActionResult> GetAvailableTables(
            [FromQuery] DateTime start,
            [FromQuery] int durationHours)
        {
            if (durationHours < 1 || durationHours > 24)
                return BadRequest("Длительность бронирования должна быть от 1 до 24 часов");

            var tables = await _tableService.GetAvailableTablesAsync(start, durationHours);
            return Ok(tables);
        }
    }
}
