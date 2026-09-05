using PosID.Core.Entities;

namespace PosID.Application.Abstractions;

public interface IOrdenRepository
{
    Task<Orden?> GetForUpdateAsync(int id, CancellationToken cancellationToken);
    Task<Orden?> GetDetailAsync(int id, CancellationToken cancellationToken);
    Task<bool> NumeroInternoExistsAsync(string numeroInterno, CancellationToken cancellationToken);
    Task AddAsync(Orden orden, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
