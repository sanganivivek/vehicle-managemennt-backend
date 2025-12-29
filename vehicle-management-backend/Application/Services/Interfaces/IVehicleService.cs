using vehicle_management_backend.Core.Models;

namespace vehicle_management_backend.Application.Services.Interfaces
{
    public interface IVehicleService
    {
        // Change int -> Guid
        Task<IList<VehicleMaster>> GetAllAsync();
        Task<VehicleMaster?> GetByIdAsync(Guid id);

        Task CreateAsync(VehicleMaster vehicle);
        Task UpdateAsync(VehicleMaster vehicle);
        Task DeleteAsync(Guid id);
    }
}