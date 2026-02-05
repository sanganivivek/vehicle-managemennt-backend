using vehicle_management_backend.Core.DTOs;
using vehicle_management_backend.Core.Models;

namespace vehicle_management_backend.Application.Services.Interfaces
{
    public interface IDealerService
    {
        Task<IEnumerable<Dealer>> GetAllDealersAsync();
        Task<Dealer> GetDealerByIdAsync(int id);
        Task<Dealer> CreateDealerAsync(CreateDealerDTO dealerDto);
        Task<Dealer> UpdateDealerAsync(int id, UpdateDealerDTO dealerDto);
        Task<bool> DeleteDealerAsync(int id);
    }
}