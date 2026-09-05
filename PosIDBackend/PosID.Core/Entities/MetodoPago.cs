using PosID.Core.Common;

namespace PosID.Core.Entities
{
    public class MetodoPago : AuditableEntity, ISoftDelete
    {
        public int Id { get; set; }
        public string Codigo { get; set; } = string.Empty; // e.g. EF, TC, TR, CR
        public string Descripcion { get; set; } = string.Empty; // Efectivo, Tarjeta, etc.
        public bool Activo { get; set; } = true;

        public bool IsDeleted { get; set; }
        public System.DateTimeOffset? DeletedAt { get; set; }
        public string? DeletedBy { get; set; }
    }
}
