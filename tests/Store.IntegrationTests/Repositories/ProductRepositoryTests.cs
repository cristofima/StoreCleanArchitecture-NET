using AutoMapper;
using Store.ApplicationCore.DTOs;
using Store.ApplicationCore.Exceptions;
using Store.ApplicationCore.Mappings;
using Store.Infrastructure.Persistence.Commands;
using Store.Infrastructure.Persistence.Handlers;
using Store.Infrastructure.Persistence.Queries;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Store.IntegrationTests.Repositories
{
    public class ProductRepositoryTests : IClassFixture<SharedDatabaseFixture>
    {
        private readonly IMapper _mapper;
        private SharedDatabaseFixture Fixture { get; }

        public ProductRepositoryTests(SharedDatabaseFixture fixture)
        {
            Fixture = fixture;

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<GeneralProfile>();
            });

            _mapper = configuration.CreateMapper();
        }

        [Fact]
        public async Task GetAllProductsHandler_ReturnsAllProducts()
        {
            using (var context = Fixture.CreateContext())
            {
                var handler = new GetAllProductsHandler(context, _mapper);

                var products = await handler.Handle(new GetAllProductsQuery(), CancellationToken.None);

                Assert.Equal(10, products.Count);
            }
        }

        [Fact]
        public async Task GetProductByIdHandler_ProductDoesntExist_ThrowsNotFoundException()
        {
            using (var context = Fixture.CreateContext())
            {
                var handler = new GetProductByIdHandler(context, _mapper);
                var productId = 56;

                var action = async () => await handler.Handle(new GetProductByIdQuery(productId), CancellationToken.None);
                await Assert.ThrowsAsync<NotFoundException>(action);
            }
        }

        [Fact]
        public async Task AddProductHandler_SavesCorrectData()
        {
            using (var transaction = Fixture.Connection.BeginTransaction())
            {
                var productId = 0;

                var request = new CreateProductRequest
                {
                    Name = "Product 11",
                    Description = "Description 11",
                    Price = 5
                };

                using (var context = Fixture.CreateContext(transaction))
                {
                    var handler = new AddProductHandler(context, _mapper);

                    var product = await handler.Handle(new AddProductCommand(request), CancellationToken.None);
                    productId = product.Id;
                }

                using (var context = Fixture.CreateContext(transaction))
                {
                    var handler = new GetProductByIdHandler(context, _mapper);

                    var product = await handler.Handle(new GetProductByIdQuery(productId), CancellationToken.None);

                    Assert.NotNull(product);
                    Assert.Equal(request.Name, product.Name);
                    Assert.Equal(request.Description, product.Description);
                    Assert.Equal(request.Price, product.Price);
                    Assert.Equal(0, product.Stock);
                }
            }
        }

        [Fact]
        public async Task UpdateProductHandler_SavesCorrectData()
        {
            using (var transaction = Fixture.Connection.BeginTransaction())
            {
                var productId = 1;

                var request = new UpdateProductRequest
                {
                    Name = "Product 1",
                    Description = "Description 1",
                    Price = 5.12,
                    Stock = 23
                };

                using (var context = Fixture.CreateContext(transaction))
                {
                    var handler = new UpdateProductHandler(context, _mapper);

                    await handler.Handle(new UpdateProductCommand(productId, request), CancellationToken.None);
                }

                using (var context = Fixture.CreateContext(transaction))
                {
                    var handler = new GetProductByIdHandler(context, _mapper);

                    var product = await handler.Handle(new GetProductByIdQuery(productId), CancellationToken.None);

                    Assert.NotNull(product);
                    Assert.Equal(request.Name, product.Name);
                    Assert.Equal(request.Description, product.Description);
                    Assert.Equal(request.Price, product.Price);
                    Assert.Equal(request.Stock, product.Stock);
                }
            }
        }

        [Fact]
        public async Task UpdateProductHandler_ProductDoesntExist_ThrowsNotFoundException()
        {
            var productId = 15;

            var request = new UpdateProductRequest
            {
                Name = "Product 15",
                Description = "Description 15",
                Price = 5.12,
                Stock = 23
            };

            using (var context = Fixture.CreateContext())
            {
                var handler = new UpdateProductHandler(context, _mapper);
                var action = async() => await handler.Handle(new UpdateProductCommand(productId, request), CancellationToken.None);

                await Assert.ThrowsAsync<NotFoundException>(action);
            }
        }

        [Fact]
        public async Task DeleteProductByIdHandler_EnsuresProductIsDeleted()
        {
            using (var transaction = Fixture.Connection.BeginTransaction())
            {
                var productId = 2;

                using (var context = Fixture.CreateContext(transaction))
                {
                    var handler = new DeleteProductByIdHandler(context);

                    await handler.Handle(new DeleteProductByIdCommand(productId), CancellationToken.None);
                }

                using (var context = Fixture.CreateContext(transaction))
                {
                    var handler = new GetProductByIdHandler(context, _mapper);
                    var action = async() => await handler.Handle(new GetProductByIdQuery(productId), CancellationToken.None);

                    await Assert.ThrowsAsync<NotFoundException>(action);
                }
            }
        }

        [Fact]
        public async Task DeleteProductByIdHandler_ProductDoesntExist_ThrowsNotFoundException()
        {
            var productId = 48;

            using (var context = Fixture.CreateContext())
            {
                var handler = new DeleteProductByIdHandler(context);
                var action = async () => await handler.Handle(new DeleteProductByIdCommand(productId), CancellationToken.None);

                await Assert.ThrowsAsync<NotFoundException>(action);
            }
        }
    }
}