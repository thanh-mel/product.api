using Microsoft.Extensions.Logging;
using Product.API.Exceptions;
using Product.API.Models;
using Product.API.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Product.API.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class ProductOptionService : IProductOptionService
    {
        private readonly ILogger<ProductOptionService> _logger;
        private readonly IRepository<Models.Product> _productRepository;
        private readonly IRepository<ProductOption> _productOptionrepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="productRepository"></param>
        /// <param name="productOptionRepository"></param>
        public ProductOptionService(ILogger<ProductOptionService> logger,
            IRepository<Models.Product> productRepository,
            IRepository<ProductOption> productOptionRepository)
        {
            _logger = logger;
            _productRepository = productRepository;
            _productOptionrepository = productOptionRepository;
        }
        /// <summary>
        /// Add options to a specified product
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public async Task<List<ProductOption>> AddManyProductOptionsAsync(string productId, List<ProductOption> options)
        {
            if (options == null || !options.Any())
            {
                // Nothing to add
                return null;
            }

            //Only add options which have value for Name
            var validOptions = options.Where(x => !string.IsNullOrEmpty(x.Name)).ToList();
            if (!validOptions.Any())
            {
                //Nothing to add too
                return null;
            }

            var product = await _productRepository.GetAsync(Guid.Parse(productId));
            if (product == null)
            {
                _logger.LogDebug($"Product {productId} not found. Ignore product options addition.");
                throw new ProductNotFoundException($"Product {productId} not found");
            }

            foreach (var option in validOptions)
            {
                option.ProductId = productId;
                option.Id = Guid.NewGuid().ToString();
            }

            await _productOptionrepository.CreateManyAsync(validOptions);

            return validOptions;
        }
        /// <summary>
        /// Delete options from a specified product
        /// </summary>
        /// <param name="optionIds"></param>
        /// <returns></returns>
        public async Task DeleteManyProductOptionsAsync(List<string> optionIds)
        {
            if (optionIds == null || !optionIds.Any())
            {
                // Nothing to delete
                return;
            }

            await _productOptionrepository.DeleteManyAsync(optionIds);
        }
    }
}
