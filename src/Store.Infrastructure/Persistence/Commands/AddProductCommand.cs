using MediatR;
using Store.ApplicationCore.DTOs;

namespace Store.Infrastructure.Persistence.Commands
{
    public class AddProductCommand : IRequest<SingleProductResponse>
    {
        public CreateProductRequest Request { get; set; }

        public AddProductCommand(CreateProductRequest request)
        {
            Request = request;
        }
    }
}