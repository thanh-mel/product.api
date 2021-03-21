using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Product.API.Exceptions;
using Product.API.Models;
using Product.API.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Product.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ProductOptionController : ControllerBase
    {
        private readonly ILogger<ProductOptionController> _logger;
        private readonly IProductOptionService _productOptionService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="productService"></param>
        public ProductOptionController(ILogger<ProductOptionController> logger,
            IProductOptionService productService)
        {
            _logger = logger;
            _productOptionService = productService;
        }
        /// <summary>
        /// Add more options to a specified product. Ignore OptionId in the payload as they will be generated automatically.
        /// Required: `productId` parameter in the endpoint.
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        [Route("v1/products/{productId}/options"), HttpPost]
        public async Task<IActionResult> AddProductOptionsAsync(string productId, [FromBody] List<ProductOption> options)
        {
            try
            {
                var result = await _productOptionService.AddManyProductOptionsAsync(productId, options);
                return Ok(result);
            }
            catch(ProductNotFoundException pne)
            {
                _logger.LogError($"An error occurred. Exception: {pne}");
                return NotFound($"Product {productId} not found");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred. Exception: {ex}");
                return StatusCode(500, "Internal Server Error. Please try again later");
            }
        }
        /// <summary>
        /// Delete options from a specified product.
        /// </summary>
        /// <param name="optionIds"></param>
        /// <returns></returns>
        [Route("v1/options"), HttpDelete]
        public async Task<IActionResult> DeleteProductOptionsAsync([FromBody] List<string> optionIds)
        {
            try
            {
                await _productOptionService.DeleteManyProductOptionsAsync(optionIds);
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
