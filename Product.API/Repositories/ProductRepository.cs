using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Product.API.DbConnection;
using Product.API.Repositories.DataAccessModels;
using Product.API.Repositories.Mappers;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;


namespace Product.API.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    public class ProductRepository : IRepository<Models.Product>
    {
        private readonly ILogger<ProductRepository> _logger;
        private readonly IDbConnectionStrategy _dbConnectionStrategy;
        private readonly IProductMapper _productMapper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="dbConnectionStrategy"></param>
        /// <param name="productMapper"></param>
        public ProductRepository(ILogger<ProductRepository> logger,
            IDbConnectionStrategy dbConnectionStrategy,
            IProductMapper productMapper)
        {
            _logger = logger;
            _dbConnectionStrategy = dbConnectionStrategy;
            _productMapper = productMapper;
        }
        /// <summary>
        /// Delete product by id.
        /// </summary>
        /// <returns></returns>
        public async Task DeleteAsync(string id)
        {
            IDbTransaction transaction = null;
            try
            {
                using (var conn = await _dbConnectionStrategy.CreateConnectionAsync(Models.DatabaseType.Sqlite))
                {
                    transaction = conn.BeginTransaction();

                    var query = @"
                                    delete
                                    from ProductOptions
                                    where ProductId = @ProductId collate nocase
                                ";

                    await conn.ExecuteAsync(query,
                        new
                        {
                            ProductId = id
                        }, transaction);

                    query = @"
                                delete
                                from Products
                                where Id = @ProductId collate nocase
                            ";
                    await conn.ExecuteAsync(query, new
                    {
                        ProductId = id
                    }, transaction);

                    transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when trying to delete a product id {id}. Exception: {ex}");
                _logger.LogInformation($"Roll back is being invoked...");

                try
                {
                    transaction?.Rollback();
                    _logger.LogInformation($"Roll back is complete.");
                }
                catch (SqliteException sqlEx)
                {
                    if (transaction.Connection != null)
                    {
                        _logger.LogError($"Failed to roll back. Exception: {sqlEx}");
                    }
                }
                throw ex;
            }
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
                                    select p.Id as ProductId, p.Name as ProductName, p.Description as ProductDesc, cast(p.Price as real) as Price, cast(p.DeliveryPrice as real) as DeliveryPrice, 
                                            po.Id as OptionId, po.Name as OptionName, po.Description as OptionDesc
                                    from Products p inner join ProductOptions po on p.Id = po.ProductId
                                ";

                    var result = await conn.QueryAsync<ProductDataAccessModel>(query);

                    var products = _productMapper.Map(result.AsList());

                    return products?.AsQueryable();
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
                                    select p.Id as ProductId, p.Name as ProductName, p.Description as ProductDesc, cast(p.Price as real) as Price, cast(p.DeliveryPrice as real) as DeliveryPrice, 
                                            po.Id as OptionId, po.Name as OptionName, po.Description as OptionDesc
                                    from Products p inner join ProductOptions po on p.Id = po.ProductId
                                    where p.Id = @Id collate nocase
                                ";

                    var result = await conn.QueryAsync<ProductDataAccessModel>(query, new { Id = id.ToString() });

                    var products = _productMapper.Map(result.AsList());

                    return products?.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when trying to get the product id {id}. Exception: {ex}");
                throw ex;
            }
        }
        /// <summary>
        /// Create a new product
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Models.Product> CreateAsync(Models.Product model)
        {
            IDbTransaction transaction = null;
            try
            {
                using (var conn = await _dbConnectionStrategy.CreateConnectionAsync(Models.DatabaseType.Sqlite))
                {
                    transaction = conn.BeginTransaction();

                    var query = @"
                                    insert into Products (Id, Name, Description, Price, DeliveryPrice)
                                    values (@Id, @Name, @Description, @Price, @DeliveryPrice)
                                ";

                    var productId = Guid.NewGuid().ToString();
                    model.Id = productId;

                    await conn.ExecuteAsync(query,
                        new
                        {
                            Id = model.Id,
                            Name = model.Name,
                            Description = model.Description,
                            Price = model.Price,
                            DeliveryPrice = model.DeliveryPrice
                        }, transaction);

                    foreach (var option in model.Options)
                    {
                        query = @"
                                    insert into ProductOptions (Id, ProductId, Name, Description)
                                    values (@OptionId, @ProductId, @Name, @Description)
                                ";
                        await conn.ExecuteAsync(query,
                            new
                            {
                                OptionId = Guid.NewGuid().ToString(),
                                ProductId = productId,
                                Name = option.Name,
                                Description = option.Description
                            }, transaction);
                    }

                    transaction.Commit();
                    return model;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when trying to create a product {JsonConvert.SerializeObject(model)}. Exception: {ex}");
                _logger.LogInformation($"Roll back is being invoked...");
                try
                {
                    transaction?.Rollback();
                    _logger.LogInformation($"Roll back is complete.");
                }
                catch (SqliteException sqlEx)
                {
                    if (transaction.Connection != null)
                    {
                        _logger.LogError($"Failed to roll back. Exception: {sqlEx}");
                    }
                }
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task UpdateAsync(Models.Product model)
        {
            IDbTransaction transaction = null;
            try
            {
                using (var conn = await _dbConnectionStrategy.CreateConnectionAsync(Models.DatabaseType.Sqlite))
                {
                    transaction = conn.BeginTransaction();

                    var query = @"
                                    update Products
                                    set Name = @Name, Description = @Description, Price = @Price, DeliveryPrice = @DeliveryPrice
                                    where Id = @Id collate nocase
                                ";

                    await conn.ExecuteAsync(query,
                    new
                    {
                        Id = model.Id.ToString(),
                        Name = model.Name,
                        Description = model.Description,
                        Price = model.Price,
                        DeliveryPrice = model.DeliveryPrice
                    }, transaction);

                    foreach (var option in model.Options)
                    {
                        query = @"
                                    update ProductOptions
                                    set Name = @Name, Description = @Description
                                    where Id = @OptionId collate nocase
                                ";

                        await conn.ExecuteAsync(query, new
                        {
                            OptionId = option.Id,
                            Name = option.Name,
                            Description = option.Description
                        }, transaction);
                    }

                    transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when trying to update the product. Product: {JsonConvert.SerializeObject(model)}. Exception: {ex}");
                _logger.LogInformation($"Roll back is being invoked...");
                try
                {
                    transaction?.Rollback();
                    _logger.LogInformation($"Roll back is complete.");
                }
                catch (SqliteException sqlEx)
                {
                    if (transaction.Connection != null)
                    {
                        _logger.LogError($"Failed to roll back. Exception: {sqlEx}");
                    }
                }
                throw ex;
            }
        }
    }
}
