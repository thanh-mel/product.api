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
        /// <param name="options"></param>
        /// <returns></returns>
        Task AddProductOptionsAsync(List<ProductOption> options);
        /// <summary>
        /// Delete options from a product
        /// </summary>
        /// <param name="optionIds"></param>
        /// <returns></returns>
        Task DeleteProductOptionsAsync(List<string> optionIds);
    }
}
