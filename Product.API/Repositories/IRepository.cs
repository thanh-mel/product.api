using System;
using System.Linq;
using System.Threading.Tasks;

namespace Product.API.Repositories
{
    /// <summary>
    /// The generic repository that has basic CRUD operations
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public interface IRepository<TModel>
    {
        /// <summary>
        /// Create a new TModel
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<TModel> CreateAsync(TModel model);
        /// <summary>
        /// Update the existing the TModel
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task UpdateAsync(TModel model);
        /// <summary>
        /// Delete the TModel
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteAsync(string id);
        /// <summary>
        /// Get the TModel by Guid
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TModel> GetAsync(Guid id);
        /// <summary>
        /// Get all TModels 
        /// </summary>
        /// <returns></returns>
        Task<IQueryable<TModel>> GetAllAsync();
    }
}
