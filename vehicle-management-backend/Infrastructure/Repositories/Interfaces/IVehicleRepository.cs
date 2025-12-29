using vehicle_management_backend.Core.Models;

namespace vehicle_management_backend.Infrastructure.Repositories.Interfaces
{
    public interface IVehicleRepository
    {
        // Fix 1: Remove 'VehicleListRequest' to match Controller call
        Task<List<VehicleMaster>> GetAllAsync();

        // Fix 2: Change int -> Guid
        Task<VehicleMaster?> GetByIdAsync(Guid id);

        Task AddAsync(VehicleMaster vehicle);

        Task UpdateAsync(VehicleMaster vehicle);

        // Fix 3: Change int -> Guid
        Task DeleteAsync(Guid id);
    }
}