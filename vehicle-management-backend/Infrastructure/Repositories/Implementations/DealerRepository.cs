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

        public async Task<IEnumerable<Dealer>> GetAllDealersAsync()
        {
            return await _context.Dealers.ToListAsync();
        }

        public async Task<Dealer> GetDealerByIdAsync(int id)
        {
            return await _context.Dealers.FindAsync(id);
        }

        public async Task<Dealer> AddDealerAsync(Dealer dealer)
        {
            await _context.Dealers.AddAsync(dealer);
            await _context.SaveChangesAsync();
            return dealer;
        }

        public async Task<Dealer> UpdateDealerAsync(Dealer dealer)
        {
            _context.Dealers.Update(dealer);
            await _context.SaveChangesAsync();
            return dealer;
        }

        public async Task<bool> DeleteDealerAsync(int id)
        {
            var dealer = await _context.Dealers.FindAsync(id);
            if (dealer == null) return false;

            _context.Dealers.Remove(dealer);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}