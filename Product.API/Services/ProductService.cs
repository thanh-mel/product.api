using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Product.API.Exceptions;
using Product.API.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Product.API.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class ProductService : IProductService
    {
        private readonly ILogger<ProductService> _logger;
        private readonly IRepository<Models.Product> _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOptimisticLockingResolver _optimisticLockingResolver;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="repository"></param>
        /// <param name="httpContextAccessor"></param>
        /// <param name="optimisticLockingResolver"></param>
        public ProductService(ILogger<ProductService> logger,
            IRepository<Models.Product> repository,
            IHttpContextAccessor httpContextAccessor,
            IOptimisticLockingResolver optimisticLockingResolver)
        {
            _logger = logger;
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
            _optimisticLockingResolver = optimisticLockingResolver;
        }
        /// <summary>
        /// Create new product.
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public async Task<Models.Product> CreateProductAsync(Models.Product product)
        {
            return await _repository.CreateAsync(product);
        }
        /// <summary>
        /// Delete existing product.
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public async Task DeleteProductAsync(string productId)
        {
            await _repository.DeleteAsync(productId);
        }

        /// <summary>
        /// Get all products
        /// </summary>
        /// <returns></returns>
        public async Task<IQueryable<Models.Product>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }
        /// <summary>
        /// Get a specific product by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Models.Product> GetAsync(Guid id)
        {            
            var product = await _repository.GetAsync(id);

            return product;
        }
        /// <summary>
        /// Update the existing product.
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public async Task UpdateProductAsync(Models.Product product)
        {
            //IMPORTANT: In order to avoid the LOST UPDATE problem from REST API, we need to implement the optimistic locking mechanism.
            //The optimistic locking mechanism can be done at database level or web api level. 
            //This project will choose the web api implementation by using the ETag & If-Match from Http request headers

            //1. Reload the product from database to see if there is any recent changes
            var existingProduct = await _repository.GetAsync(Guid.Parse(product.Id));

            //2. Compare the hash from the request header If-Match to the existing product
            var isRequestValid = _optimisticLockingResolver.IsRequestValid(_httpContextAccessor.HttpContext.Request.Headers["If-Match"], existingProduct);
            
            if(!isRequestValid)
            {
                _logger.LogError($"The product id {product.Id} has been changed recently. To avoid the LOST UPDATE problem, please reload the product details and try again. Product: {JsonConvert.SerializeObject(product)}");
                throw new ConcurrencyOperationException($"The product id {product.Id} has been changed recently. To avoid the LOST UPDATE problem, please reload the product details and try again.");
            }

            await _repository.UpdateAsync(product);
        }
    }
}
