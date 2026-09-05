namespace PosID.Application.Orders;

public sealed record CrearOrdenRequest(string NumeroInterno, int ClienteId, int SucursalId, int CreadorId, DateTimeOffset? FechaPrometidaEntrega, IReadOnlyCollection<CrearLineaOrdenRequest> Lineas);
public sealed record CrearLineaOrdenRequest(int ServicioProductoId, int Cantidad, decimal? DescuentoMonto, string? MotivoDescuento);
public sealed record RegistrarPagoRequest(int MetodoPagoId, int UsuarioId, decimal Monto, string? ReferenciaExterna);
public sealed record OrdenLineaResponse(int ServicioProductoId, int Cantidad, decimal PrecioUnitario, decimal Subtotal, decimal DescuentoMonto, decimal TasaITBIS, decimal MontoITBIS, decimal Total);
public sealed record PagoResponse(int Id, decimal Monto, string Estado, DateTimeOffset FechaPago, string? ReferenciaExterna);
public sealed record OrdenResponse(int Id, string NumeroInterno, string Estado, int ClienteId, int SucursalId, decimal Subtotal, decimal DescuentoTotal, decimal ImpuestoTotal, decimal Total, decimal SaldoPendiente, IReadOnlyCollection<OrdenLineaResponse> Lineas, IReadOnlyCollection<PagoResponse> Pagos);
