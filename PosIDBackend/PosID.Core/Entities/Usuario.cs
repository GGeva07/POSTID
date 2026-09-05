using PosID.Core.Common;

namespace PosID.Core.Entities
{
    public class Usuario : AuditableEntity, ISoftDelete
    {
        public int Id { get; set; }
        public int SucursalId { get; set; }
        public string Identidad { get; set; } = string.Empty; // e.g. email, username
        public string Estado { get; set; } = "Activo";
        public string Permisos { get; set; } = string.Empty; // Simplified role/permissions representation
        public System.DateTimeOffset? UltimoAcceso { get; set; }

        public Sucursal Sucursal { get; set; } = null!;

        public bool IsDeleted { get; set; }
        public System.DateTimeOffset? DeletedAt { get; set; }
        public string? DeletedBy { get; set; }
    }
}
