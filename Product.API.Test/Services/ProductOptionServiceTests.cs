using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Product.API.Exceptions;
using Product.API.Models;
using Product.API.Repositories;
using Product.API.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Product.API.Test.Services
{
    [TestFixture]
    class ProductOptionServiceTests
    {
        private Mock<ILogger<ProductOptionService>> _logger;
        private Mock<IRepository<Models.Product>> _productRepository;
        private Mock<IRepository<ProductOption>> _productOptionRepository;
        /// <summary>
        /// 
        /// </summary>
        [SetUp]
        public void Setup()
        {
            _logger = new Mock<ILogger<ProductOptionService>>();
            _productOptionRepository = new Mock<IRepository<ProductOption>>();
            _productRepository = new Mock<IRepository<Models.Product>>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task AddManyProductOptionsAsync_ValidProductIdAndOptions_ReturnOptions()
        {
            //Arrange
            var options = new List<ProductOption>() 
            {
                new ProductOption()
                {
                    Name = "6b64c5a8-00f9-4a57-a62b-54198117ef54",
                    Description = "Option 1"
                },
                new ProductOption()
                {
                    Name = "15601605-3d80-452d-8170-89368bad5f0a",
                    Description = "Option 2"
                }
            };

            var product = new Models.Product()
            { 
                Id = "7fb87db7-4c8d-4e19-905d-0d3fb88fb6a4"
            };

            _productRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(product);
            

            //Act
            var productOptionService = new ProductOptionService(_logger.Object, 
                                                                _productRepository.Object, 
                                                                _productOptionRepository.Object);

            var result = await productOptionService.AddManyProductOptionsAsync(product.Id, options);

            //Assert
            _productOptionRepository.Verify(x => x.CreateManyAsync(It.IsAny<List<ProductOption>>()), Times.Once);
            Assert.IsNotNull(result);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task AddManyProductOptionsAsync_InvalidProductId_ThrowException()
        {
            //Arrange
            var options = new List<ProductOption>()
            {
                new ProductOption()
                {
                    Name = "6b64c5a8-00f9-4a57-a62b-54198117ef54",
                    Description = "Option 1"
                },
                new ProductOption()
                {
                    Name = "15601605-3d80-452d-8170-89368bad5f0a",
                    Description = "Option 2"
                }
            };

            var product = new Models.Product()
            {
                Id = "7fb87db7-4c8d-4e19-905d-0d3fb88fb6a4"
            };

            _productRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ThrowsAsync(new ProductNotFoundException("Product not found"));


            //Act
            var productOptionService = new ProductOptionService(_logger.Object,
                                                                _productRepository.Object,
                                                                _productOptionRepository.Object);

            //Assert
            Assert.ThrowsAsync<ProductNotFoundException>(async () => await productOptionService.AddManyProductOptionsAsync(product.Id, options));

        }
    }
}
