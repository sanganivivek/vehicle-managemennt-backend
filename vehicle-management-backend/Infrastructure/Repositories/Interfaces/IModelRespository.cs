using vehicle_management_backend.Core.Models;

namespace vehicle_management_backend.Infrastructure.Repositories.Interfaces
{
    public interface IModelRepository
    {
        Task<List<Model>> GetByBrandIdAsync(Guid brandId); // Changed int to Guid
        Task AddAsync(Model model);
    }
}