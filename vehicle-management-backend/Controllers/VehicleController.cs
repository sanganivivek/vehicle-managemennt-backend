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
            var vehicle = new VehicleMaster
            {
                VehicleId = Guid.NewGuid(),
                VehicleName = dto.VehicleName,
                BrandId = dto.BrandId,
                ModelId = dto.ModelId,
                IsActive = true
            };

            await _vehicleService.CreateAsync(vehicle);
            dto.VehicleId = vehicle.VehicleId;
            return Ok(dto);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? search, [FromQuery] string? brand, 
            [FromQuery] string? sortBy, [FromQuery] string? sortOrder, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var vehicles = await _vehicleService.GetAllAsync();
            
            // Apply filtering
            if (!string.IsNullOrEmpty(search))
                vehicles = vehicles.Where(v => v.VehicleName.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();

            var totalCount = vehicles.Count();
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            
            // Apply pagination
            vehicles = vehicles.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var dtos = vehicles.Select(v => new VehicleDTO
            {
                VehicleId = v.VehicleId,
                VehicleName = v.VehicleName,
                BrandId = v.BrandId,
                ModelId = v.ModelId
            });

            return Ok(new {
                totalCount,
                page,
                data = dtos,
                totalPages,
                totalRecords = totalCount,
                pageSize
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var vehicle = await _vehicleService.GetByIdAsync(id);
            if (vehicle == null) return NotFound();

            var dto = new VehicleDTO
            {
                VehicleId = vehicle.VehicleId,
                VehicleName = vehicle.VehicleName,
                BrandId = vehicle.BrandId,
                ModelId = vehicle.ModelId
            };
            return Ok(dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, VehicleDTO dto)
        {
            var vehicle = await _vehicleService.GetByIdAsync(id);
            if (vehicle == null) return NotFound();

            vehicle.VehicleName = dto.VehicleName;
            vehicle.BrandId = dto.BrandId;
            vehicle.ModelId = dto.ModelId;

            await _vehicleService.UpdateAsync(vehicle);
            return Ok(dto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var vehicle = await _vehicleService.GetByIdAsync(id);
            if (vehicle == null) return NotFound();

            await _vehicleService.DeleteAsync(id);
            return NoContent();
        }
    }
}