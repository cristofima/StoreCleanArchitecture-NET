using AutoMapper;
using MediatR;
using Store.ApplicationCore.Exceptions;
using Store.Infrastructure.Persistence.Commands;
using Store.Infrastructure.Persistence.Contexts;
using System.Threading;
using System.Threading.Tasks;

namespace Store.Infrastructure.Persistence.Handlers
{
    public class DeleteProductByIdHandler : IRequestHandler<DeleteProductByIdCommand>
    {
        private readonly StoreContext storeContext;

        public DeleteProductByIdHandler(StoreContext storeContext)
        {
            this.storeContext = storeContext;
        }

        public async Task Handle(DeleteProductByIdCommand request, CancellationToken cancellationToken)
        {
            var product = await this.storeContext.Products.FindAsync(request.Id);
            if (product != null)
            {
                this.storeContext.Products.Remove(product);
                await this.storeContext.SaveChangesAsync();
            }
            else
            {
                throw new NotFoundException();
            }
        }
    }
}