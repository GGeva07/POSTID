using System;
using PosID.Core.Common;
using PosID.Core.Enums;

namespace PosID.Core.Entities
{
    public class Pago : AuditableEntity, ISoftDelete
    {
        public int Id { get; set; }
        
        public int OrdenId { get; set; }
        public Orden Orden { get; set; } = null!;

        public int MetodoPagoId { get; set; }
        public MetodoPago MetodoPago { get; set; } = null!;

        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;

        public decimal Monto { get; set; }
        public DateTimeOffset FechaPago { get; set; }
        public EstadoPago Estado { get; set; } = EstadoPago.Pendiente;

        public string? ReferenciaExterna { get; set; }
        
        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
        public string? DeletedBy { get; set; }
    }
}
