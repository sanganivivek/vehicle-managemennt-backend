using vehicle_management_backend.Core.DTOs;
using vehicle_management_backend.Core.Models;
namespace vehicle_management_backend.Application.Services.Interfaces
{
    public interface IModelService
    {
        Task<List<Model>> GetModelsByBrandAsync(Guid brandId);
        Task<List<ModelDTO>> GetModelsAsync();
        Task<ModelDTO?> GetByIdAsync(Guid id);
        Task CreateAsync(Model model);
        Task UpdateAsync(Guid id, CreateModelDTO dto);
        Task DeleteAsync(Guid id);
    }
}