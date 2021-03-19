using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;
using Product.API.Models;
using System.Data;
using System.Threading.Tasks;

namespace Product.API.DbConnection
{
    /// <summary>
    /// 
    /// </summary>
    public class SqliteDbConnectionFactory : IDbConnectionFactory
    {
        private readonly IOptions<SqliteDatabaseSettings> _options;

        /// <summary>
        /// 
        /// </summary>
        public SqliteDbConnectionFactory(IOptions<SqliteDatabaseSettings> options)
        {
            _options = options;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IDbConnection> CreateConnectionAsync()
        {
            var conn = new SqliteConnection(_options.Value.ConnectionString);
            await conn.OpenAsync();
            return conn;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="databaseType"></param>
        /// <returns></returns>
        public bool Support(DatabaseType databaseType)
        {
            return databaseType == DatabaseType.Sqlite;
        }
    }
}
