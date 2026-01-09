using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vehicle_management_backend.Infrastructure.Data;
using vehicle_management_backend.Core.Enums;
using vehicle_management_backend.Core.DTOs;
namespace vehicle_management_backend.Controllers
{
    [ApiController]
    [Route("api/dashboard")]
    public class DashboardController : ControllerBase
    {
        private readonly AppDbContext _context;
        public DashboardController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet("stats")]
        public async Task<IActionResult> GetDashboardStats()
        {
            var vehicles = await _context.Vehicles.ToListAsync();
            var result = new DashboardStatsDto
            {
                TotalVehicles = vehicles.Count,
                AvailableVehicles = vehicles.Count(v => v.CurrentStatus == (int)VehicleStatus.Available),
                OnRoad = vehicles.Count(v => v.CurrentStatus == (int)VehicleStatus.OnRoad),
                InMaintenance = vehicles.Count(v => v.CurrentStatus == (int)VehicleStatus.Maintenance)
            };
            return Ok(result);
        }
    }
}