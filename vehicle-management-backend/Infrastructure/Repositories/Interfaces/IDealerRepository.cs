using vehicle_management_backend.Core.Models;

namespace vehicle_management_backend.Infrastructure.Repositories.Interfaces
{
    public interface IDealerRepository
    {
        Task<IEnumerable<Dealer>> GetAllAsync();
        Task<Dealer> GetByIdAsync(int id);
        Task<Dealer> AddAsync(Dealer dealer);
        Task UpdateAsync(Dealer dealer);
        Task DeleteAsync(int id);
    }
}