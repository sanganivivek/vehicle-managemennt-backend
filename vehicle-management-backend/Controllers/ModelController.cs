using Microsoft.AspNetCore.Mvc;
using vehicle_management_backend.Application.Services.Interfaces;
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
        public async Task<IActionResult> GetByBrand(int brandId)
        {
            return Ok(await _modelService.GetModelsByBrandAsync(brandId));
        }

        [HttpPost]
        public async Task<IActionResult> Create(Model model)
        {
            await _modelService.CreateAsync(model);
            return Ok();
        }
    }
}
