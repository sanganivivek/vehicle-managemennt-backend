using vehicle_management_backend.Core.DTOs;

namespace vehicle_management_backend.Application.Services.Interfaces
{
    public interface IModelService
    {
        Task CreateAsync(CreateModelDTO dto);
        Task<List<ModelDTO>> GetModelsByBrandAsync(Guid brandId);
    }
}
