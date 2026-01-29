using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vehicle_management_backend.Infrastructure.Data;
using vehicle_management_backend.Core.Enums;
using vehicle_management_backend.Core.DTOs;
using vehicle_management_backend.Core.Models;

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

        // =========================
        // Dashboard Stats
        // GET: /api/dashboard/stats
        // =========================
        [HttpGet("stats")]
        public async Task<IActionResult> GetDashboardStats()
        {
            // Perform aggregation on the database side to avoid loading all rows into memory
            var stats = await _context.Vehicles
                .GroupBy(v => 1)
                .Select(g => new
                {
                    Total = g.Count(),
                    Available = g.Count(v => v.CurrentStatus == (int)VehicleStatus.Available),
                    Rented = g.Count(v => v.CurrentStatus == (int)VehicleStatus.Rented),
                    Inmaintance = g.Count(v => v.CurrentStatus == (int)VehicleStatus.Inmaintance)
                })
                .FirstOrDefaultAsync();

            var result = new DashboardStatsDto
            {
                TotalVehicles = stats?.Total ?? 0,
                AvailableVehicles = stats?.Available ?? 0,
                Rented = stats?.Rented ?? 0,
                Inmaintance = stats?.Inmaintance ?? 0
            };

            return Ok(result);
        }

        // =========================
        // Recent Activity
        // GET: /api/dashboard/activity
        // =========================
        [HttpGet("activity")]
        public async Task<IActionResult> GetRecentActivity()
        {
            // Project only required fields, use AsNoTracking for read-only query
            var recentActivities = await _context.ActivityLogs
                .AsNoTracking()
                .OrderByDescending(v => v.CreatedAt)
                .Take(5)
                .Select(a => new { a.Id, a.Message, a.CreatedAt, a.Type })
                .ToListAsync();

            var result = recentActivities.Select(a => new
            {
                id = a.Id,
                message = a.Message,
                time = GetTimeAgo(a.CreatedAt),
                type = a.Type
            });

            return Ok(result);
        }

        // =========================
        // Helper Method
        // =========================
        private string GetTimeAgo(DateTime date)
        {
            var span = DateTime.UtcNow - date;

            if (span.TotalMinutes < 1)
                return "Just now";
            if (span.TotalMinutes < 60)
                return $"{span.Minutes} mins ago";
            if (span.TotalHours < 24)
                return $"{span.Hours} hours ago";

            return $"{span.Days} days ago";
        }
    }
}
