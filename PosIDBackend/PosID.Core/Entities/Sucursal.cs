using System.Collections.Generic;
using PosID.Core.Common;

namespace PosID.Core.Entities
{
    public class Sucursal : AuditableEntity, ISoftDelete
    {
        public int Id { get; set; }
        public int EmpresaId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Direccion { get; set; }
        public string ZonaHoraria { get; set; } = "America/Santo_Domingo";
        public bool Activa { get; set; } = true;

        public Empresa Empresa { get; set; } = null!;
        public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();

        public bool IsDeleted { get; set; }
        public System.DateTimeOffset? DeletedAt { get; set; }
        public string? DeletedBy { get; set; }
    }
}
