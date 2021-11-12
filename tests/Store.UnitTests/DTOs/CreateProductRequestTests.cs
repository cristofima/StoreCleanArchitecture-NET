using Store.ApplicationCore.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Store.UnitTests.DTOs
{
    public class CreateProductRequestTests : BaseTests
    {
        [Theory]
        [InlineData("Test", "Description", 0.02, 0)]
        [InlineData("Test", null, 0.02, 1)]
        [InlineData(null, null, 0.02, 2)]
        [InlineData(null, null, -1, 3)]
        public void ValidateModel_ReturnsCorrectNumberOfErrors(string name, string description, double price, int numErrosExpected)
        {
            var request = new CreateProductRequest
            {
                Name = name,
                Description = description,
                Price = price
            };

            var errorList = ValidateModel(request);

            Assert.Equal(numErrosExpected, errorList.Count);
        }
    }
}
