using Microsoft.EntityFrameworkCore;
using vehicle_management_backend.Core.Models;
using vehicle_management_backend.Infrastructure.Data;
using vehicle_management_backend.Infrastructure.Repositories.Interfaces;

namespace vehicle_management_backend.Infrastructure.Repositories.Implementations
{
    public class BrandRepository : IBrandRespository
    {
        private readonly AppDbContext _context;
        public BrandRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Brand?> GetByCodeAsync(string brandCode)
        {
            // Case-insensitive check
            return await _context.Brands
                .FirstOrDefaultAsync(b => b.BrandCode.ToLower() == brandCode.ToLower());
        }
        public async Task AddAsync(Brand brand)
        {
            _context.Brands.Add(brand);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Brand>> GetAllAsync()
        {
            return await _context.Brands.ToListAsync();
        }

        public async Task<Brand?> GetByIdAsync(Guid id)
        {
            return await _context.Brands.FindAsync(id);
        }

        public async Task UpdateAsync(Brand brand)
        {
            _context.Brands.Update(brand);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var brand = await _context.Brands.FindAsync(id);
            if (brand != null)
            {
                _context.Brands.Remove(brand);
                await _context.SaveChangesAsync();
            }
        }
    }
}