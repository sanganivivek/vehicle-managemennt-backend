using vehicle_management_backend.Core.DTOs;
namespace vehicle_management_backend.Application.Services.Interfaces
{
    public interface IBrandService
    {
        Task AddBrandAsync(BrandDTO dto);
        Task<List<BrandDTO>> GetBrandsAsync();
    }
}