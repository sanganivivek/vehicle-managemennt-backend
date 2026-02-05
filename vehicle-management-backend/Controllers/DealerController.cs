using Microsoft.AspNetCore.Mvc;
using vehicle_management_backend.Application.Services.Interfaces;
using vehicle_management_backend.Core.DTOs;

namespace vehicle_management_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DealerController : ControllerBase
    {
        private readonly IDealerService _dealerService;

        public DealerController(IDealerService dealerService)
        {
            _dealerService = dealerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDealers()
        {
            var dealers = await _dealerService.GetAllDealersAsync();
            return Ok(dealers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDealerById(int id)
        {
            var dealer = await _dealerService.GetDealerByIdAsync(id);
            if (dealer == null) return NotFound();
            return Ok(dealer);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDealer([FromBody] CreateDealerDTO dealerDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var createdDealer = await _dealerService.CreateDealerAsync(dealerDto);
            return CreatedAtAction(nameof(GetDealerById), new { id = createdDealer.Id }, createdDealer);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDealer(int id, [FromBody] UpdateDealerDTO dealerDto)
        {
            if (id != dealerDto.Id) return BadRequest("ID mismatch");

            var updatedDealer = await _dealerService.UpdateDealerAsync(id, dealerDto);
            if (updatedDealer == null) return NotFound();

            return Ok(updatedDealer);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDealer(int id)
        {
            var deleted = await _dealerService.DeleteDealerAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}