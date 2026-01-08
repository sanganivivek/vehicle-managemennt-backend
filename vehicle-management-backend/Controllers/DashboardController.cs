using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vehicle_management_backend.Infrastructure.Data;
// The namespace now matches the one defined in VehicleStatus.cs
using vehicle_management_backend.Core.Enums;

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

            var result = new
            {
                totalVehicles = vehicles.Count,
                // Cast Enum to int because your DB 'CurrentStatus' is likely an int
                availableVehicles = vehicles.Count(v => v.CurrentStatus == (int)VehicleStatus.Available),
                onRoad = vehicles.Count(v => v.CurrentStatus == (int)VehicleStatus.OnRoad),
                inMaintenance = vehicles.Count(v => v.CurrentStatus == (int)VehicleStatus.Maintenance)
            };

            return Ok(result);
        }
    }
}