using vehicle_management_backend.Application.Services.Interfaces;
using vehicle_management_backend.Core.DTOs;
using vehicle_management_backend.Core.Models;
using vehicle_management_backend.Infrastructure.Repositories.Interfaces;

namespace vehicle_management_backend.Application.Services.Implementations
{
    public class DealerService : IDealerService
    {
        private readonly IDealerRepository _dealerRepository;

        public DealerService(IDealerRepository dealerRepository)
        {
            _dealerRepository = dealerRepository;
        }

        public async Task<IEnumerable<Dealer>> GetAllDealersAsync()
        {
            return await _dealerRepository.GetAllAsync();
        }

        public async Task<Dealer> GetDealerByIdAsync(int id)
        {
            return await _dealerRepository.GetByIdAsync(id);
        }

        // FIXED: Renamed from AddDealerAsync to CreateDealerAsync to match Interface
        public async Task<Dealer> CreateDealerAsync(CreateDealerDTO dealerDto)
        {
            var dealer = new Dealer
            {
                Name = dealerDto.Name,
                ContactPerson = dealerDto.ContactPerson,
                ContactNo = dealerDto.ContactNo,
                Email = dealerDto.Email,
                GSTNo = dealerDto.GSTNo,
                City = dealerDto.City,
                Address = dealerDto.Address,
                status = dealerDto.Status,
                CreatedDate = DateTime.Now
            };

            return await _dealerRepository.AddAsync(dealer);
        }

        public async Task<Dealer> UpdateDealerAsync(int id, UpdateDealerDTO dealerDto)
        {
            var existingDealer = await _dealerRepository.GetByIdAsync(id);
            if (existingDealer == null) return null;

            existingDealer.Name = dealerDto.Name;
            existingDealer.ContactPerson = dealerDto.ContactPerson;
            existingDealer.ContactNo = dealerDto.ContactNo;
            existingDealer.Email = dealerDto.Email;
            existingDealer.GSTNo = dealerDto.GSTNo;
            existingDealer.City = dealerDto.City;
            existingDealer.Address = dealerDto.Address;
            existingDealer.status = dealerDto.Status;

            await _dealerRepository.UpdateAsync(existingDealer);
            return existingDealer;
        }

        public async Task<bool> DeleteDealerAsync(int id)
        {
            var dealer = await _dealerRepository.GetByIdAsync(id);
            if (dealer == null) return false;

            await _dealerRepository.DeleteAsync(id);
            return true;
        }
    }
}