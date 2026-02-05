using vehicle_management_backend.Core.Models;

namespace vehicle_management_backend.Infrastructure.Repositories.Interfaces
{
    public interface IDealerRepository
    {
        Task<IEnumerable<Dealer>> GetAllDealersAsync();
        Task<Dealer> GetDealerByIdAsync(int id);
        Task<Dealer> AddDealerAsync(Dealer dealer);
        Task<Dealer> UpdateDealerAsync(Dealer dealer);
        Task<bool> DeleteDealerAsync(int id);
    }
}