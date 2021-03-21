using System;
using System.Collections.Generic;
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
        /// Create a list of TModel
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        Task CreateManyAsync(IList<TModel> models); 
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
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task DeleteManyAsync(List<string> ids);
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
