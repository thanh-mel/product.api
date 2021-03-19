using Product.API.Models;
using System.Data;
using System.Threading.Tasks;

namespace Product.API.DbConnection
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDbConnectionStrategy
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<IDbConnection> CreateConnectionAsync(DatabaseType type);
    }
}
