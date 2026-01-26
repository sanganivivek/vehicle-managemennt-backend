using vehicle_management_backend.Application.Services.Interfaces;
using vehicle_management_backend.Core.DTOs;
using vehicle_management_backend.Core.Models;
using vehicle_management_backend.Infrastructure.Repositories.Interfaces;

namespace vehicle_management_backend.Application.Services.Implementations
{
    public class BrandService : IBrandService
    {
        private readonly IBrandRespository _brandRepository;

        public BrandService(IBrandRespository brandRepository)
        {
            _brandRepository = brandRepository;
        }

        public async Task AddBrandAsync(BrandDTO dto)
        {
            var brand = new Brand
            {
                BrandId = Guid.NewGuid(),
                BrandName = dto.BrandName,
                BrandCode = dto.BrandCode, // Map new field
                IsActive = dto.IsActive,   // Map new field
                Models = new List<Model>()
            };
            await _brandRepository.AddAsync(brand);
        }

        public async Task<List<BrandDTO>> GetBrandsAsync()
        {
            var brands = await _brandRepository.GetAllAsync();
            return brands.Select(b => new BrandDTO
            {
                BrandId = b.BrandId,
                BrandName = b.BrandName,
                BrandCode = b.BrandCode,
                IsActive = b.IsActive
            }).ToList();
        }

        public async Task<BrandDTO?> GetBrandByIdAsync(Guid id)
        {
            var brand = await _brandRepository.GetByIdAsync(id);
            if (brand == null) return null;
            return new BrandDTO
            {
                BrandId = brand.BrandId,
                BrandName = brand.BrandName,
                BrandCode = brand.BrandCode,
                IsActive = brand.IsActive
            };
        }

        public async Task UpdateBrandAsync(Guid id, BrandDTO dto)
        {
            var brand = await _brandRepository.GetByIdAsync(id);
            if (brand != null)
            {
                brand.BrandName = dto.BrandName;
                brand.BrandCode = dto.BrandCode;
                brand.IsActive = dto.IsActive;
                await _brandRepository.UpdateAsync(brand);
            }
        }

        public async Task DeleteBrandAsync(Guid id)
        {
            await _brandRepository.DeleteAsync(id);
        }
    }
}