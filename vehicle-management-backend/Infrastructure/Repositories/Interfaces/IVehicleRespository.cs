using vehicle_management_backend.Core.Models;

namespace vehicle_management_backend.Infrastructure.Repositories.Interfaces
{
    public interface IVehicleRepository
    {
        Task<List<VehicleMaster>> GetAllAsync();
        Task<VehicleMaster?> GetByIdAsync(int id);
        Task AddAsync(VehicleMaster vehicle);
        Task UpdateAsync(VehicleMaster vehicle);
        Task DeleteAsync(int id);
    }
}
