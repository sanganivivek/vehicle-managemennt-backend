using vehicle_management_backend.Core.Models;
namespace vehicle_management_backend.Infrastructure.Repositories.Interfaces
{
    public interface IVehicleRepository
    {
        Task<List<VehicleMaster>> GetAllAsync();
        Task<VehicleMaster?> GetByIdAsync(Guid id);
        Task<(List<VehicleMaster> Items, int TotalCount)> GetVehiclesAsync(string? search, string? brand, int? status, string? sortBy, string? sortOrder, int page, int pageSize);
        Task AddAsync(VehicleMaster vehicle);
        Task UpdateAsync(VehicleMaster vehicle);
        Task DeleteAsync(Guid id);
        
        // Stored Procedure Methods
        Task<List<VehicleMaster>> GetAllViaStoredProcAsync();
        Task CreateViaStoredProcAsync(VehicleMaster vehicle);
    }
}