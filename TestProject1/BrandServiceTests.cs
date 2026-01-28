using Moq;
using Xunit;
using vehicle_management_backend.Application.Services.Implementations;
using vehicle_management_backend.Infrastructure.Repositories.Interfaces;
using vehicle_management_backend.Core.Models;
using vehicle_management_backend.Core.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace TestProject1
{
    public class BrandServiceTests
    {
        // 1. Define Mocks and the Service
        private readonly Mock<IBrandRespository> _mockRepo; // Note: Uses your existing interface name with the typo
        private readonly BrandService _brandService;

        public BrandServiceTests()
        {
            // 2. Initialize the Mock
            _mockRepo = new Mock<IBrandRespository>();

            // 3. Inject the Mock object into the Service
            _brandService = new BrandService(_mockRepo.Object);
        }

        [Fact]
        public async Task GetBrandsAsync_ShouldReturnListOfBrandDTOs()
        {
            // --- ARRANGE ---
            var dummyBrands = new List<Brand>
            {
                new Brand { BrandId = Guid.NewGuid(), BrandName = "Toyota", BrandCode = "TYT", IsActive = true, Models = new List<Model>() },
                new Brand { BrandId = Guid.NewGuid(), BrandName = "Honda", BrandCode = "HND", IsActive = true, Models = new List<Model>() }
            };

            // Setup the mock to return our dummy list
            _mockRepo.Setup(repo => repo.GetAllAsync())
                     .ReturnsAsync(dummyBrands);

            // --- ACT ---
            var result = await _brandService.GetBrandsAsync();

            // --- ASSERT ---
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("Toyota", result[0].BrandName);
        }

        [Fact]
        public async Task GetBrandByIdAsync_ShouldReturnBrand_WhenExists()
        {
            // --- ARRANGE ---
            var brandId = Guid.NewGuid();
            var dummyBrand = new Brand
            {
                BrandId = brandId,
                BrandName = "Ford",
                BrandCode = "FRD",
                IsActive = true,
                Models = new List<Model>()
            };

            _mockRepo.Setup(repo => repo.GetByIdAsync(brandId))
                     .ReturnsAsync(dummyBrand);

            // --- ACT ---
            var result = await _brandService.GetBrandByIdAsync(brandId);

            // --- ASSERT ---
            Assert.NotNull(result);
            Assert.Equal(brandId, result.BrandId);
            Assert.Equal("Ford", result.BrandName);
        }

        [Fact]
        public async Task AddBrandAsync_ShouldCallRepositoryAdd()
        {
            // --- ARRANGE ---
            var newBrandDto = new BrandDTO
            {
                BrandName = "Tesla",
                BrandCode = "TSLA",
                IsActive = true
            };

            // --- ACT ---
            await _brandService.AddBrandAsync(newBrandDto);

            // --- ASSERT ---
            // Verify that the repository's AddAsync was called exactly once
            _mockRepo.Verify(repo => repo.AddAsync(It.Is<Brand>(b =>
                b.BrandName == "Tesla" &&
                b.BrandCode == "TSLA"
            )), Times.Once);
        }

        [Fact]
        public async Task UpdateBrandAsync_ShouldCallRepositoryUpdate()
        {
            // --- ARRANGE ---
            var brandId = Guid.NewGuid();
            var existingBrand = new Brand { BrandId = brandId, BrandName = "Old", BrandCode = "OLD", Models = new List<Model>() };
            var updateDto = new BrandDTO { BrandName = "New", BrandCode = "NEW", IsActive = true };

            // Mock finding the existing brand
            _mockRepo.Setup(repo => repo.GetByIdAsync(brandId))
                     .ReturnsAsync(existingBrand);

            // --- ACT ---
            await _brandService.UpdateBrandAsync(brandId, updateDto);

            // --- ASSERT ---
            // Verify UpdateAsync was called
            _mockRepo.Verify(repo => repo.UpdateAsync(It.IsAny<Brand>()), Times.Once);
        }

        [Fact]
        public async Task DeleteBrandAsync_ShouldCallRepositoryDelete()
        {
            // --- ARRANGE ---
            var brandId = Guid.NewGuid();

            // --- ACT ---
            await _brandService.DeleteBrandAsync(brandId);

            // --- ASSERT ---
            _mockRepo.Verify(repo => repo.DeleteAsync(brandId), Times.Once);
        }
    }
}