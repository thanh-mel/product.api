using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Product.API.Exceptions;
using Product.API.Filters;
using Product.API.Services;
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
        private readonly IProductService _productService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="productService"></param>
        public ProductController(ILogger<ProductController> logger,
            IProductService productService)
        {
            _logger = logger;
            _productService = productService;
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
                var products = await _productService.GetAllAsync();
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
        [ETagFilter]
        [Route("v1/products/{productId}"), HttpGet]
        public async Task<IActionResult> GetProductAsync(Guid productId)
        {
            try
            {
                var product = await _productService.GetAsync(productId);
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
        /// <summary>
        /// Update the existing product.
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [Route("v1/products"), HttpPut]
        public async Task<IActionResult> UpdateProductAsync([FromBody] Models.Product product)
        {
            try
            {
                if (!Request.Headers.ContainsKey("If-Match"))
                {
                    //Return 412 Precondition Failed error
                    return StatusCode(412, @"Please provide the hash of the product in the request header named 'If-Match'. 
                                                The hash of the product will be returned in the response header named ETag 
                                                when you get the product details by hitting the GetProduct endpoint.");
                }

                await _productService.UpdateProductAsync(product);

                return Ok();
            }
            catch (ConcurrencyOperationException coe)
            {
                return StatusCode(412, coe.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred. Exception: {ex}");
                return StatusCode(500, "Internal Server Error. Please try again later");
            }
        }
        /// <summary>
        /// Create new product. Please ignore the Product Id and Option Id in the payload as they will be automatically generated.
        /// </summary>
        /// <param name="createProductRequest"></param>
        /// <returns></returns>
        [Route("v1/products"), HttpPost]
        public async Task<IActionResult> CreateProductAsync([FromBody] Models.Product createProductRequest)
        {
            try
            {
                var product = await _productService.CreateProductAsync(createProductRequest);
                return Ok(product);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred. Exception: {ex}");
                return StatusCode(500, "Internal Server Error. Please try again later");
            }
        }
        /// <summary>
        /// Delete the existing product
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [Route("v1/products/{productId}"), HttpDelete]
        public async Task<IActionResult> DeleteProductAsync(string productId)
        {
            try
            {
                await _productService.DeleteProductAsync(productId);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred. Exception: {ex}");
                return StatusCode(500, "Internal Server Error. Please try again later");
            }
        }
    }
}
