using Microsoft.EntityFrameworkCore;
using vehicle_management_backend.Core.Models;
using vehicle_management_backend.Infrastructure.Data;
using vehicle_management_backend.Infrastructure.Repositories.Interfaces;

namespace vehicle_management_backend.Infrastructure.Repositories.Implementations
{
    public class DealerRepository : IDealerRepository
    {
        private readonly AppDbContext _context;

        public DealerRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Dealer>> GetAllAsync()
        {
            // Retrieves all dealers from the database
            return await _context.Dealers.ToListAsync();
        }

        public async Task<Dealer> GetByIdAsync(int id)
        {
            // Finds a specific dealer by ID
            return await _context.Dealers.FindAsync(id);
        }

        public async Task<Dealer> AddAsync(Dealer dealer)
        {
            // Adds a new dealer and saves changes
            await _context.Dealers.AddAsync(dealer);
            await _context.SaveChangesAsync();
            return dealer;
        }

        public async Task UpdateAsync(Dealer dealer)
        {
            // Updates an existing dealer
            _context.Dealers.Update(dealer);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            // Finds the dealer and removes it if it exists
            var dealer = await _context.Dealers.FindAsync(id);
            if (dealer != null)
            {
                _context.Dealers.Remove(dealer);
                await _context.SaveChangesAsync();
            }
        }
    }
}