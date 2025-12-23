using vehicle_management_backend.Application.Services.Interfaces;
using vehicle_management_backend.Core.DTOs;
using vehicle_management_backend.Core.Models;
using vehicle_management_backend.Infrastructure.Repositories.Interfaces;

namespace vehicle_management_backend.Application.Services.Implementations
{
    public class BrandService : IBrandService
    {
        private readonly IBrandRepository _brandRepository;

        public BrandService(IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
        }

        public async Task AddBrandAsync(BrandDTO dto)
        {
            var brand = new Brand
            {
                BrandId = Guid.NewGuid(),
                BrandName = dto.BrandName,
                Models = new List<Model>() // Fix: Set required Models property
            };

            await _brandRepository.AddAsync(brand);
        }

        public async Task<List<BrandDTO>> GetBrandsAsync()
        {
            var brands = await _brandRepository.GetAllAsync();
            return brands.Select(b => new BrandDTO
            {
                BrandName = b.BrandName
            }).ToList();
        }
    }
}
