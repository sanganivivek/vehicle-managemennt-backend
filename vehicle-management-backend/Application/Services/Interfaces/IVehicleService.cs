// ... your existing using statements ...

using vehicle_management_backend.Core.DTOs;
using vehicle_management_backend.Core.Models;

public interface IVehicleService
{
    Task<IList<VehicleMaster>> GetAllAsync(VehicleListRequest request);

    Task<VehicleMaster?> GetByIdAsync(int id);
    Task CreateAsync(VehicleMaster vehicle);
    Task UpdateAsync(VehicleMaster vehicle);
    Task DeleteAsync(int id);
}
