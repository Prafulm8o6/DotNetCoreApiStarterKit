using DotNetCoreApiStarterKit.Domain.Interfaces.Repositories;
using DotNetCoreApiStarterKit.Infrastructure.Repositories;

namespace DotNetCoreApiStarterKit.API.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
