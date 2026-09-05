using PosID.Core.Entities;

namespace PosID.Application.Abstractions;

public interface ICatalogoRepository
{
    Task<Cliente?> GetClienteActivoAsync(int id, CancellationToken cancellationToken);
    Task<Usuario?> GetUsuarioActivoAsync(int id, CancellationToken cancellationToken);
    Task<ServicioProducto?> GetServicioProductoActivoAsync(int id, CancellationToken cancellationToken);
    Task<MetodoPago?> GetMetodoPagoActivoAsync(int id, CancellationToken cancellationToken);
}
