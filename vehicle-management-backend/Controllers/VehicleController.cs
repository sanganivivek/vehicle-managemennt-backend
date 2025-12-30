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
        private readonly IBrandService _brandService;
        private readonly IModelService _modelService;

        public VehicleController(IVehicleService vehicleService, IBrandService brandService, IModelService modelService)
        {
            _vehicleService = vehicleService;
            _brandService = brandService;
            _modelService = modelService;
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
                    IsActive = dto.IsActive
                };

                await _vehicleService.CreateAsync(vehicle);
                return Ok(new { vehicleId = vehicle.VehicleId, message = "Vehicle saved successfully" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, new { error = ex.Message, innerException = ex.InnerException?.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? search, [FromQuery] string? brand,
            [FromQuery] string? sortBy, [FromQuery] string? sortOrder, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var vehicles = await _vehicleService.GetAllAsync();
                var brands = await _brandService.GetBrandsAsync();
                var models = await _modelService.GetModelsAsync();

                // Handle null or empty vehicles list
                if (vehicles == null)
                {
                    vehicles = new List<VehicleMaster>();
                }

                // Apply filtering
                if (!string.IsNullOrEmpty(search))
                    vehicles = vehicles.Where(v => v.VehicleName.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                                                  v.RegNo.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();

                if (!string.IsNullOrEmpty(brand))
                {
                    var brandId = brands.FirstOrDefault(b => b.BrandName.Equals(brand, StringComparison.OrdinalIgnoreCase))?.BrandId;
                    if (brandId.HasValue)
                        vehicles = vehicles.Where(v => v.BrandId == brandId.Value).ToList();
                }

                // Apply sorting
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

                // Apply pagination
                vehicles = vehicles.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                var dtos = vehicles.Select(v => {
                    var vehicleBrand = brands.FirstOrDefault(b => b.BrandId == v.BrandId);
                    var vehicleModel = models.FirstOrDefault(m => m.ModelId == v.ModelId);

                    return new VehicleDTO
                    {
                        VehicleId = v.VehicleId,
                        VehicleName = v.VehicleName,
                        RegNo = v.RegNo,
                        BrandId = v.BrandId,
                        ModelId = v.ModelId,
                        Brand = vehicleBrand?.BrandName ?? "Unknown",
                        Model = vehicleModel?.ModelName ?? "Unknown",
                        BrandName = vehicleBrand?.BrandName ?? "Unknown",
                        ModelName = vehicleModel?.ModelName ?? "Unknown",
                        ModelYear = v.ModelYear,
                        IsActive = v.IsActive
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

                // Try to parse as Guid first
                if (Guid.TryParse(id, out Guid vehicleId))
                {
                    vehicle = await _vehicleService.GetByIdAsync(vehicleId);
                }
                else
                {
                    // If not a Guid, treat as RegNo
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
                    IsActive = vehicle.IsActive
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
        public async Task<IActionResult> Update(string id, VehicleDTO dto)
        {
            try
            {
                VehicleMaster? vehicle = null;

                // Try to parse as Guid first
                if (Guid.TryParse(id, out Guid vehicleId))
                {
                    vehicle = await _vehicleService.GetByIdAsync(vehicleId);
                }
                else
                {
                    // If not a Guid, treat as RegNo
                    var vehicles = await _vehicleService.GetAllAsync();
                    vehicle = vehicles.FirstOrDefault(v => v.RegNo == id);
                }

                if (vehicle == null) return NotFound();

                vehicle.VehicleName = dto.VehicleName;
                vehicle.BrandId = dto.BrandId;
                vehicle.ModelId = dto.ModelId;
                vehicle.ModelYear = dto.ModelYear;
                vehicle.IsActive = dto.IsActive;

                await _vehicleService.UpdateAsync(vehicle);
                return Ok(dto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Update: {ex.Message}");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                // Try to parse as Guid first
                if (Guid.TryParse(id, out Guid vehicleId))
                {
                    var vehicle = await _vehicleService.GetByIdAsync(vehicleId);
                    if (vehicle == null) return NotFound();

                    await _vehicleService.DeleteAsync(vehicleId);
                    return NoContent();
                }
                else
                {
                    // If not a Guid, treat as RegNo
                    var vehicles = await _vehicleService.GetAllAsync();
                    var vehicle = vehicles.FirstOrDefault(v => v.RegNo == id);
                    if (vehicle == null) return NotFound();

                    await _vehicleService.DeleteAsync(vehicle.VehicleId);
                    return NoContent();
                }
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