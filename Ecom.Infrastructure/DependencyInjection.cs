using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ecom.Application.Interfaces;
using Ecom.Infrastructure.Data;
using Ecom.Infrastructure.Repositories;
using Ecom.Infrastructure.Services;

namespace Ecom.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlite(
                configuration.GetConnectionString("DefaultConnection")
            );
        });

        services.AddScoped<IUnitOfWork>(provider =>
            provider.GetRequiredService<AppDbContext>());

        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IProductGroupRepository, ProductGroupRepository>();
        services.AddScoped<IExcelImportService, ExcelImportService>();
        return services;
    }
}