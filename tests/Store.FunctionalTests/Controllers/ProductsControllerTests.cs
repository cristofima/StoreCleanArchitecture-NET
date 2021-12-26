using Newtonsoft.Json;
using Store.ApplicationCore.DTOs;
using Store.FunctionalTests.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Store.FunctionalTests.Controllers
{
    public class ProductsControllerTests : BaseControllerTests
    {
        public ProductsControllerTests(CustomWebApplicationFactory<Program> factory) : base(factory)
        {
        }

        [Fact]
        public async Task GetProducts_ReturnsAllRecords()
        {
            var client = this.GetNewClient();
            var response = await client.GetAsync("/api/Products");
            response.EnsureSuccessStatusCode();

            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<IEnumerable<ProductResponse>>(stringResponse).ToList();
            var statusCode = response.StatusCode.ToString();

            Assert.Equal("OK", statusCode);
            Assert.True(result.Count == 10);
        }

        [Fact]
        public async Task GetProductById_ProductExists_ReturnsCorrectProduct()
        {
            var productId = 5;
            var client = this.GetNewClient();
            var response = await client.GetAsync($"/api/Products/{productId}");
            response.EnsureSuccessStatusCode();

            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<SingleProductResponse>(stringResponse);
            var statusCode = response.StatusCode.ToString();

            Assert.Equal("OK", statusCode);
            Assert.Equal(productId, result.Id);
            Assert.NotNull(result.Name);
            Assert.True(result.Price > 0);
            Assert.True(result.Stock > 0);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(20)]
        public async Task GetProductById_ProductDoesntExist_ReturnsNotFound(int productId)
        {
            var client = this.GetNewClient();
            var response = await client.GetAsync($"/api/Products/{productId}");

            var statusCode = response.StatusCode.ToString();

            Assert.Equal("NotFound", statusCode);
        }

        [Fact]
        public async Task PostProduct_ReturnsCreatedProduct()
        {
            var client = this.GetNewClient();

            // Create product

            var request = new CreateProductRequest
            {
                Description = "Description",
                Name = "Test product",
                Price = 25.3
            };

            var stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            var response1 = await client.PostAsync("/api/Products", stringContent);
            response1.EnsureSuccessStatusCode();

            var stringResponse1 = await response1.Content.ReadAsStringAsync();
            var createdProduct = JsonConvert.DeserializeObject<SingleProductResponse>(stringResponse1);
            var statusCode1 = response1.StatusCode.ToString();

            Assert.Equal("Created", statusCode1);

            // Get created product

            var response2 = await client.GetAsync($"/api/Products/{createdProduct.Id}");
            response2.EnsureSuccessStatusCode();

            var stringResponse2 = await response2.Content.ReadAsStringAsync();
            var result2 = JsonConvert.DeserializeObject<SingleProductResponse>(stringResponse2);
            var statusCode2 = response2.StatusCode.ToString();

            Assert.Equal("OK", statusCode2);
            Assert.Equal(createdProduct.Id, result2.Id);
            Assert.Equal(createdProduct.Name, result2.Name);
            Assert.Equal(createdProduct.Description, result2.Description);
            Assert.Equal(createdProduct.Stock, result2.Stock);
        }

        [Fact]
        public async Task PostProduct_InvalidData_ReturnsErrors()
        {
            var client = this.GetNewClient();

            // Create product

            var request = new CreateProductRequest
            {
                Description = "Description",
                Name = null,
                Price = 0
            };

            var stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/api/Products", stringContent);

            var stringResponse = await response.Content.ReadAsStringAsync();
            var badRequest = JsonConvert.DeserializeObject<BadRequestModel>(stringResponse);
            var statusCode = response.StatusCode.ToString();

            Assert.Equal("BadRequest", statusCode);
            Assert.NotNull(badRequest.Title);
            Assert.NotNull(badRequest.Errors);
            Assert.Equal(2, badRequest.Errors.Count);
            Assert.Contains(badRequest.Errors.Keys, k => k == "Name");
            Assert.Contains(badRequest.Errors.Keys, k => k == "Price");
        }


        [Fact]
        public async Task PutProduct_ReturnsUpdatedProduct()
        {
            var client = this.GetNewClient();

            // Update product

            var productId = 6;
            var request = new UpdateProductRequest
            {
                Description = "Description",
                Name = "Test product",
                Price = 17.67,
                Stock = 94
            };

            var stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            var response1 = await client.PutAsync($"/api/Products/{productId}", stringContent);
            response1.EnsureSuccessStatusCode();

            var stringResponse1 = await response1.Content.ReadAsStringAsync();
            var updatedProduct = JsonConvert.DeserializeObject<SingleProductResponse>(stringResponse1);
            var statusCode1 = response1.StatusCode.ToString();

            Assert.Equal("OK", statusCode1);

            // Get updated product

            var response2 = await client.GetAsync($"/api/Products/{updatedProduct.Id}");
            response2.EnsureSuccessStatusCode();

            var stringResponse2 = await response2.Content.ReadAsStringAsync();
            var result2 = JsonConvert.DeserializeObject<SingleProductResponse>(stringResponse2);
            var statusCode2 = response2.StatusCode.ToString();

            Assert.Equal("OK", statusCode2);
            Assert.Equal(updatedProduct.Id, result2.Id);
            Assert.Equal(updatedProduct.Name, result2.Name);
            Assert.Equal(updatedProduct.Description, result2.Description);
            Assert.Equal(updatedProduct.Stock, result2.Stock);
        }

        [Fact]
        public async Task DeleteProductById_ReturnsNoContent()
        {
            var client = this.GetNewClient();
            var productId = 5;

            // Delete product

            var response1 = await client.DeleteAsync($"/api/Products/{productId}");

            var statusCode1 = response1.StatusCode.ToString();

            Assert.Equal("NoContent", statusCode1);

            // Get deleted product

            var response2 = await client.GetAsync($"/api/Products/{productId}");

            var statusCode2 = response2.StatusCode.ToString();

            Assert.Equal("NotFound", statusCode2);
        }
    }
}