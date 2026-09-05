using PosID.Core.Common;

namespace PosID.Core.Entities;

public class OrdenLinea : AuditableEntity, ISoftDelete
{
    public int Id { get; set; }
    public int OrdenId { get; set; }
    public Orden Orden { get; set; } = null!;
    public int ServicioProductoId { get; set; }
    public ServicioProducto ServicioProducto { get; set; } = null!;
    public int Cantidad { get; private set; }
    public decimal PrecioUnitario { get; private set; }
    public decimal Subtotal { get; private set; }
    public decimal DescuentoMonto { get; private set; }
    public decimal TasaITBISAplicada { get; private set; }
    public decimal MontoITBIS { get; private set; }
    public decimal TotalLinea { get; private set; }
    public string? MotivoDescuento { get; private set; }
    public string? AutorizadorDescuentoId { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    public string? DeletedBy { get; set; }

    public static OrdenLinea Create(int servicioProductoId, int cantidad, decimal precioUnitario, decimal tasaItbis, decimal descuentoMonto, string? motivoDescuento)
    {
        if (cantidad <= 0) throw new DomainRuleViolationException("La cantidad debe ser mayor que cero.");
        if (precioUnitario < 0) throw new DomainRuleViolationException("El precio unitario no puede ser negativo.");
        if (tasaItbis < 0 || tasaItbis > 1) throw new DomainRuleViolationException("La tasa de ITBIS debe estar entre 0 y 1.");
        var subtotal = decimal.Round(cantidad * precioUnitario, 4, MidpointRounding.AwayFromZero);
        if (descuentoMonto < 0 || descuentoMonto > subtotal) throw new DomainRuleViolationException("El descuento debe estar entre cero y el subtotal de la línea.");
        if (descuentoMonto > 0 && string.IsNullOrWhiteSpace(motivoDescuento)) throw new DomainRuleViolationException("Todo descuento requiere un motivo.");
        var baseImponible = subtotal - descuentoMonto;
        var itbis = decimal.Round(baseImponible * tasaItbis, 4, MidpointRounding.AwayFromZero);
        return new OrdenLinea { ServicioProductoId = servicioProductoId, Cantidad = cantidad, PrecioUnitario = precioUnitario, Subtotal = subtotal, DescuentoMonto = descuentoMonto, TasaITBISAplicada = tasaItbis, MontoITBIS = itbis, TotalLinea = baseImponible + itbis, MotivoDescuento = motivoDescuento };
    }
}
