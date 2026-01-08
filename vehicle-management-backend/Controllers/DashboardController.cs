using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vehicle_management_backend.Infrastructure.Data;

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
            availableVehicles = vehicles.Count(v => v.CurrentStatus == 0),
            activeVehicles = vehicles.Count(v => v.CurrentStatus == 1),
            inMaintenance = vehicles.Count(v => v.CurrentStatus == 2)
        };

        return Ok(result);
    }
}
