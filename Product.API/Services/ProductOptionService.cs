using Microsoft.Extensions.Logging;
using Product.API.Models;
using Product.API.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Product.API.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class ProductOptionService : IProductOptionService
    {
        private readonly ILogger<ProductOptionService> _logger;
        private readonly IRepository<ProductOption> _repository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="repository"></param>
        public ProductOptionService(ILogger<ProductOptionService> logger,
            IRepository<ProductOption> repository)
        {
            _logger = logger;
            _repository = repository;
        }
        /// <summary>
        /// Add options to a specified product
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public async Task AddProductOptionsAsync(List<ProductOption> options)
        {
            foreach(var option in options)
            {
                await _repository.CreateAsync(option);
            }
        }
        /// <summary>
        /// Delete options from a specified product
        /// </summary>
        /// <param name="optionIds"></param>
        /// <returns></returns>
        public async Task DeleteProductOptionsAsync(List<string> optionIds)
        {
            foreach(var optionId in optionIds)
            {
                await _repository.DeleteAsync(optionId);
            }
        }
    }
}
