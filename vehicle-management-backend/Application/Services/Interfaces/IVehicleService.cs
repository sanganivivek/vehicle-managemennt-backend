using vehicle_management_backend.Core.Models;

namespace vehicle_management_backend.Application.Services.Interfaces
{
    public interface IVehicleService
    {
        Task<List<VehicleMaster>> GetAllAsync();
        Task<VehicleMaster?> GetByIdAsync(int id);
        Task CreateAsync(VehicleMaster vehicle);
        Task UpdateAsync(VehicleMaster vehicle);
        Task DeleteAsync(int id);
    }
}
