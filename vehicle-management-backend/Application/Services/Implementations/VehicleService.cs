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
        public async Task<IList<VehicleMaster>> GetAllAsync()
        {
            return await _vehicleRepository.GetAllAsync();
        }

        public async Task<(IList<VehicleMaster> Items, int TotalCount)> GetVehiclesAsync(string? search, string? brand, int? status, string? sortBy, string? sortOrder, int page, int pageSize)
        {
            var result = await _vehicleRepository.GetVehiclesAsync(search, brand, status, sortBy, sortOrder, page, pageSize);
            return (result.Items, result.TotalCount);
        }
        
        public async Task<VehicleMaster?> GetByIdAsync(Guid id)
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
        public async Task DeleteAsync(Guid id)
        {
            await _vehicleRepository.DeleteAsync(id);
        }
        
        // Stored Procedure Methods
        public async Task<IList<VehicleMaster>> GetAllSPAsync()
        {
            return await _vehicleRepository.GetAllViaStoredProcAsync();
        }
        
        public async Task CreateSPAsync(VehicleMaster vehicle)
        {
            await _vehicleRepository.CreateViaStoredProcAsync(vehicle);
        }
    }
}