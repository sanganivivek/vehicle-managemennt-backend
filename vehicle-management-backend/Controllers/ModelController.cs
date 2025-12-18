using Microsoft.AspNetCore.Mvc;
using vehicle_management_backend.Application.Services.Interfaces;
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

        // GET: api/models/{brandId}
        [HttpGet("{brandId}")]
        public async Task<IActionResult> GetByBrand(Guid brandId)
        {
            var result = await _modelService.GetModelsByBrandAsync(brandId);
            return Ok(result);
        }

        // POST: api/models
        [HttpPost]
        public async Task<IActionResult> Create(CreateModelDTO dto)
        {
            await _modelService.CreateAsync(dto);
            return Ok();
        }
    }
}
