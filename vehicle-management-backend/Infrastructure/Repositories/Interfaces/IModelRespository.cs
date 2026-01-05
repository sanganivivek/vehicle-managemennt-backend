using vehicle_management_backend.Core.Models;

namespace vehicle_management_backend.Infrastructure.Repositories.Interfaces
{
    public interface IModelRespository
    {
        Task<List<Model>> GetAllAsync();
        Task<List<Model>> GetByBrandIdAsync(Guid brandId); 
        Task AddAsync(Model model);
    }
}