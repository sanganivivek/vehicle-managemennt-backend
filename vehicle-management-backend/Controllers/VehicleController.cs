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
                VehicleId = Guid.NewGuid(),
                VehicleName = dto.VehicleName, // Fix: Use VehicleName (not RegNo)
                BrandId = dto.BrandId,
                ModelId = dto.ModelId,
                IsActive = true
            };

            // Fix: Call CreateAsync (AddVehicleAsync does not exist)
            await _vehicleService.CreateAsync(vehicle);

            // Return updated DTO
            dto.VehicleId = vehicle.VehicleId;
            return Ok(dto);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            // Fix: Call GetAllAsync (GetAllVehiclesAsync does not exist)
            var vehicles = await _vehicleService.GetAllAsync();

            // Map Entity -> DTO
            var dtos = vehicles.Select(v => new VehicleDTO
            {
                VehicleId = v.VehicleId,
                VehicleName = v.VehicleName, // Fix: Use VehicleName
                BrandId = v.BrandId,
                ModelId = v.ModelId
            });

            return Ok(dtos);
        }
    }
}