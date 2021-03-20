using Product.API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Product.API.DbConnection
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class DbConnectionStrategy : IDbConnectionStrategy
    {
        private readonly IEnumerable<IDbConnectionFactory> _factories;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="factories"></param>
        public DbConnectionStrategy(IEnumerable<IDbConnectionFactory> factories)
        {
            _factories = factories;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<IDbConnection> CreateConnectionAsync(DatabaseType type)
        {
            var factory = _factories.FirstOrDefault(x => x.Support(type));
            if (factory == null)
            {
                throw new NotImplementedException($"No database connection factory found for database type {type}");
            }
            return await factory.CreateConnectionAsync();
        }
    }
}
