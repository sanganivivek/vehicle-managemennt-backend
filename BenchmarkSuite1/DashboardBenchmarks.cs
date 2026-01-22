using System;
using System.Linq;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using vehicle_management_backend.Infrastructure.Data;
using vehicle_management_backend.Controllers;
using vehicle_management_backend.Core.Models;
using System.Collections.Generic;
using BenchmarkDotNet.Diagnosers;

[MemoryDiagnoser]
public class DashboardBenchmarks
{
    private AppDbContext _context = null !;
    private DashboardController _controller = null !;
    [GlobalSetup]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(databaseName: "BenchmarkDb").Options;
        _context = new AppDbContext(options);
        // Seed vehicles
        var vehicles = new List<VehicleMaster>();
        for (int i = 0; i < 1000; i++)
        {
            vehicles.Add(new VehicleMaster { VehicleId = Guid.NewGuid(), BrandId = Guid.NewGuid(), ModelId = Guid.NewGuid(), VehicleName = "Vehicle " + i, RegNo = "REG" + i, ModelYear = 2000 + (i % 20), IsActive = true, CurrentStatus = i % 3, CreatedAt = DateTime.UtcNow });
        }

        _context.Vehicles.AddRange(vehicles);
        // Seed activity logs
        var logs = new List<ActivityLog>();
        for (int i = 0; i < 1000; i++)
        {
            logs.Add(new ActivityLog { Id = i + 1, Message = "Activity " + i, Type = "info", CreatedAt = DateTime.UtcNow.AddMinutes(-i) });
        }

        _context.ActivityLogs.AddRange(logs);
        _context.SaveChanges();
        _controller = new DashboardController(_context);
    }

    [Benchmark]
    public async Task GetDashboardStats_Benchmark()
    {
        var result = await _controller.GetDashboardStats();
    }

    [Benchmark]
    public async Task GetRecentActivity_Benchmark()
    {
        var result = await _controller.GetRecentActivity();
    }
}