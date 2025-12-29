using Microsoft.AspNetCore.Mvc;
using vehicle_management_backend.Application.Services.Interfaces;
using vehicle_management_backend.Core.DTOs;
using vehicle_management_backend.Core.Models;

namespace vehicle_management_backend.Controllers
{
    [ApiController]
    [Route("api/vehicles")]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;

        public VehicleController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(VehicleDTO dto)
        {
            // Map DTO -> Entity
            var vehicle = new VehicleMaster
            {
                VehicleId = Guid.NewGuid(), // Matches Model now
                VehicleName = dto.VehicleName, // Matches Model now (was RegNo)
                BrandId = dto.BrandId,
                ModelId = dto.ModelId,
                IsActive = true
            };

            // Fix: Call CreateAsync (not AddVehicleAsync)
            await _vehicleService.CreateAsync(vehicle);

            // Return DTO with the new ID
            dto.VehicleId = vehicle.VehicleId;
            return Ok(dto);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            // Fix: Call GetAllAsync (not GetAllVehiclesAsync)
            var vehicles = await _vehicleService.GetAllAsync();

            // Map Entity -> DTO
            var dtos = vehicles.Select(v => new VehicleDTO
            {
                VehicleId = v.VehicleId,
                VehicleName = v.VehicleName, // Was RegNo
                BrandId = v.BrandId,
                ModelId = v.ModelId
            });

            return Ok(dtos);
        }
    }
}