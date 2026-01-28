using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using vehicle_management_backend.Application.Services.Interfaces;
using vehicle_management_backend.Core.DTOs;
using vehicle_management_backend.Core.Models;
using vehicle_management_backend.Infrastructure.Data;
namespace vehicle_management_backend.Controllers
{
    [ApiController]
    [Route("api/vehicles")]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;
        private readonly IBrandService _brandService;
        private readonly IModelService _modelService;
        private readonly AppDbContext _context;
        public VehicleController(IVehicleService vehicleService, IBrandService brandService, IModelService modelService, AppDbContext context)
        {
            _vehicleService = vehicleService;
            _brandService = brandService;
            _modelService = modelService;
            _context = context;
        }
        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok(new { message = "API is working", timestamp = DateTime.Now });
        }
        [HttpPost("simple")]
        public IActionResult CreateSimple()
        {
            return Ok(new { message = "Simple endpoint works" });
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateVehicleWithoutNameDTO dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest("Request body is null");
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var vehicle = new VehicleMaster
                {
                    VehicleId = Guid.NewGuid(),
                    VehicleName = $"Vehicle-{DateTime.Now:yyyyMMddHHmmss}",
                    RegNo = dto.RegNo ?? string.Empty,
                    BrandId = dto.BrandId,
                    ModelId = dto.ModelId,
                    ModelYear = dto.ModelYear,
                    IsActive = dto.IsActive,
                    CurrentStatus = dto.CurrentStatus
                };
                await _vehicleService.CreateAsync(vehicle);

                _context.ActivityLogs.Add(new ActivityLog
                {
                    Message = $"New vehicle registered: {vehicle.RegNo}",
                    Type = "success",
                    CreatedAt = DateTime.UtcNow
                });
                await _context.SaveChangesAsync();

                return Ok(new { vehicleId = vehicle.VehicleId, message = "Vehicle saved successfully" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, new { error = ex.Message, innerException = ex.InnerException?.Message });
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? search, [FromQuery] string? brand, [FromQuery] int? status,
            [FromQuery] string? sortBy, [FromQuery] string? sortOrder, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var vehicles = await _vehicleService.GetAllAsync();
                var brands = await _brandService.GetBrandsAsync();
                var models = await _modelService.GetModelsAsync();
                if (vehicles == null)
                {
                    vehicles = new List<VehicleMaster>();
                }
                if (status.HasValue)
                {
                    vehicles = vehicles.Where(v => v.CurrentStatus == status.Value).ToList();
                }
                if (!string.IsNullOrEmpty(search))
                    vehicles = vehicles.Where(v => v.VehicleName.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                                                  v.RegNo.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
                if (!string.IsNullOrEmpty(brand))
                {
                    var brandId = brands.FirstOrDefault(b => b.BrandName.Equals(brand, StringComparison.OrdinalIgnoreCase))?.BrandId;
                    if (brandId.HasValue)
                        vehicles = vehicles.Where(v => v.BrandId == brandId.Value).ToList();
                }
                if (!string.IsNullOrEmpty(sortBy))
                {
                    switch (sortBy.ToLower())
                    {
                        case "regno":
                            vehicles = sortOrder?.ToLower() == "desc" ?
                                vehicles.OrderByDescending(v => v.RegNo).ToList() :
                                vehicles.OrderBy(v => v.RegNo).ToList();
                            break;
                        case "brand":
                            vehicles = sortOrder?.ToLower() == "desc" ?
                                vehicles.OrderByDescending(v => brands.FirstOrDefault(b => b.BrandId == v.BrandId)?.BrandName ?? "").ToList() :
                                vehicles.OrderBy(v => brands.FirstOrDefault(b => b.BrandId == v.BrandId)?.BrandName ?? "").ToList();
                            break;
                        case "model":
                            vehicles = sortOrder?.ToLower() == "desc" ?
                                vehicles.OrderByDescending(v => models.FirstOrDefault(m => m.ModelId == v.ModelId)?.ModelName ?? "").ToList() :
                                vehicles.OrderBy(v => models.FirstOrDefault(m => m.ModelId == v.ModelId)?.ModelName ?? "").ToList();
                            break;
                        default:
                            vehicles = sortOrder?.ToLower() == "desc" ?
                                vehicles.OrderByDescending(v => v.VehicleName).ToList() :
                                vehicles.OrderBy(v => v.VehicleName).ToList();
                            break;
                    }
                }
                var totalCount = vehicles.Count();
                var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
                vehicles = vehicles.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                var dtos = vehicles.Select(v =>
                {
                    var vehicleBrand = brands.FirstOrDefault(b => b.BrandId == v.BrandId);
                    var vehicleModel = models.FirstOrDefault(m => m.ModelId == v.ModelId);
                    return new VehicleDTO
                    {
                        VehicleId = v.VehicleId,
                        VehicleName = v.VehicleName,
                        RegNo = v.RegNo,
                        BrandCode = v.BrandId,
                        ModelId = v.ModelId,
                        Brand = vehicleBrand?.BrandName ?? "Unknown",
                        Model = vehicleModel?.ModelName ?? "Unknown",
                        BrandName = vehicleBrand?.BrandName ?? "Unknown",
                        ModelName = vehicleModel?.ModelName ?? "Unknown",
                        ModelYear = v.ModelYear,
                        IsActive = v.IsActive,
                        CurrentStatus = v.CurrentStatus
                    };
                }).ToList();
                return Ok(new
                {
                    totalCount,
                    page,
                    data = dtos,
                    totalPages,
                    totalRecords = totalCount,
                    pageSize
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAll: {ex.Message}");
                return Ok(new
                {
                    totalCount = 0,
                    page = 1,
                    data = new List<VehicleDTO>(),
                    totalPages = 1,
                    totalRecords = 0,
                    pageSize = 10
                });
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                VehicleMaster? vehicle = null;
                if (Guid.TryParse(id, out Guid vehicleId))
                {
                    vehicle = await _vehicleService.GetByIdAsync(vehicleId);
                }
                else
                {
                    var vehicles = await _vehicleService.GetAllAsync();
                    vehicle = vehicles.FirstOrDefault(v => v.RegNo == id);
                }
                if (vehicle == null) return NotFound();
                var brands = await _brandService.GetBrandsAsync();
                var models = await _modelService.GetModelsAsync();
                var vehicleBrand = brands.FirstOrDefault(b => b.BrandId == vehicle.BrandId);
                var vehicleModel = models.FirstOrDefault(m => m.ModelId == vehicle.ModelId);
                var dto = new VehicleDTO
                {
                    VehicleId = vehicle.VehicleId,
                    VehicleName = vehicle.VehicleName,
                    RegNo = vehicle.RegNo,
                    BrandId = vehicle.BrandId,
                    ModelId = vehicle.ModelId,
                    Brand = vehicleBrand?.BrandName ?? "Unknown",
                    Model = vehicleModel?.ModelName ?? "Unknown",
                    BrandName = vehicleBrand?.BrandName ?? "Unknown",
                    ModelName = vehicleModel?.ModelName ?? "Unknown",
                    ModelYear = vehicle.ModelYear,
                    IsActive = vehicle.IsActive,
                    CurrentStatus = vehicle.CurrentStatus
                };
                return Ok(dto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetById: {ex.Message}");
                return StatusCode(500, new { error = ex.Message });
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateVehicleDTO dto)
        {
            try
            {
                Console.WriteLine($"Update request received for ID: {id}");
                Console.WriteLine($"DTO: {System.Text.Json.JsonSerializer.Serialize(dto)}");
                VehicleMaster? vehicle = null;
                if (Guid.TryParse(id, out Guid vehicleId))
                {
                    vehicle = await _vehicleService.GetByIdAsync(vehicleId);
                }
                else
                {
                    var vehicles = await _vehicleService.GetAllAsync();
                    vehicle = vehicles.FirstOrDefault(v => v.RegNo == id);
                }
                if (vehicle == null)
                {
                    Console.WriteLine($"Vehicle not found for ID: {id}");
                    return NotFound(new { message = "Vehicle not found" });
                }
                vehicle.RegNo = dto.RegNo;
                vehicle.ModelYear = dto.ModelYear;
                vehicle.IsActive = dto.IsActive;
                vehicle.BrandId = dto.BrandId;
                vehicle.ModelId = dto.ModelId;
                vehicle.CurrentStatus = dto.CurrentStatus; 
                await _vehicleService.UpdateAsync(vehicle);
                _context.ActivityLogs.Add(new ActivityLog
                {
                    Message = $"Vehicle updated: {vehicle.RegNo}",
                    Type = "info", // Blue color in frontend
                    CreatedAt = DateTime.UtcNow
                });
                await _context.SaveChangesAsync();
                Console.WriteLine($"Vehicle updated successfully: {vehicle.VehicleId}");
                return Ok(new { message = "Vehicle updated successfully", vehicleId = vehicle.VehicleId });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Update: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return StatusCode(500, new { error = ex.Message, details = ex.InnerException?.Message });
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                // 1. Define variables to hold data for logging
                string regNo = "Unknown";
                Guid vId = Guid.Empty;

                // 2. Find the vehicle first to get its RegNo (before deleting it)
                if (Guid.TryParse(id, out Guid vehicleId))
                {
                    var vehicle = await _vehicleService.GetByIdAsync(vehicleId);
                    if (vehicle == null) return NotFound();

                    regNo = vehicle.RegNo; // Capture RegNo
                    vId = vehicleId;
                }
                else
                {
                    var vehicles = await _vehicleService.GetAllAsync();
                    var vehicle = vehicles.FirstOrDefault(v => v.RegNo == id);
                    if (vehicle == null) return NotFound();

                    regNo = vehicle.RegNo; // Capture RegNo
                    vId = vehicle.VehicleId;
                }

                // 3. Perform the Delete
                await _vehicleService.DeleteAsync(vId);

                // 4. Log the Activity (BEFORE returning)
                _context.ActivityLogs.Add(new ActivityLog
                {
                    Message = $"Vehicle deleted: {regNo}",
                    Type = "warning",
                    CreatedAt = DateTime.UtcNow
                });
                await _context.SaveChangesAsync();

                // 5. Return success response
                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Delete: {ex.Message}");
                return StatusCode(500, new { error = ex.Message });
            }
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
                    .Select(v => new
                    {
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