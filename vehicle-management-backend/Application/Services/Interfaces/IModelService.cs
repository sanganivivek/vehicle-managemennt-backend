using vehicle_management_backend.Core.Models;

namespace vehicle_management_backend.Application.Services.Interfaces
{
    public interface IModelService
    {
        Task<List<Model>> GetModelsByBrandAsync(Guid brandId); // Changed int to Guid
        Task CreateAsync(Model model);
    }
}