using vehicle_management_backend.Core.Models;
namespace vehicle_management_backend.Infrastructure.Repositories.Interfaces
{
    public interface IModelRespository
    {
        Task<List<Model>> GetAllAsync();
        Task<Model?> GetByIdAsync(Guid id);
        Task<List<Model>> GetByBrandIdAsync(Guid brandId); 
        Task AddAsync(Model model);
        Task UpdateAsync(Model model);
        Task DeleteAsync(Guid id);
    }
}