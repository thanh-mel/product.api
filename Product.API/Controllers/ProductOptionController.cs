using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        /// Add more options to a specified product.
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        [Route("v1/options"), HttpPost]
        public async Task<IActionResult> AddProductOptionsAsync([FromBody] List<ProductOption> options)
        {
            try
            {
                await _productOptionService.AddProductOptionsAsync(options);
                return Ok();
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
                await _productOptionService.DeleteProductOptionsAsync(optionIds);
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
