using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserAuthenticationAPI.Services.StatisticsService;

namespace UserAuthenticationAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class StatisticsController : Controller
    {
        private readonly IStatisticsService _statisticsService;

        public StatisticsController(IStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }

        [HttpGet("GetRegistrations")]
        public async Task<IActionResult> GetRegistrations(DateTime startDate, DateTime endDate)
        {
            var response = await _statisticsService.GetRegistrations(startDate, endDate);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet("NotRegisteredWithinOneDay")]
        public async Task<IActionResult> NotRegisteredWithinOneDay()
        {
            var response = await _statisticsService.NotRegisteredWithinOneDay();
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet("GetOnlineUsers")]
        public async Task<IActionResult> GetOnlineUsers()
        {
            var response = await _statisticsService.GetOnlineUsers();
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet("GetAverageLoginTime")]
        public async Task<IActionResult> GetAverageLoginTime(DateTime day)
        {
            var response = await _statisticsService.GetAverageLoginTime(day);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
