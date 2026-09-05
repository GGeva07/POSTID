using PosID.Core.Common;
using PosID.Core.Enums;

namespace PosID.Core.Entities;

public class Orden : AuditableEntity, ISoftDelete
{
    public int Id { get; set; }
    public string NumeroInterno { get; set; } = string.Empty;
    public EstadoOrden Estado { get; private set; } = EstadoOrden.Abierta;
    public int ClienteId { get; set; }
    public Cliente Cliente { get; set; } = null!;
    public int SucursalId { get; set; }
    public Sucursal Sucursal { get; set; } = null!;
    public int CreadorId { get; set; }
    public Usuario Creador { get; set; } = null!;
    public decimal Subtotal { get; private set; }
    public decimal DescuentoTotal { get; private set; }
    public decimal ImpuestoTotal { get; private set; }
    public decimal Total { get; private set; }
    public DateTimeOffset? FechaPrometidaEntrega { get; set; }
    public DateTimeOffset? FechaEntregada { get; set; }
    public ICollection<OrdenLinea> Lineas { get; set; } = new List<OrdenLinea>();
    public ICollection<Pago> Pagos { get; set; } = new List<Pago>();
    public ICollection<DocumentoFiscal> DocumentosFiscales { get; set; } = new List<DocumentoFiscal>();
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    public string? DeletedBy { get; set; }

    public void AddLinea(int servicioProductoId, int cantidad, decimal precioUnitario, decimal tasaItbis, decimal descuentoMonto = 0m, string? motivoDescuento = null)
    {
        EnsureModificable();
        Lineas.Add(OrdenLinea.Create(servicioProductoId, cantidad, precioUnitario, tasaItbis, descuentoMonto, motivoDescuento));
        RecalcularTotales();
    }

    public void RegistrarPago(int metodoPagoId, int usuarioId, decimal monto, string? referenciaExterna, DateTimeOffset fechaPago)
    {
        if (Estado is EstadoOrden.Cancelada or EstadoOrden.Pagada)
            throw new DomainRuleViolationException("No se pueden registrar pagos para una orden cancelada o ya pagada.");
        if (monto <= 0) throw new DomainRuleViolationException("El monto del pago debe ser mayor que cero.");
        if (monto > SaldoPendiente) throw new DomainRuleViolationException("El pago no puede exceder el saldo pendiente.");

        Pagos.Add(new Pago { MetodoPagoId = metodoPagoId, UsuarioId = usuarioId, Monto = monto, FechaPago = fechaPago, Estado = EstadoPago.Confirmado, ReferenciaExterna = referenciaExterna });
        Estado = SaldoPendiente == 0m ? EstadoOrden.Pagada : EstadoOrden.PendientePago;
    }

    public void Cancelar()
    {
        if (Estado is EstadoOrden.Pagada or EstadoOrden.Entregada)
            throw new DomainRuleViolationException("Una orden pagada o entregada no puede cancelarse; requiere un flujo compensatorio.");
        if (Pagos.Any(p => p.Estado == EstadoPago.Confirmado))
            throw new DomainRuleViolationException("No se puede cancelar una orden con pagos confirmados.");
        Estado = EstadoOrden.Cancelada;
    }

    public decimal SaldoPendiente => decimal.Round(Total - Pagos.Where(p => p.Estado == EstadoPago.Confirmado).Sum(p => p.Monto), 2, MidpointRounding.AwayFromZero);

    private void EnsureModificable()
    {
        if (Estado != EstadoOrden.Abierta) throw new DomainRuleViolationException("Solo se pueden modificar órdenes abiertas.");
    }

    private void RecalcularTotales()
    {
        Subtotal = Lineas.Sum(x => x.Subtotal);
        DescuentoTotal = Lineas.Sum(x => x.DescuentoMonto);
        ImpuestoTotal = Lineas.Sum(x => x.MontoITBIS);
        Total = Lineas.Sum(x => x.TotalLinea);
    }
}
