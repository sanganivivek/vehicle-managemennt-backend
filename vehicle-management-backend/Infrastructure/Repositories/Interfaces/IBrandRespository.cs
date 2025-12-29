using vehicle_management_backend.Core.Models;

namespace vehicle_management_backend.Infrastructure.Repositories.Interfaces
{
    public interface IBrandRespository
    {
        Task AddAsync(Brand brand);
        Task<List<Brand>> GetAllAsync();
    }
}
