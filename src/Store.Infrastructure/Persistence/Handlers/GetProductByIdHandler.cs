using AutoMapper;
using MediatR;
using Store.ApplicationCore.DTOs;
using Store.ApplicationCore.Exceptions;
using Store.Infrastructure.Persistence.Contexts;
using Store.Infrastructure.Persistence.Queries;
using System.Threading;
using System.Threading.Tasks;

namespace Store.Infrastructure.Persistence.Handlers
{
    public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, SingleProductResponse>
    {
        private readonly StoreContext storeContext;
        private readonly IMapper mapper;

        public GetProductByIdHandler(StoreContext storeContext, IMapper mapper)
        {
            this.storeContext = storeContext;
            this.mapper = mapper;
        }

        public async Task<SingleProductResponse> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await this.storeContext.Products.FindAsync(request.Id);
            if (product != null)
            {
                return this.mapper.Map<SingleProductResponse>(product);
            }

            throw new NotFoundException();
        }
    }
}