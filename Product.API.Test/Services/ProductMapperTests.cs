using NUnit.Framework;
using Product.API.Repositories.DataAccessModels;
using Product.API.Repositories.Mappers;
using System.Collections.Generic;
using System.Linq;

namespace Product.API.Test.Services
{
    [TestFixture]
    class ProductMapperTests
    {
        [Test]
        public void Map_NoProductFound_ReturnEmptyList()
        {
            //Arrange & Act
            var mapper = new ProductMapper();

            var result = mapper.Map(new List<ProductDataAccessModel>());
            var result2 = mapper.Map(null);

            //Assert
            Assert.AreEqual(result.Count, 0);
            Assert.AreEqual(result2.Count, 0);
        }

        [Test]
        public void Map_ProductsFound_ReturnListOfMappedProducts()
        {
            //Arrange
            var productDbModel = new List<ProductDataAccessModel>() 
            { 
                new ProductDataAccessModel()
                {
                    ProductId = "7fb87db7-4c8d-4e19-905d-0d3fb88fb6a4",
                    ProductName = "Test Product",
                    ProductDesc = "Test Product Description",
                    Price = 10m,
                    DeliveryPrice = 1m,
                    OptionId = "6b64c5a8-00f9-4a57-a62b-54198117ef54",
                    OptionName = "Option 1",
                    OptionDesc = "Option 1 description"
                },
                new ProductDataAccessModel()
                {
                    ProductId = "7fb87db7-4c8d-4e19-905d-0d3fb88fb6a4",
                    ProductName = "Test Product",
                    ProductDesc = "Test Product Description",
                    Price = 10m,
                    DeliveryPrice = 1m,
                    OptionId = "15601605-3d80-452d-8170-89368bad5f0a",
                    OptionName = "Option 2",
                    OptionDesc = "Option 2 description"
                },
                new ProductDataAccessModel()
                {
                    ProductId = "7fb87db7-4c8d-4e19-905d-0d3fb88fb6a4",
                    ProductName = "Test Product",
                    ProductDesc = "Test Product Description",
                    Price = 10m,
                    DeliveryPrice = 1m,
                    OptionId = "b898d8c3-635b-4d1f-96c9-78bcf5f92d6f",
                    OptionName = "Option 3",
                    OptionDesc = "Option 3 description"
                },
            };

            //Act
            var mapper = new ProductMapper();
            var result = mapper.Map(productDbModel);

            //Assert
            Assert.AreEqual(result.Count, 1);
            Assert.AreEqual(result.First().Options.Count, 3);
        }
    }
}
