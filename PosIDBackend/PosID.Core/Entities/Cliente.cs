using System.Collections.Generic;
using PosID.Core.Common;

namespace PosID.Core.Entities
{
    public class Cliente : AuditableEntity, ISoftDelete
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string? TipoDocumento { get; set; } // Cedula, RNC, Pasaporte
        public string? DocumentoFiscal { get; set; }
        public string? DireccionFiscal { get; set; }
        public string Estado { get; set; } = "Activo";

        public ICollection<Orden> Ordenes { get; set; } = new List<Orden>();

        public bool IsDeleted { get; set; }
        public System.DateTimeOffset? DeletedAt { get; set; }
        public string? DeletedBy { get; set; }
    }
}
