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
        public async Task<IActionResult> Create(CreateVehicleWithoutNameDTO dto)
        {
            var vehicle = new VehicleMaster
            {
                VehicleId = Guid.NewGuid(),
                VehicleName = $"Vehicle-{DateTime.Now:yyyyMMddHHmmss}", // Auto-generate name
                RegNo = dto.RegNo,
                BrandId = dto.BrandId,
                ModelId = dto.ModelId,
                ModelYear = dto.ModelYear,
                IsActive = dto.IsActive
            };

            await _vehicleService.CreateAsync(vehicle);
            return Ok(new { vehicleId = vehicle.VehicleId, message = "Vehicle saved successfully" });
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
                RegNo = v.RegNo,
                BrandId = v.BrandId,
                ModelId = v.ModelId,
                ModelYear = v.ModelYear,
                IsActive = v.IsActive
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
                RegNo = vehicle.RegNo,
                BrandId = vehicle.BrandId,
                ModelId = vehicle.ModelId,
                ModelYear = vehicle.ModelYear,
                IsActive = vehicle.IsActive
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

        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboardData()
        {
            var vehicles = await _vehicleService.GetAllAsync();
            
            var dashboardData = new
            {
                totalVehicles = vehicles.Count,
                activeVehicles = vehicles.Count(v => v.IsActive),
                recentVehicles = vehicles.OrderByDescending(v => v.VehicleId)
                    .Take(5)
                    .Select(v => new {
                        vehicleId = v.VehicleId,
                        vehicleName = v.VehicleName,
                        regNo = v.RegNo,
                        brandId = v.BrandId,
                        modelId = v.ModelId,
                        modelYear = v.ModelYear,
                        isActive = v.IsActive
                    })
            };

            return Ok(dashboardData);
        }
    }
}