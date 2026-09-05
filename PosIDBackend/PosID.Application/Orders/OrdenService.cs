using PosID.Application.Abstractions;
using PosID.Application.Exceptions;
using PosID.Core.Common;
using PosID.Core.Entities;

namespace PosID.Application.Orders;

public sealed class OrdenService(IOrdenRepository ordenes, ICatalogoRepository catalogo) : IOrdenService
{
    public async Task<OrdenResponse> CrearAsync(CrearOrdenRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.NumeroInterno)) throw new DomainRuleViolationException("El número interno es obligatorio.");
        if (request.Lineas is null || request.Lineas.Count == 0) throw new DomainRuleViolationException("La orden debe contener al menos una línea.");
        if (await ordenes.NumeroInternoExistsAsync(request.NumeroInterno, cancellationToken)) throw new ConflictException("El número interno ya está en uso.");
        var cliente = await catalogo.GetClienteActivoAsync(request.ClienteId, cancellationToken) ?? throw new NotFoundException("cliente activo", request.ClienteId);
        var usuario = await catalogo.GetUsuarioActivoAsync(request.CreadorId, cancellationToken) ?? throw new NotFoundException("usuario activo", request.CreadorId);
        if (usuario.SucursalId != request.SucursalId) throw new DomainRuleViolationException("El creador debe pertenecer a la sucursal de la orden.");
        var orden = new Orden { NumeroInterno = request.NumeroInterno.Trim(), ClienteId = cliente.Id, SucursalId = request.SucursalId, CreadorId = usuario.Id, FechaPrometidaEntrega = request.FechaPrometidaEntrega };
        foreach (var linea in request.Lineas)
        {
            var item = await catalogo.GetServicioProductoActivoAsync(linea.ServicioProductoId, cancellationToken) ?? throw new NotFoundException("servicio o producto activo", linea.ServicioProductoId);
            orden.AddLinea(item.Id, linea.Cantidad, item.Precio, item.TasaITBIS, linea.DescuentoMonto ?? 0, linea.MotivoDescuento);
        }
        await ordenes.AddAsync(orden, cancellationToken);
        await ordenes.SaveChangesAsync(cancellationToken);
        return Map(orden);
    }

    public async Task<OrdenResponse> ObtenerAsync(int id, CancellationToken cancellationToken) => Map(await ordenes.GetDetailAsync(id, cancellationToken) ?? throw new NotFoundException("orden", id));

    public async Task<OrdenResponse> RegistrarPagoAsync(int ordenId, RegistrarPagoRequest request, CancellationToken cancellationToken)
    {
        var orden = await ordenes.GetForUpdateAsync(ordenId, cancellationToken) ?? throw new NotFoundException("orden", ordenId);
        var metodo = await catalogo.GetMetodoPagoActivoAsync(request.MetodoPagoId, cancellationToken) ?? throw new NotFoundException("método de pago activo", request.MetodoPagoId);
        var usuario = await catalogo.GetUsuarioActivoAsync(request.UsuarioId, cancellationToken) ?? throw new NotFoundException("usuario activo", request.UsuarioId);
        if (usuario.SucursalId != orden.SucursalId) throw new DomainRuleViolationException("El usuario que registra el pago debe pertenecer a la sucursal de la orden.");
        orden.RegistrarPago(metodo.Id, usuario.Id, request.Monto, request.ReferenciaExterna, DateTimeOffset.UtcNow);
        await ordenes.SaveChangesAsync(cancellationToken);
        return Map(orden);
    }

    public async Task CancelarAsync(int ordenId, CancellationToken cancellationToken)
    {
        var orden = await ordenes.GetForUpdateAsync(ordenId, cancellationToken) ?? throw new NotFoundException("orden", ordenId);
        orden.Cancelar();
        await ordenes.SaveChangesAsync(cancellationToken);
    }

    private static OrdenResponse Map(Orden orden) => new(orden.Id, orden.NumeroInterno, orden.Estado.ToString(), orden.ClienteId, orden.SucursalId, orden.Subtotal, orden.DescuentoTotal, orden.ImpuestoTotal, orden.Total, orden.SaldoPendiente,
        orden.Lineas.Select(x => new OrdenLineaResponse(x.ServicioProductoId, x.Cantidad, x.PrecioUnitario, x.Subtotal, x.DescuentoMonto, x.TasaITBISAplicada, x.MontoITBIS, x.TotalLinea)).ToArray(),
        orden.Pagos.Select(x => new PagoResponse(x.Id, x.Monto, x.Estado.ToString(), x.FechaPago, x.ReferenciaExterna)).ToArray());
}
