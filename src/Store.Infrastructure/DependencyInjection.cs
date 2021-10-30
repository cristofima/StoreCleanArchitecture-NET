using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Store.ApplicationCore.Interfaces;
using Store.Infrastructure.Persistence.Contexts;
using Store.Infrastructure.Persistence.Repositories;

namespace Store.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var defaultConnectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<StoreContext>(options =>
               options.UseSqlServer(defaultConnectionString));

            services.AddScoped<IProductRepository, ProductRepository>();

            return services;
        }
    }
}