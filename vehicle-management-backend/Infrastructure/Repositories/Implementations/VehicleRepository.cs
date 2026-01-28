using Microsoft.EntityFrameworkCore;
using vehicle_management_backend.Core.Models;
using vehicle_management_backend.Infrastructure.Data;
using vehicle_management_backend.Infrastructure.Repositories.Interfaces;

namespace vehicle_management_backend.Infrastructure.Repositories.Implementations
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly AppDbContext _context;

        public VehicleRepository(AppDbContext context)
        {
            _context = context;
        }

        // 1. GET ALL VEHICLES
        public async Task<List<VehicleMaster>> GetAllAsync()
        {
            return await _context.Vehicles
                .Include(v => v.Brand)
                .Include(v => v.Model)
                .OrderByDescending(v => v.CreatedAt)
                .ToListAsync();
        }

        // 2. GET VEHICLE BY ID
        public async Task<VehicleMaster?> GetByIdAsync(Guid id)
        {
            return await _context.Vehicles
                .Include(v => v.Brand)
                .Include(v => v.Model)
                .FirstOrDefaultAsync(v => v.VehicleId == id);
        }

        // 3. ADD (INSERT) VEHICLE
        public async Task AddAsync(VehicleMaster vehicle)
        {
            await _context.Vehicles.AddAsync(vehicle);
            await _context.SaveChangesAsync();
        }

        // 4. UPDATE VEHICLE
        public async Task UpdateAsync(VehicleMaster vehicle)
        {
            _context.Vehicles.Update(vehicle);
            await _context.SaveChangesAsync();
        }

        // 5. DELETE VEHICLE
        public async Task DeleteAsync(Guid id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle != null)
            {
                _context.Vehicles.Remove(vehicle);
                await _context.SaveChangesAsync();
            }
        }
    }
}