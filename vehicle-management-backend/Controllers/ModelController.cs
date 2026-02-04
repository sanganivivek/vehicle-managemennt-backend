using Microsoft.AspNetCore.Mvc;
using vehicle_management_backend.Application.Services.Interfaces;
using vehicle_management_backend.Core.DTOs;
using vehicle_management_backend.Core.Models;
namespace vehicle_management_backend.Controllers
{
    [ApiController]
    [Route("api/models")]
    public class ModelController : ControllerBase
    {
        private readonly IModelService _modelService;
        private readonly IBrandService _brandService;
        
        public ModelController(IModelService modelService, IBrandService brandService)
        {
            _modelService = modelService;
            _brandService = brandService;
        }
        
        [HttpGet("by-brand/{brandId}")]
        public async Task<IActionResult> GetByBrand(Guid brandId)
        {
            var models = await _modelService.GetModelsByBrandAsync(brandId);
            var brands = await _brandService.GetBrandsAsync();
            
            var dtos = models.Select(m =>
            {
                var brand = brands.FirstOrDefault(b => b.BrandId == m.BrandId);
                return new ModelDTO
                {
                    ModelId = m.ModelId,
                    ModelCode = m.ModelCode,
                    ModelName = m.ModelName,

                    Description = m.Description,
                    BrandId = m.BrandId,
                    BrandName = brand?.BrandName,
                    BrandCode = brand?.BrandCode
                };
            });
            return Ok(dtos);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? search, [FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? sortBy = null, [FromQuery] string? sortOrder = "asc")
        {
            try
            {
                var models = await _modelService.GetModelsAsync();
                var brands = await _brandService.GetBrandsAsync();
                
                // 1. Join Models with Brands (Models don't store BrandName directly)
                var dtos = models.Select(m =>
                {
                    var brand = brands.FirstOrDefault(b => b.BrandId == m.BrandId);
                    return new ModelDTO
                    {
                        ModelId = m.ModelId,
                        ModelCode = m.ModelCode,
                        ModelName = m.ModelName,
                        Description = m.Description,
                        BrandId = m.BrandId,
                        BrandName = brand?.BrandName ?? "Unknown",
                        BrandCode = brand?.BrandCode
                    };
                });

                // 2. Filter
                if (!string.IsNullOrEmpty(search))
                {
                    dtos = dtos.Where(m => 
                        (m.ModelName?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false) || 
                        (m.ModelCode?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false) ||
                        (m.BrandName?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false)
                    );
                }

                // 3. Sort
                if (!string.IsNullOrEmpty(sortBy))
                {
                    switch (sortBy.ToLower())
                    {
                        case "brandname":
                            dtos = sortOrder?.ToLower() == "desc" ? dtos.OrderByDescending(m => m.BrandName) : dtos.OrderBy(m => m.BrandName);
                            break;
                        case "modelname":
                            dtos = sortOrder?.ToLower() == "desc" ? dtos.OrderByDescending(m => m.ModelName) : dtos.OrderBy(m => m.ModelName);
                            break;
                        case "modelcode":
                            dtos = sortOrder?.ToLower() == "desc" ? dtos.OrderByDescending(m => m.ModelCode) : dtos.OrderBy(m => m.ModelCode);
                            break;
                        default:
                            dtos = dtos.OrderBy(m => m.ModelName);
                            break;
                    }
                }
                else
                {
                    dtos = dtos.OrderBy(m => m.ModelName); // Default sort
                }

                // 4. Paginate
                var totalCount = dtos.Count();
                var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
                var pagedData = dtos.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                return Ok(new
                {
                    totalCount,
                    page,
                    data = pagedData,
                    totalPages,
                    totalRecords = totalCount,
                    pageSize
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var model = await _modelService.GetByIdAsync(id);
            if (model == null) return NotFound();
            
            var brands = await _brandService.GetBrandsAsync();
            var brand = brands.FirstOrDefault(b => b.BrandId == model.BrandId);
            
            var dto = new ModelDTO
            {
                ModelId = model.ModelId,
                ModelCode = model.ModelCode,
                ModelName = model.ModelName,

                Description = model.Description,
                BrandId = model.BrandId,
                BrandName = brand?.BrandName,
                BrandCode = brand?.BrandCode
            };
            
            return Ok(dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, CreateModelDTO dto)
        {
            await _modelService.UpdateAsync(id, dto);
            return Ok(new { message = "Model updated successfully" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                // Check if any vehicles reference this model
                var vehicleService = HttpContext.RequestServices.GetService<IVehicleService>();
                if (vehicleService != null)
                {
                    var vehicles = await vehicleService.GetAllAsync();
                    var vehiclesUsingModel = vehicles.Where(v => v.ModelId == id).ToList();
                    
                    if (vehiclesUsingModel.Any())
                    {
                        return BadRequest(new { 
                            error = "Cannot Delete Model", 
                            message = $"Cannot delete this model because {vehiclesUsingModel.Count} vehicle(s) are currently using it. Please reassign or delete those vehicles first.",
                            vehicleCount = vehiclesUsingModel.Count
                        });
                    }
                }

                await _modelService.DeleteAsync(id);
                return Ok(new { message = "Model deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message, innerException = ex.InnerException?.Message });
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> Create(CreateModelDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var model = new Model
                {
                    ModelId = Guid.NewGuid(),
                    BrandId = dto.BrandId,
                    ModelCode = dto.ModelCode,
                    ModelName = dto.Name,

                    Description = dto.Description
                };
                await _modelService.CreateAsync(model);

                return Ok(new { message = "Model created successfully", modelId = model.ModelId });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message, innerException = ex.InnerException?.Message });
            }
        }
    }
}