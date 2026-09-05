using PosID.Core.Common;

namespace PosID.Core.Entities
{
    public class TipoComprobante : AuditableEntity, ISoftDelete
    {
        public int Id { get; set; }
        public string CodigoDGII { get; set; } = string.Empty; // e.g. B01, B02, E31
        public string Descripcion { get; set; } = string.Empty;
        public bool EsElectronico { get; set; }
        public bool Activo { get; set; } = true;

        public bool IsDeleted { get; set; }
        public System.DateTimeOffset? DeletedAt { get; set; }
        public string? DeletedBy { get; set; }
    }
}
