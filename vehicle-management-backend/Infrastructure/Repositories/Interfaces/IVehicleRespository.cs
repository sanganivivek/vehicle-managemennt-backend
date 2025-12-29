using vehicle_management_backend.Core.Models;

namespace vehicle_management_backend.Infrastructure.Repositories.Interfaces
{
    public interface IVehicleRepository // Fix the class name typo if you can, or keep IVehicleRespository
    {
        // 1. Change int -> Guid
        // 2. Remove 'VehicleListRequest' to match your simplified Repository
        Task<List<VehicleMaster>> GetAllAsync();

        Task<VehicleMaster?> GetByIdAsync(Guid id); // int -> Guid

        Task AddAsync(VehicleMaster vehicle);

        Task UpdateAsync(VehicleMaster vehicle);

        Task DeleteAsync(Guid id); // int -> Guid
    }
}