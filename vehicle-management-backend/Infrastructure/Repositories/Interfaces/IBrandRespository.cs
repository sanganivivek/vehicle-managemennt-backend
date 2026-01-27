using vehicle_management_backend.Core.Models;

namespace vehicle_management_backend.Infrastructure.Repositories.Interfaces
{
    public interface IBrandRespository
    {
        Task AddAsync(Brand brand);
        Task<List<Brand>> GetAllAsync();
        Task<Brand?> GetByIdAsync(Guid id);

        // Add this new method
        Task<Brand?> GetByCodeAsync(string brandCode);

        Task UpdateAsync(Brand brand);
        Task DeleteAsync(Guid id);
    }
}