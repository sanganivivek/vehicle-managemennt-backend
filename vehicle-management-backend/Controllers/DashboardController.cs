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

        // =========================
        // Recent Activity
        // GET: /api/dashboard/activity
        // =========================
        [HttpGet("activity")]
        public async Task<IActionResult> GetRecentActivity()
        {
            var recentActivities = await _context.ActivityLogs
                .OrderByDescending(v => v.CreatedAt)
                .Take(5)
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
