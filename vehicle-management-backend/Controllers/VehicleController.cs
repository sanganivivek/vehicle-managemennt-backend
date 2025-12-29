using Microsoft.AspNetCore.Mvc;
using vehicle_management_backend.Core.DTOs;

[HttpGet]
public async Task<IActionResult> GetAll()
{
    // FIX: Call 'GetAllAsync' (not GetAllVehiclesAsync) and pass no arguments
    var vehicles = await _vehicleService.GetAllAsync();

    var dtos = vehicles.Select(v => new VehicleDTO
    {
        VehicleId = v.VehicleId,
        VehicleName = v.VehicleName,
        BrandId = v.BrandId,
        ModelId = v.ModelId
    });

    return Ok(dtos);
}