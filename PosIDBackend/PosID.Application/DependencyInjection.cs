using Microsoft.Extensions.DependencyInjection;
using PosID.Application.Orders;

namespace PosID.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IOrdenService, OrdenService>();
        return services;
    }
}
