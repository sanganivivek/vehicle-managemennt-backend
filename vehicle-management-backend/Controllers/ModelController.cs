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

        [HttpGet("by-brand/{brandId}")]
        public async Task<IActionResult> GetByBrand(Guid brandId)
        {
            var models = await _modelService.GetModelsByBrandAsync(brandId);

            var dtos = models.Select(m => new ModelDTO
            {
                ModelId = m.ModelId,
                ModelName = m.ModelName,
                BrandId = m.BrandId
            });

            return Ok(dtos);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateModelDTO dto)
        {
            var model = new Model
            {
                ModelId = Guid.NewGuid(),
                BrandId = dto.BrandId,
                ModelName = dto.Name
            };

            await _modelService.CreateAsync(model);
            return Ok(model);
        }
    }
}