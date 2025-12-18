using vehicle_management_backend.Core.Models;

namespace vehicle_management_backend.Application.Services.Interfaces
{
    public interface IModelService
    {
        Task<List<Model>> GetModelsByBrandAsync(int brandId);
        Task CreateAsync(Model model);
    }
}
