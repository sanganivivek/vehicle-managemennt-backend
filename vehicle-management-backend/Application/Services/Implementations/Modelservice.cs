using vehicle_management_backend.Application.Services.Interfaces;
using vehicle_management_backend.Core.Models;
using vehicle_management_backend.Infrastructure.Repositories.Interfaces;

namespace vehicle_management_backend.Application.Services.Implementations
{
    public class ModelService : IModelService
    {
        private readonly IModelRepository _modelRepository;

        public ModelService(IModelRepository modelRepository)
        {
            _modelRepository = modelRepository;
        }

        public async Task<List<Model>> GetModelsByBrandAsync(int brandId)
        {
            return await _modelRepository.GetByBrandIdAsync(brandId);
        }

        public async Task CreateAsync(Model model)
        {
            await _modelRepository.AddAsync(model);
        }
    }
}
