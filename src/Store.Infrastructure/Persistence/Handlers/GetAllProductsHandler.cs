using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Store.ApplicationCore.DTOs;
using Store.Infrastructure.Persistence.Contexts;
using Store.Infrastructure.Persistence.Queries;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Store.Infrastructure.Persistence.Handlers
{
    public class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, List<ProductResponse>>
    {
        private readonly StoreContext storeContext;
        private readonly IMapper mapper;

        public GetAllProductsHandler(StoreContext storeContext, IMapper mapper)
        {
            this.storeContext = storeContext;
            this.mapper = mapper;
        }

        public async Task<List<ProductResponse>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            return await this.storeContext.Products.Select(p => this.mapper.Map<ProductResponse>(p)).ToListAsync();
        }
    }
}