using vehicle_management_backend.Core.DTOs;

namespace vehicle_management_backend.Application.Services.Interfaces
{
    public interface IBrandService
    {
        Task AddBrandAsync(BrandDTO dto);
        Task<List<BrandDTO>> GetBrandsAsync();
        // Added missing methods
        Task<BrandDTO?> GetBrandByIdAsync(Guid id);
        Task UpdateBrandAsync(Guid id, BrandDTO dto);
        Task DeleteBrandAsync(Guid id);
    }
}