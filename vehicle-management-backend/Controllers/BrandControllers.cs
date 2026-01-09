using Microsoft.AspNetCore.Mvc;
using vehicle_management_backend.Application.Services.Interfaces;
using vehicle_management_backend.Core.DTOs;
namespace vehicle_management_backend.Controllers
{
    [ApiController]
    [Route("api/brands")]
    public class BrandControllers : ControllerBase
    {
        private readonly IBrandService _brandService;
        public BrandControllers(IBrandService brandService)
        {
            _brandService = brandService;
        }
        [HttpPost]
        public async Task<IActionResult> Create(BrandDTO dto)
        {
            await _brandService.AddBrandAsync(dto);
            return Ok();
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _brandService.GetBrandsAsync());
        }
    }
}