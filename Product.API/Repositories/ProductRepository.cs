using Dapper;
using Microsoft.Extensions.Logging;
using Product.API.DbConnection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Product.API.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    public class ProductRepository : IProductRepository
    {
        private readonly ILogger<ProductRepository> _logger;
        private readonly IDbConnectionStrategy _dbConnectionStrategy;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="dbConnectionStrategy"></param>
        public ProductRepository(ILogger<ProductRepository> logger,
            IDbConnectionStrategy dbConnectionStrategy)
        {
            _logger = logger;
            _dbConnectionStrategy = dbConnectionStrategy;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Task DeleteAsync(Models.Product model)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IQueryable<Models.Product>> GetAllAsync()
        {
            try
            {
                using (var conn = await _dbConnectionStrategy.CreateConnectionAsync(Models.DatabaseType.Sqlite))
                {
                    var query = @"
                                    select Id, Name, Description, Price, DeliveryPrice
                                    from Products
                                ";

                    var result = await conn.QueryAsync<Models.Product>(query);
                    return result.AsQueryable();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when trying to get all products. Exception: {ex}");
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Models.Product> GetAsync(Guid id)
        {
            try
            {
                using (var conn = await _dbConnectionStrategy.CreateConnectionAsync(Models.DatabaseType.Sqlite))
                {
                    var query = @"
                                    select Id, Name, Description, Price, DeliveryPrice
                                    from Products
                                    where Id = @Id collate nocase
                                ";

                    var result = await conn.QueryFirstOrDefaultAsync<Models.Product>(query, new { Id = id.ToString() });
                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when trying to get the product id {id}. Exception: {ex}");
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Task UpsertAsync(Models.Product model)
        {
            throw new NotImplementedException();
        }
    }
}
