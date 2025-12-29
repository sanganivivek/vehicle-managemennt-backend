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
            var vehicle = new VehicleMaster // Assuming your entity is VehicleMaster or Vehicle
            {
                VehicleId = Guid.NewGuid(),
                RegNo = dto.VehicleName, // Mapping Name to RegNo if that's your schema
                BrandId = dto.BrandId,   // Uses ID only
                ModelId = dto.ModelId,   // Uses ID only
                IsActive = true
            };

            await _vehicleService.AddVehicleAsync(vehicle);
            return Ok(dto);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var vehicles = await _vehicleService.GetAllVehiclesAsync();

            // Map Entity -> DTO
            var dtos = vehicles.Select(v => new VehicleDTO
            {
                VehicleId = v.VehicleId,
                VehicleName = v.RegNo,
                BrandId = v.BrandId,
                ModelId = v.ModelId
            });

            return Ok(dtos);
        }
    }
}