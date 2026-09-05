using Microsoft.EntityFrameworkCore;
using PosID.Application.Abstractions;
using PosID.Core.Entities;
using PosID.Infrastructure.Persistence;

namespace PosID.Infrastructure.Repositories;

public sealed class OrdenRepository(ApplicationDbContext db) : IOrdenRepository
{
    public Task<bool> NumeroInternoExistsAsync(string numeroInterno, CancellationToken cancellationToken) => db.Ordenes.AnyAsync(x => x.NumeroInterno == numeroInterno, cancellationToken);
    public Task AddAsync(Orden orden, CancellationToken cancellationToken) => db.Ordenes.AddAsync(orden, cancellationToken).AsTask();
    public Task<Orden?> GetForUpdateAsync(int id, CancellationToken cancellationToken) => Graph().SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
    public Task<Orden?> GetDetailAsync(int id, CancellationToken cancellationToken) => Graph().AsNoTracking().SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
    public Task SaveChangesAsync(CancellationToken cancellationToken) => db.SaveChangesAsync(cancellationToken);
    private IQueryable<Orden> Graph() => db.Ordenes.Include(x => x.Lineas).Include(x => x.Pagos);
}
