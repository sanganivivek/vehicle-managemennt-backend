using vehicle_management_backend.Application.Services.Interfaces;
using vehicle_management_backend.Core.Models;
using vehicle_management_backend.Infrastructure.Repositories.Interfaces;

namespace vehicle_management_backend.Application.Services.Implementations
{
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleRepository _vehicleRepository;

        public VehicleService(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        public async Task<IList<VehicleMaster>> GetAllAsync(VehicleListRequest request)
        {
            return await _vehicleRepository.GetAllAsync(request);
        }

        public async Task<VehicleMaster?> GetByIdAsync(int id)
        {
            return await _vehicleRepository.GetByIdAsync(id);
        }

        public async Task CreateAsync(VehicleMaster vehicle)
        {
            await _vehicleRepository.AddAsync(vehicle);
        }

        public async Task UpdateAsync(VehicleMaster vehicle)
        {
            await _vehicleRepository.UpdateAsync(vehicle);
        }

        public async Task DeleteAsync(int id)
        {
            await _vehicleRepository.DeleteAsync(id);
        }
    }
}
