using MediatR;
using Store.ApplicationCore.DTOs;

namespace Store.Infrastructure.Persistence.Commands
{
    public class UpdateProductCommand : IRequest<SingleProductResponse>
    {
        public int Id { get; set; }
        public UpdateProductRequest Request { get; set; }

        public UpdateProductCommand(int id, UpdateProductRequest request)
        {
            Id = id;
            Request = request;
        }
    }
}