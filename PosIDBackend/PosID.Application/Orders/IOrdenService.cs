namespace PosID.Application.Orders;

public interface IOrdenService
{
    Task<OrdenResponse> CrearAsync(CrearOrdenRequest request, CancellationToken cancellationToken);
    Task<OrdenResponse> ObtenerAsync(int id, CancellationToken cancellationToken);
    Task<OrdenResponse> RegistrarPagoAsync(int ordenId, RegistrarPagoRequest request, CancellationToken cancellationToken);
    Task CancelarAsync(int ordenId, CancellationToken cancellationToken);
}
