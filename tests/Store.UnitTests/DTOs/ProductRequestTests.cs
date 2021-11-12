using Store.ApplicationCore.DTOs;
using Xunit;

namespace Store.UnitTests.DTOs
{
    public class ProductRequestTests : BaseTests
    {
        [Theory]
        [InlineData("Test", "Description", 0.02, 0)]
        [InlineData("Test", null, 0.02, 1)]
        [InlineData(null, null, 0.02, 2)]
        [InlineData(null, null, -1, 3)]
        public void ValidateModel_CreateProductRequest_ReturnsCorrectNumberOfErrors(string name, string description, double price, int numberExpectedErrors)
        {
            var request = new CreateProductRequest
            {
                Name = name,
                Description = description,
                Price = price
            };

            var errorList = ValidateModel(request);

            Assert.Equal(numberExpectedErrors, errorList.Count);
        }

        [Theory]
        [InlineData("Test", "Description", 0.02, 4, 0)]
        [InlineData("Test", null, 0.02, 9, 1)]
        [InlineData(null, null, 0.02, 1, 2)]
        [InlineData(null, null, -1, 8, 3)]
        [InlineData(null, null, -1, 200, 4)]
        public void ValidateModel_UpdateProductRequest_ReturnsCorrectNumberOfErrors(string name, string description, double price, int stock, int numberExpectedErrors)
        {
            var request = new UpdateProductRequest
            {
                Name = name,
                Description = description,
                Price = price,
                Stock = stock
            };

            var errorList = ValidateModel(request);

            Assert.Equal(numberExpectedErrors, errorList.Count);
        }
    }
}