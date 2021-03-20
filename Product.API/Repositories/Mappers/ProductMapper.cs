using Product.API.Repositories.DataAccessModels;
using System.Collections.Generic;
using System.Linq;

namespace Product.API.Repositories.Mappers
{
    /// <summary>
    /// 
    /// </summary>
    public class ProductMapper : IProductMapper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<Models.Product> Map(List<ProductDataAccessModel> model)
        {
            var products = new List<Models.Product>();
            if (model == null)
            {
                return products;
            }

            var productIds = model.Select(x => x.ProductId).Distinct();

            foreach (var productId in productIds)
            {
                var selectedProduct = model.First(x => x.ProductId == productId);

                var product = new Models.Product()
                {
                    Id = productId,
                    Name = selectedProduct.ProductName,
                    Description = selectedProduct.ProductDesc,
                    Price = selectedProduct.Price,
                    DeliveryPrice = selectedProduct.DeliveryPrice,
                    Options = new List<Models.ProductOption>()
                };

                products.Add(product);

                var options = model.Where(x => x.ProductId == productId);

                foreach (var option in options)
                {
                    product.Options.Add(new Models.ProductOption()
                    {
                        Id = option.OptionId,
                        Name = option.OptionName,
                        Description = option.OptionDesc
                    });
                }
            }
            return products;
        }
    }
}
