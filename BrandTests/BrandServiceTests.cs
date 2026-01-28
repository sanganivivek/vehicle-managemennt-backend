using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using vehicle_management_backend.Application.Services.Implementations;
using vehicle_management_backend.Core.DTOs;
using vehicle_management_backend.Core.Models;
using vehicle_management_backend.Infrastructure.Repositories.Interfaces;
using Xunit;

namespace TestProject1.BrandTests
{
    public class BrandServiceTests
    {
        // 1. Define Mocks and the Service
        private readonly Mock<IBrandRespository> _mockRepo;
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

            _mockRepo.Setup(repo => repo.GetAllAsync())
                     .ReturnsAsync(dummyBrands);

            // --- ACT ---
            var result = await _brandService.GetBrandsAsync();

            // --- ASSERT ---
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("Toyota", result[0].BrandName);
            Assert.Equal("TYT", result[0].BrandCode);
        }

        [Fact]
        public async Task GetBrandByIdAsync_ShouldReturnBrandDTO_WhenExists()
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
        public async Task GetBrandByIdAsync_ShouldReturnNull_WhenDoesNotExist()
        {
            // --- ARRANGE ---
            var brandId = Guid.NewGuid();
            _mockRepo.Setup(repo => repo.GetByIdAsync(brandId))
                     .ReturnsAsync((Brand?)null);

            // --- ACT ---
            var result = await _brandService.GetBrandByIdAsync(brandId);

            // --- ASSERT ---
            Assert.Null(result);
        }

        [Fact]
        public async Task AddBrandAsync_ShouldCallAddAsyncInRepository_WithMappedEntity()
        {
            // --- ARRANGE ---
            var brandDto = new BrandDTO
            {
                BrandName = "Tesla",
                BrandCode = "TSLA",
                IsActive = true
            };

            // --- ACT ---
            await _brandService.AddBrandAsync(brandDto);

            // --- ASSERT ---
            // Verify that AddAsync was called exactly once with a Brand object matching our DTO
            _mockRepo.Verify(repo => repo.AddAsync(It.Is<Brand>(b =>
                b.BrandName == brandDto.BrandName &&
                b.BrandCode == brandDto.BrandCode &&
                b.IsActive == brandDto.IsActive &&
                b.BrandId != Guid.Empty // Verify ID was generated
            )), Times.Once);
        }

        [Fact]
        public async Task UpdateBrandAsync_ShouldUpdateAndCallSave_WhenBrandExists()
        {
            // --- ARRANGE ---
            var brandId = Guid.NewGuid();
            var existingBrand = new Brand
            {
                BrandId = brandId,
                BrandName = "Old Name",
                BrandCode = "OLD",
                IsActive = false,
                Models = new List<Model>()
            };

            var updateDto = new BrandDTO
            {
                BrandName = "New Name",
                BrandCode = "NEW",
                IsActive = true
            };

            // Setup mock to return the existing brand when GetById is called
            _mockRepo.Setup(repo => repo.GetByIdAsync(brandId))
                     .ReturnsAsync(existingBrand);

            // --- ACT ---
            await _brandService.UpdateBrandAsync(brandId, updateDto);

            // --- ASSERT ---
            // 1. Verify GetById was called
            _mockRepo.Verify(repo => repo.GetByIdAsync(brandId), Times.Once);

            // 2. Verify UpdateAsync was called with the modified object
            _mockRepo.Verify(repo => repo.UpdateAsync(It.Is<Brand>(b =>
                b.BrandId == brandId &&
                b.BrandName == "New Name" &&
                b.BrandCode == "NEW" &&
                b.IsActive == true
            )), Times.Once);
        }

        [Fact]
        public async Task DeleteBrandAsync_ShouldCallDeleteAsyncInRepository()
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