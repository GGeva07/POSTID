using System.Collections.Generic;
using PosID.Core.Common;

namespace PosID.Core.Entities
{
    public class Empresa : AuditableEntity, ISoftDelete
    {
        public int Id { get; set; }
        public string RNC { get; set; } = string.Empty;
        public string RazonSocial { get; set; } = string.Empty;
        public string? Direccion { get; set; }
        public string RegimenFiscal { get; set; } = string.Empty;

        public bool IsDeleted { get; set; }
        public System.DateTimeOffset? DeletedAt { get; set; }
        public string? DeletedBy { get; set; }

        public ICollection<Sucursal> Sucursales { get; set; } = new List<Sucursal>();
    }
}
