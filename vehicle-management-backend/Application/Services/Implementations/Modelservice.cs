using vehicle_management_backend.Application.Services.Interfaces;
using vehicle_management_backend.Core.DTOs;
using vehicle_management_backend.Core.Models;
using vehicle_management_backend.Infrastructure.Repositories.Interfaces;

namespace vehicle_management_backend.Application.Services.Implementations
{
    public class ModelService : IModelService
    {
        private readonly IModelRespository _modelRepository;

        public ModelService(IModelRespository modelRepository)
        {
            _modelRepository = modelRepository;
        }

        public async Task<List<Model>> GetModelsByBrandAsync(Guid brandId)
        {
            return await _modelRepository.GetByBrandIdAsync(brandId);
        }

        public async Task<List<ModelDTO>> GetModelsAsync()
        {
            var models = await _modelRepository.GetAllAsync();
            return models.Select(m => new ModelDTO
            {
                ModelId = m.ModelId,
                ModelName = m.ModelName,
                BrandId = m.BrandId
            }).ToList();
        }

        public async Task CreateAsync(Model model)
        {
            await _modelRepository.AddAsync(model);
        }
    }
}