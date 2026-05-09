using Ecom.Application.Interfaces;
using Ecom.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Ecom.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IProductGroupingService, ProductGroupingService>();
        services.AddScoped<IProductPackingService, ProductPackingService>();

        return services;
    }
}