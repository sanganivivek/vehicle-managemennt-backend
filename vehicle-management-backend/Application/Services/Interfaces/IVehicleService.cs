using vehicle_management_backend.Core.DTOs;

public interface IVehicleService
{
    Task<List<VehicleDTO>> GetAllAsync();
    Task<VehicleDTO?> GetByIdAsync(Guid id);
    Task CreateAsync(CreateVehicleDTO dto);
    Task UpdateAsync(Guid id, UpdateVehicleDTO dto);
    Task DeleteAsync(Guid id);
}
