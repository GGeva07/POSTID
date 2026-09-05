using Microsoft.AspNetCore.Diagnostics;
using PosID.Application;
using PosID.Application.Exceptions;
using PosID.Application.Orders;
using PosID.Core.Common;
using PosID.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

var app = builder.Build();
if (app.Environment.IsDevelopment()) app.MapOpenApi();
app.UseExceptionHandler(exceptionApp => exceptionApp.Run(async context =>
{
    var error = context.Features.Get<IExceptionHandlerFeature>()?.Error;
    var status = error switch
    {
        NotFoundException => StatusCodes.Status404NotFound,
        ConflictException => StatusCodes.Status409Conflict,
        DomainRuleViolationException => StatusCodes.Status422UnprocessableEntity,
        _ => StatusCodes.Status500InternalServerError
    };
    await Results.Problem(statusCode: status, title: status == 500 ? "Ocurrió un error inesperado." : error?.Message).ExecuteAsync(context);
}));
app.UseHttpsRedirection();

var ordenes = app.MapGroup("/api/ordenes").WithTags("Órdenes");
ordenes.MapPost("/", async (CrearOrdenRequest request, IOrdenService service, CancellationToken ct) =>
{
    var orden = await service.CrearAsync(request, ct);
    return Results.Created($"/api/ordenes/{orden.Id}", orden);
});
ordenes.MapGet("/{id:int}", async (int id, IOrdenService service, CancellationToken ct) => Results.Ok(await service.ObtenerAsync(id, ct)));
ordenes.MapPost("/{id:int}/pagos", async (int id, RegistrarPagoRequest request, IOrdenService service, CancellationToken ct) => Results.Ok(await service.RegistrarPagoAsync(id, request, ct)));
ordenes.MapPost("/{id:int}/cancelacion", async (int id, IOrdenService service, CancellationToken ct) => { await service.CancelarAsync(id, ct); return Results.NoContent(); });
app.Run();
