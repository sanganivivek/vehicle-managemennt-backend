using vehicle_management_backend.Core.Models;
namespace vehicle_management_backend.Infrastructure.Repositories.Interfaces
{
    public interface IBrandRespository
    {
        Task AddAsync(Brand brand);
        Task<List<Brand>> GetAllAsync();
        Task<Brand?> GetByIdAsync(Guid id);
        Task UpdateAsync(Brand brand);
        Task DeleteAsync(Brand id);

    }
}