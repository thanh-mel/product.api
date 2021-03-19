using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Product.API.Repositories;
using System;
using System.Threading.Tasks;

namespace Product.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductRepository _productRepository;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="productRepository"></param>
        public ProductController(ILogger<ProductController> logger,
            IProductRepository productRepository)
        {
            _logger = logger;
            _productRepository = productRepository;
        }
        /// <summary>
        /// Get all products
        /// </summary>
        /// <returns></returns>
        [Route("v1/products"), HttpGet]
        public async Task<IActionResult> GetAllProductAsync()
        {
            try
            {
                var products = await _productRepository.GetAllAsync();
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred. Exception: {ex}");
                return StatusCode(500, "Internal Server Error. Please try again later");
            }
        }
        /// <summary>
        /// Get a specific product by id
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [Route("v1/products/{productId}"), HttpGet]
        public async Task<IActionResult> GetProductAsync(Guid productId)
        {
            try
            {
                var product = await _productRepository.GetAsync(productId);
                if (product != null)
                {
                    return Ok(product);
                }
                else
                {
                    return NotFound();
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred. Exception: {ex}");
                return StatusCode(500, "Internal Server Error. Please try again later");
            }
        }
    }
}
