using vehicle_management_backend.Core.Models;

namespace vehicle_management_backend.Infrastructure.Repositories.Interfaces
{
    public interface IVehicleRepository
    {
        // 1. Remove 'VehicleListRequest' parameter
        Task<List<VehicleMaster>> GetAllAsync();

        // 2. Change int -> Guid
        Task<VehicleMaster?> GetByIdAsync(Guid id);

        Task AddAsync(VehicleMaster vehicle);

        Task UpdateAsync(VehicleMaster vehicle);

        // 3. Change int -> Guid (Crucial Fix)
        Task DeleteAsync(Guid id);
    }
}