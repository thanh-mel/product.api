using Product.API.Models;
using System.Data;
using System.Threading.Tasks;

namespace Product.API.DbConnection
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDbConnectionFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="databaseType"></param>
        /// <returns></returns>
        bool Support(DatabaseType databaseType);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<IDbConnection> CreateConnectionAsync();
    }
}
