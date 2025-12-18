using vehicle_management_backend.Core.Models;

namespace vehicle_management_backend.Infrastructure.Repositories.Interfaces
{
    public interface IModelRepository
    {
        Task<List<Model>> GetByBrandIdAsync(int brandId);
        Task AddAsync(Model model);
    }
}
