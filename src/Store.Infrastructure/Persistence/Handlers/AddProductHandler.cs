using AutoMapper;
using MediatR;
using Store.ApplicationCore.DTOs;
using Store.ApplicationCore.Entities;
using Store.ApplicationCore.Utils;
using Store.Infrastructure.Persistence.Commands;
using Store.Infrastructure.Persistence.Contexts;
using System.Threading;
using System.Threading.Tasks;

namespace Store.Infrastructure.Persistence.Handlers
{
    public class AddProductHandler : IRequestHandler<AddProductCommand, SingleProductResponse>
    {
        private readonly StoreContext storeContext;
        private readonly IMapper mapper;

        public AddProductHandler(StoreContext storeContext, IMapper mapper)
        {
            this.storeContext = storeContext;
            this.mapper = mapper;
        }

        public async Task<SingleProductResponse> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            var product = this.mapper.Map<Product>(request.Request);
            product.Stock = 0;
            product.CreatedAt = product.UpdatedAt = DateUtil.GetCurrentDate();

            this.storeContext.Products.Add(product);
            await this.storeContext.SaveChangesAsync();

            return this.mapper.Map<SingleProductResponse>(product);
        }
    }
}