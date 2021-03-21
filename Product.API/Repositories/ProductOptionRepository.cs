using Dapper;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Product.API.DbConnection;
using Product.API.Models;
using System;
using System.Collections.Generic;
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
        public async Task DeleteAsync(string id)
        {
            try
            {
                using (var conn = await _dbConnectionStrategy.CreateConnectionAsync(DatabaseType.Sqlite))
                {
                    var query = @"
                                    delete
                                    from ProductOptions
                                    where Id = @Id collate nocase
                                ";

                    await conn.ExecuteAsync(query, new { Id = id.ToString() });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when trying to delete the product option id {id}. Exception: {ex}");
                throw ex;
            }
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
        public async Task<ProductOption> CreateAsync(ProductOption model)
        {
            try
            {
                using (var conn = await _dbConnectionStrategy.CreateConnectionAsync(DatabaseType.Sqlite))
                {
                    var query = @"
                                    insert into ProductOptions (Id, ProductId, Name, Description)
                                    values (@Id, @ProductId, @Name, @Description)
                                ";

                    await conn.ExecuteAsync(query,
                        new
                        {
                            Id = model.Id,
                            ProductId = model.ProductId,
                            Name = model.Name,
                            Description = model.Description
                        });
                    return model;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when trying to create the product option {JsonConvert.SerializeObject(model)}. Exception: {ex}");
                throw ex;
            }
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public async Task CreateManyAsync(IList<ProductOption> models)
        {
            try
            {
                using (var conn = await _dbConnectionStrategy.CreateConnectionAsync(DatabaseType.Sqlite))
                {
                    var query = @"
                                    insert into ProductOptions (Id, ProductId, Name, Description)
                                    values (@Id, @ProductId, @Name, @Description)
                                ";

                    foreach (var model in models)
                    {
                        await conn.ExecuteAsync(query,
                        new
                        {
                            Id = model.Id,
                            ProductId = model.ProductId,
                            Name = model.Name,
                            Description = model.Description
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when trying to create many product options {JsonConvert.SerializeObject(models)}. Exception: {ex}");
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task DeleteManyAsync(List<string> ids)
        {
            try
            {
                using (var conn = await _dbConnectionStrategy.CreateConnectionAsync(DatabaseType.Sqlite))
                {
                    var query = @"
                                    delete
                                    from ProductOptions
                                    where Id in @Ids
                                ";

                    await conn.ExecuteAsync(query,
                    new
                    {
                        Ids = ids
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when trying to delete many product options {JsonConvert.SerializeObject(ids)}. Exception: {ex}");
                throw ex;
            }
        }
    }
}
