using MediatR;
using Store.ApplicationCore.DTOs;

namespace Store.Infrastructure.Persistence.Queries
{
    public class GetProductByIdQuery : IRequest<SingleProductResponse>
    {
        public int Id { get; }

        public GetProductByIdQuery(int id)
        {
            Id = id;
        }
    }
}