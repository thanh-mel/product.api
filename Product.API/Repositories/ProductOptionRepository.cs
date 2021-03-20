using Microsoft.Extensions.Logging;
using Product.API.DbConnection;
using Product.API.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Product.API.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    public class ProductOptionRepository : IRepository<ProductOption>
    {
        private readonly ILogger<ProductOptionRepository> _logger;
        private readonly IDbConnectionStrategy _dbConnectionStrategy;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="dbConnectionStrategy"></param>
        public ProductOptionRepository(ILogger<ProductOptionRepository> logger, 
            IDbConnectionStrategy dbConnectionStrategy)
        {
            _logger = logger;
            _dbConnectionStrategy = dbConnectionStrategy;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<IQueryable<ProductOption>> GetAllAsync()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<ProductOption> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Task<ProductOption> CreateAsync(ProductOption model)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Task UpdateAsync(ProductOption model)
        {
            throw new NotImplementedException();
        }
    }
}
