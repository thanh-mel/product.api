using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Product.API.Exceptions;
using Product.API.Repositories;
using Product.API.Services;
using System;
using System.Threading.Tasks;

namespace Product.API.Test
{
    /// <summary>
    /// 
    /// </summary>
    [TestFixture]
    public class ProductServiceTests
    {
        private Mock<ILogger<ProductService>> _logger;
        private Mock<IRepository<Models.Product>> _repository;
        private Mock<IHttpContextAccessor> _httpContextAccessor;
        private Mock<IOptimisticLockingResolver> _optimisticLockingResolver;
        [SetUp]
        public void Setup()
        {
            _logger = new Mock<ILogger<ProductService>>();
            _repository = new Mock<IRepository<Models.Product>>();
            _httpContextAccessor = new Mock<IHttpContextAccessor>();
            _optimisticLockingResolver = new Mock<IOptimisticLockingResolver>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task CreateProductAsync_ValidProduct_ReturnProduct()
        {
            //Arrange
            var product = new Models.Product()
            {
                Id = "7fb87db7-4c8d-4e19-905d-0d3fb88fb6a4",
                Name = "Test Product",
                Description = "This is test product"
            };

            _repository.Setup(x => x.CreateAsync(product)).ReturnsAsync(product);

            //Act
            var productService = new ProductService(_logger.Object,
                                                    _repository.Object,
                                                    _httpContextAccessor.Object,
                                                    _optimisticLockingResolver.Object);

            var result = await productService.CreateProductAsync(product);

            //Assert
            Assert.IsNotNull(product);
        }
        /// <summary>
        /// Assuming we are creating a new product where its productId is already existing in the database. 
        /// In fact, the test method can apply to any exception thrown from the service.
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task CreateProductAsync_DuplicateProductId_ThrowException()
        {
            //Arrange
            var product = new Models.Product()
            {
                Id = "7fb87db7-4c8d-4e19-905d-0d3fb88fb6a4",
                Name = "Test Product",
                Description = "This is test product"
            };

            _repository.Setup(x => x.CreateAsync(product)).ThrowsAsync(new Exception());

            //Act
            var productService = new ProductService(_logger.Object,
                                                    _repository.Object,
                                                    _httpContextAccessor.Object,
                                                    _optimisticLockingResolver.Object);

            //Assert
            Assert.ThrowsAsync<Exception>(async () => await productService.CreateProductAsync(product));
        }
        /// <summary>
        /// This test method will verify the following:
        /// The hash in the header's If-Match is not the same as the product to be updated, therefore action will be rejected
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task UpdateProductAsync_TheIfMatchNotValid_UpdateDeclined()
        {
            //Arrange
            var existingProduct = new Models.Product()
            {
                Id = "7fb87db7-4c8d-4e19-905d-0d3fb88fb6a4",
                Name = "Test Product",
                Description = "This is test product - this description has been changed."
            };

            var updateProduct = new Models.Product()
            {
                Id = "7fb87db7-4c8d-4e19-905d-0d3fb88fb6a4",
                Name = "Test Product",
                Description = "This is test product - to be updated"
            };

            var hashFromIfMatch = "6D23E6A0417D6F9803CC3EE5D72C59334167F853E125D886E5B7021F1E997FB4";

            _repository.Setup(x => x.GetAsync(It.Is<Guid>(x => x == Guid.Parse(existingProduct.Id)))).ReturnsAsync(existingProduct);
            _httpContextAccessor.Setup(x => x.HttpContext.Request.Headers["If-Match"]).Returns(hashFromIfMatch);
            _optimisticLockingResolver.Setup(x => x.IsRequestValid(It.IsAny<string>(), It.IsAny<object>())).Returns(false);

            //Act
            var productService = new ProductService(_logger.Object,
                                                    _repository.Object,
                                                    _httpContextAccessor.Object,
                                                    _optimisticLockingResolver.Object);


            //Assert
            Assert.ThrowsAsync<ConcurrencyOperationException>(async () => await productService.UpdateProductAsync(updateProduct));
        }

        /// <summary>
        /// This test method will verify the following:
        /// The hash in the header's If-Match is same as the product to be updated, therefore action will be accepted
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task UpdateProductAsync_TheIfMatchValid_UpdateAccepted()
        {
            //Arrange
            var existingProduct = new Models.Product()
            {
                Id = "7fb87db7-4c8d-4e19-905d-0d3fb88fb6a4",
                Name = "Test Product",
                Description = "This is test product"
            };

            var updateProduct = new Models.Product()
            {
                Id = "7fb87db7-4c8d-4e19-905d-0d3fb88fb6a4",
                Name = "Test Product",
                Description = "This is test product - to be updated"
            };

            var hashFromIfMatch = "6D23E6A0417D6F9803CC3EE5D72C59334167F853E125D886E5B7021F1E997FB4";

            _repository.Setup(x => x.GetAsync(It.Is<Guid>(x => x == Guid.Parse(existingProduct.Id)))).ReturnsAsync(existingProduct);
            _httpContextAccessor.Setup(x => x.HttpContext.Request.Headers["If-Match"]).Returns(hashFromIfMatch);
            _optimisticLockingResolver.Setup(x => x.IsRequestValid(It.IsAny<string>(), It.IsAny<object>())).Returns(true);

            //Act
            var productService = new ProductService(_logger.Object,
                                                    _repository.Object,
                                                    _httpContextAccessor.Object,
                                                    _optimisticLockingResolver.Object);

            await productService.UpdateProductAsync(updateProduct);

            //Assert
            _repository.Verify(x => x.UpdateAsync(updateProduct), Times.Once);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task GetAsync_ValidProductId_ReturnProduct()
        {
            //Arrange
            var product = new Models.Product()
            {
                Id = "7fb87db7-4c8d-4e19-905d-0d3fb88fb6a4",
                Name = "Test Product",
                Description = "This is test product"
            };

            _repository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(product);

            //Act
            var productService = new ProductService(_logger.Object,
                                                    _repository.Object,
                                                    _httpContextAccessor.Object,
                                                    _optimisticLockingResolver.Object);

            var result = await productService.GetAsync(Guid.NewGuid());

            //Assert
            Assert.AreEqual(product, result);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task GetAsync_InvalidProductId_ReturnNull()
        {
            //Arrange
            Models.Product product = null;

            _repository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(product);

            //Act
            var productService = new ProductService(_logger.Object,
                                                    _repository.Object,
                                                    _httpContextAccessor.Object,
                                                    _optimisticLockingResolver.Object);

            var result = await productService.GetAsync(Guid.NewGuid());

            //Assert
            Assert.AreEqual(product, result);
        }
    }
}