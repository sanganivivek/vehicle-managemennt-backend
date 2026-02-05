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
            return await _dealerRepository.GetAllDealersAsync();
        }

        public async Task<Dealer> GetDealerByIdAsync(int id)
        {
            return await _dealerRepository.GetDealerByIdAsync(id);
        }

        public async Task<Dealer> CreateDealerAsync(CreateDealerDTO dealerDto)
        {
            var dealer = new Dealer
            {
                Name = dealerDto.Name,
                Address = dealerDto.Address,
                City = dealerDto.City,
                MobileNo = dealerDto.MobileNo,
                EmailId = dealerDto.EmailId
            };
            return await _dealerRepository.AddDealerAsync(dealer);
        }

        public async Task<Dealer> UpdateDealerAsync(int id, UpdateDealerDTO dealerDto)
        {
            var existingDealer = await _dealerRepository.GetDealerByIdAsync(id);
            if (existingDealer == null) return null;

            existingDealer.Name = dealerDto.Name;
            existingDealer.Address = dealerDto.Address;
            existingDealer.City = dealerDto.City;
            existingDealer.MobileNo = dealerDto.MobileNo;
            existingDealer.EmailId = dealerDto.EmailId;

            return await _dealerRepository.UpdateDealerAsync(existingDealer);
        }

        public async Task<bool> DeleteDealerAsync(int id)
        {
            return await _dealerRepository.DeleteDealerAsync(id);
        }
    }
}