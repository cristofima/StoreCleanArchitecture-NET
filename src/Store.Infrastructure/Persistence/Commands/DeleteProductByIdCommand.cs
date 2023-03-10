using MediatR;

namespace Store.Infrastructure.Persistence.Commands
{
    public class DeleteProductByIdCommand : IRequest
    {
        public int Id { get; set; }

        public DeleteProductByIdCommand(int id)
        {
            Id = id;
        }
    }
}