using vehicle_management_backend.Core.Models;

namespace vehicle_management_backend.Application.Services.Interfaces
{
    public interface IVehicleService
    {
        // Fix 1: No parameter
        Task<IList<VehicleMaster>> GetAllAsync();

        // Fix 2: Guid
        Task<VehicleMaster?> GetByIdAsync(Guid id);

        Task CreateAsync(VehicleMaster vehicle);
        Task UpdateAsync(VehicleMaster vehicle);

        // Fix 3: Guid
        Task DeleteAsync(Guid id);
    }
}