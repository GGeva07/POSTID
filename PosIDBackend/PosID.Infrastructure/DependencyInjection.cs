using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PosID.Application.Abstractions;
using PosID.Infrastructure.Persistence;
using PosID.Infrastructure.Repositories;

namespace PosID.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("PosID") ?? throw new InvalidOperationException("No se configuró ConnectionStrings:PosID.");
        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
        services.AddScoped<IOrdenRepository, OrdenRepository>();
        services.AddScoped<ICatalogoRepository, CatalogoRepository>();
        return services;
    }
}
