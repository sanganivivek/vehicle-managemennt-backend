using vehicle_management_backend.Core.Models;

public interface IVehicleService
{
    // FIX: Remove 'VehicleListRequest' parameter
    Task<IList<VehicleMaster>> GetAllAsync();

    // Ensure these use Guid too
    Task<VehicleMaster?> GetByIdAsync(Guid id);
    Task CreateAsync(VehicleMaster vehicle);
    Task UpdateAsync(VehicleMaster vehicle);
    Task DeleteAsync(Guid id);
}