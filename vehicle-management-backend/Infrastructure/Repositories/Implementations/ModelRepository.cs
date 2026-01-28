using Microsoft.EntityFrameworkCore;
using vehicle_management_backend.Core.Models;
using vehicle_management_backend.Infrastructure.Data;
using vehicle_management_backend.Infrastructure.Repositories.Interfaces;
namespace vehicle_management_backend.Infrastructure.Repositories.Implementations
{
    public class ModelRepository : IModelRespository
    {
        private readonly AppDbContext _context;
        public ModelRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Model>> GetAllAsync()
        {
            return await _context.Models.ToListAsync();
        }
        public async Task<List<Model>> GetByBrandIdAsync(Guid brandId)
        {
            return await _context.Models
                                 .Where(m => m.BrandId == brandId)
                                 .ToListAsync();
        }
        public async Task AddAsync(Model model)
        {
            await _context.Models.AddAsync(model);
            await _context.SaveChangesAsync();
        }

        public async Task<Model?> GetByIdAsync(Guid id)
        {
            return await _context.Models.FindAsync(id);
        }

        public async Task UpdateAsync(Model model)
        {
            _context.Models.Update(model);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var model = await _context.Models.FindAsync(id);
            if (model != null)
            {
                _context.Models.Remove(model);
                await _context.SaveChangesAsync();
            }
        }
    }
}