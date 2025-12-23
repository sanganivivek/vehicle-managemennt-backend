using Microsoft.AspNetCore.Mvc;
using vehicle_management_backend.Application.Services.Interfaces;
using vehicle_management_backend.Core.Models;
using vehicle_management_backend.Core.DTOs;

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
            return Ok(await _modelService.GetModelsByBrandAsync(brandId));
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateModelDTO dto)
        {
            var model = new Model
            {
                ModelId = Guid.NewGuid(),   
                ModelName = dto.Name, // Maps 'Name' from DTO to 'ModelName' in DB
                BrandId = dto.BrandId
            };

            await _modelService.CreateAsync(model);
            return Ok();
        }
    }
}