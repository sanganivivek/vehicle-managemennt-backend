using vehicle_management_backend.Core.Models;

namespace vehicle_management_backend.Application.Services.Interfaces
{
    public interface IModelService
    {
        // CHANGE THIS: int -> Guid
        Task<List<Model>> GetModelsByBrandAsync(Guid brandId);
        Task CreateAsync(Model model);
    }
}