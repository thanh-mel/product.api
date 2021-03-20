using System;
using System.Linq;
using System.Threading.Tasks;

namespace Product.API.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface IProductService
    {
        /// <summary>
        /// Get the product by Guid
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Models.Product> GetAsync(Guid id);
        /// <summary>
        /// Get all products 
        /// </summary>
        /// <returns></returns>
        Task<IQueryable<Models.Product>> GetAllAsync();
        /// <summary>
        /// Update existing product
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        Task UpdateProductAsync(Models.Product product);
        /// <summary>
        /// Create new product
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        Task<Models.Product> CreateProductAsync(Models.Product product);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        Task DeleteProductAsync(string productId);
    }
}
