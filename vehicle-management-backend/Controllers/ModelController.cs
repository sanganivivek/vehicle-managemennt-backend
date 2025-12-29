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

        public ModelController(IModelService modelService)
        {
            _modelService = modelService;
        }

        // GET: Returns clean DTOs (No nested loops)
        [HttpGet("by-brand/{brandId}")]
        public async Task<IActionResult> GetByBrand(Guid brandId)
        {
            var models = await _modelService.GetModelsByBrandAsync(brandId);

            // Map Entity -> DTO
            var dtos = models.Select(m => new ModelDTO
            {
                ModelId = m.ModelId,
                ModelName = m.ModelName,
                BrandId = m.BrandId
            });

            return Ok(dtos);
        }

        // POST: Accepts clean DTO
        [HttpPost]
        public async Task<IActionResult> Create(ModelDTO dto)
        {
            // Map DTO -> Entity
            var model = new Model
            {
                ModelId = Guid.NewGuid(),
                ModelName = dto.ModelName,
                BrandId = dto.BrandId
            };

            await _modelService.CreateAsync(model);

            // Return the DTO back
            dto.ModelId = model.ModelId;
            return Ok(dto);
        }
    }
}