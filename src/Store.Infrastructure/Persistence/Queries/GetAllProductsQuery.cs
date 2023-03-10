using MediatR;
using Store.ApplicationCore.DTOs;
using System.Collections.Generic;

namespace Store.Infrastructure.Persistence.Queries
{
    public class GetAllProductsQuery : IRequest<List<ProductResponse>>
    {
    }
}