using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieReservationSystem.Domain.Constants;
using MovieReservationSystem.Domain.DTOs.DashboardDTOs;
using MovieReservationSystem.Services_Abstraction.Interfaces;

namespace MovieReservationSystem.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Roles.Admin)]
    public class DashboardController(IDashboardService dashboardService) : ControllerBase
    {
        [HttpGet("stats")]
        public async Task<ActionResult<DashboardStatsDto>> GetStats()
        {
            try
            {
                var stats = await dashboardService.GetSystemStatsAsync();
                return Ok(stats);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
