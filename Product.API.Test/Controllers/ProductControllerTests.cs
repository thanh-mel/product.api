using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Product.API.Controllers;
using Product.API.Exceptions;
using Product.API.Services;
using System;
using System.Threading.Tasks;

namespace Product.API.Test.Controllers
{
    [TestFixture]
    class ProductControllerTests
    {
        private Mock<ILogger<ProductController>> _logger;
        private Mock<IProductService> _productService;


        [SetUp]
        public void SetUp()
        {
            _logger = new Mock<ILogger<ProductController>>();
            _productService = new Mock<IProductService>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task CreateProductAsync_ValidProduct_Return200Ok()
        {
            //Arrange
            var product = new Models.Product()
            {
                Id = "7fb87db7-4c8d-4e19-905d-0d3fb88fb6a4",
                Name = "Test Product",
                Description = "This is test product"
            };

            _productService.Setup(x => x.CreateProductAsync(It.IsAny<Models.Product>())).ReturnsAsync(product);

            //Act
            var controller = new ProductController(_logger.Object, _productService.Object);

            var response = await controller.CreateProductAsync(product);

            var result = response as OkObjectResult;

            //Assert
            Assert.AreEqual(result.StatusCode, 200);
            Assert.AreEqual(result.Value, product);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task CreateProductAsync_InvalidProduct_Return500InternalServerError()
        {
            //Arrange
            var product = new Models.Product()
            {
                Id = "7fb87db7-4c8d-4e19-905d-0d3fb88fb6a4",
                Name = "Test Product",
                Description = "This is test product"
            };

            _productService.Setup(x => x.CreateProductAsync(It.IsAny<Models.Product>())).ThrowsAsync(new Exception());

            //Act
            var controller = new ProductController(_logger.Object, _productService.Object);

            var response = await controller.CreateProductAsync(product);

            var result = response as ObjectResult;

            //Assert
            Assert.AreEqual(result.StatusCode, 500);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task UpdateProductAsync_NoIfMatchFound_UpdateDeclined()
        {
            //Arrange
            var product = new Models.Product()
            {
                Id = "7fb87db7-4c8d-4e19-905d-0d3fb88fb6a4",
                Name = "Test Product",
                Description = "This is test product"
            };

            //Act
            var httpContext = new DefaultHttpContext();
            var controller = new ProductController(_logger.Object, _productService.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };
            var response = await controller.UpdateProductAsync(product);
            var result = response as ObjectResult;

            //Assert
            Assert.AreEqual(result.StatusCode, 412);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task UpdateProductAsync_IfMatchFoundButInvalid_UpdateDeclined()
        {
            //Arrange
            var product = new Models.Product()
            {
                Id = "7fb87db7-4c8d-4e19-905d-0d3fb88fb6a4",
                Name = "Test Product",
                Description = "This is test product"
            };
            _productService.Setup(x => x.UpdateProductAsync(It.IsAny<Models.Product>())).ThrowsAsync(new ConcurrencyOperationException("Oops, concurrency update conflict."));

            //Act
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["If-Match"] = "abcdefgh";
            var controller = new ProductController(_logger.Object, _productService.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };
            var response = await controller.UpdateProductAsync(product);
            var result = response as ObjectResult;

            //Assert
            Assert.AreEqual(result.StatusCode, 412);
            Assert.AreEqual(result.Value, "Oops, concurrency update conflict.");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task UpdateProductAsync_IfMatchFoundAndValid_UpdateAccepted()
        {
            //Arrange
            var product = new Models.Product()
            {
                Id = "7fb87db7-4c8d-4e19-905d-0d3fb88fb6a4",
                Name = "Test Product",
                Description = "This is test product"
            };            
            _productService.Setup(x => x.UpdateProductAsync(It.IsAny<Models.Product>()));

            //Act
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["If-Match"] = "abcdefgh";
            var controller = new ProductController(_logger.Object, _productService.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };
            var response = await controller.UpdateProductAsync(product);
            var result = response as OkResult;

            //Assert
            _productService.Verify(x => x.UpdateProductAsync(It.IsAny<Models.Product>()), Times.Once);
            Assert.AreEqual(result.StatusCode, 200);
            

        }
    }
}
