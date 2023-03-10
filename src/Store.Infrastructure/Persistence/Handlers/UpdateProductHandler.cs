using AutoMapper;
using MediatR;
using Store.ApplicationCore.DTOs;
using Store.ApplicationCore.Exceptions;
using Store.ApplicationCore.Utils;
using Store.Infrastructure.Persistence.Commands;
using Store.Infrastructure.Persistence.Contexts;
using System.Threading;
using System.Threading.Tasks;

namespace Store.Infrastructure.Persistence.Handlers
{
    public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, SingleProductResponse>
    {
        private readonly StoreContext storeContext;
        private readonly IMapper mapper;

        public UpdateProductHandler(StoreContext storeContext, IMapper mapper)
        {
            this.storeContext = storeContext;
            this.mapper = mapper;
        }

        public async Task<SingleProductResponse> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await this.storeContext.Products.FindAsync(request.Id);
            if (product != null)
            {
                product.Name = request.Request.Name;
                product.Description = request.Request.Description;
                product.Price = request.Request.Price;
                product.Stock = request.Request.Stock;
                product.UpdatedAt = DateUtil.GetCurrentDate();

                this.storeContext.Products.Update(product);
                await this.storeContext.SaveChangesAsync();

                return this.mapper.Map<SingleProductResponse>(product);
            }

            throw new NotFoundException();
        }
    }
}