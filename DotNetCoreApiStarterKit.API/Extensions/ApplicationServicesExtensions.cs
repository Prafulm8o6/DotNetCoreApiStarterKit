using DotNetCoreApiStarterKit.Application.Services;
using DotNetCoreApiStarterKit.Domain.Interfaces.Repositories;
using DotNetCoreApiStarterKit.Domain.Interfaces.Services;
using DotNetCoreApiStarterKit.Infrastructure.Repositories;

namespace DotNetCoreApiStarterKit.API.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
