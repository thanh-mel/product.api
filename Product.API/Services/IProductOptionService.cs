using Product.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Product.API.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface IProductOptionService
    {
        /// <summary>
        /// Add options to a specified product
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        Task<List<ProductOption>> AddManyProductOptionsAsync(string productId, List<ProductOption> options);
        /// <summary>
        /// Delete options from a product
        /// </summary>
        /// <param name="optionIds"></param>
        /// <returns></returns>
        Task DeleteManyProductOptionsAsync(List<string> optionIds);
    }
}
