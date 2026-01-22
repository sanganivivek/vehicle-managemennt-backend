using Moq;
using Xunit;
using vehicle_management_backend.Application.Services.Implementations;
using vehicle_management_backend.Infrastructure.Repositories.Interfaces;
using vehicle_management_backend.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

public class VehicleServiceTests
{
    // 1. Define Mocks and the Service
    private readonly Mock<IVehicleRepository> _mockRepo;
    private readonly VehicleService _vehicleService;

    public VehicleServiceTests()
    {
        // 2. Initialize the Mock
        _mockRepo = new Mock<IVehicleRepository>();

        // 3. Inject the Mock object into the Service
        _vehicleService = new VehicleService(_mockRepo.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnListOfVehicles()
    {
        // --- ARRANGE (Setup data) ---
        var dummyVehicles = new List<VehicleMaster>
        {
            new VehicleMaster { VehicleId = Guid.NewGuid(), VehicleName = "Car A", RegNo = "GJ01AB0123" },
            new VehicleMaster { VehicleId = Guid.NewGuid(), VehicleName = "Car B", RegNo = "GJ01AB1234" }
        };

        // Tell the mock: "When GetAllAsync is called, return this list"
        _mockRepo.Setup(repo => repo.GetAllAsync())
                 .ReturnsAsync(dummyVehicles);

        // --- ACT (Call the method) ---
        var result = await _vehicleService.GetAllAsync();

        // --- ASSERT (Verify results) ---
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal("Car A", result[0].VehicleName);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnVehicle_WhenExists()
    {
        // --- ARRANGE ---
        var vehicleId = Guid.NewGuid();
        var dummyVehicle = new VehicleMaster { VehicleId = vehicleId, VehicleName = "Test Car" };

        _mockRepo.Setup(repo => repo.GetByIdAsync(vehicleId))
                 .ReturnsAsync(dummyVehicle);

        // --- ACT ---
        var result = await _vehicleService.GetByIdAsync(vehicleId);

        // --- ASSERT ---
        Assert.NotNull(result);
        Assert.Equal(vehicleId, result.VehicleId);
        Assert.Equal("Test Car", result.VehicleName);
    }

    [Fact]
    public async Task CreateAsync_ShouldCallAddAsyncInRepository()
    {
        // --- ARRANGE ---
        var newVehicle = new VehicleMaster { VehicleId = Guid.NewGuid(), VehicleName = "New Car" };

        // --- ACT ---
        await _vehicleService.CreateAsync(newVehicle);

        // --- ASSERT ---
        // Verify that the repository's AddAsync method was called exactly once
        _mockRepo.Verify(repo => repo.AddAsync(newVehicle), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldCallDeleteAsyncInRepository()
    {
        // --- ARRANGE ---
        var vehicleId = Guid.NewGuid();

        // --- ACT ---
        await _vehicleService.DeleteAsync(vehicleId);

        // --- ASSERT ---
        _mockRepo.Verify(repo => repo.DeleteAsync(vehicleId), Times.Once);
    }
}