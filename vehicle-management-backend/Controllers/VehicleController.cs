using Microsoft.AspNetCore.Mvc;
using vehicle_management_backend.Application.Services.Interfaces;
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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _vehicleService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _vehicleService.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Create(VehicleMaster vehicle)
        {
            await _vehicleService.CreateAsync(vehicle);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update(VehicleMaster vehicle)
        {
            await _vehicleService.UpdateAsync(vehicle);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _vehicleService.DeleteAsync(id);
            return Ok();
        }
    }
}
