using Microsoft.EntityFrameworkCore;
using PosID.Application.Abstractions;
using PosID.Core.Entities;
using PosID.Infrastructure.Persistence;

namespace PosID.Infrastructure.Repositories;

public sealed class CatalogoRepository(ApplicationDbContext db) : ICatalogoRepository
{
    public Task<Cliente?> GetClienteActivoAsync(int id, CancellationToken ct) => db.Clientes.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id && x.Estado == "Activo", ct);
    public Task<Usuario?> GetUsuarioActivoAsync(int id, CancellationToken ct) => db.Usuarios.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id && x.Estado == "Activo", ct);
    public Task<ServicioProducto?> GetServicioProductoActivoAsync(int id, CancellationToken ct) => db.ServiciosProductos.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id && x.Activo, ct);
    public Task<MetodoPago?> GetMetodoPagoActivoAsync(int id, CancellationToken ct) => db.MetodosPago.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id && x.Activo, ct);
}
