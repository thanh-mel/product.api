using Product.API.Repositories.DataAccessModels;
using System.Collections.Generic;

namespace Product.API.Repositories.Mappers
{
    /// <summary>
    /// 
    /// </summary>
    public interface IProductMapper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        List<Models.Product> Map(List<ProductDataAccessModel> model);
    }
}
