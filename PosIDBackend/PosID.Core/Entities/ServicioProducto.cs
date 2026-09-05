using PosID.Core.Common;

namespace PosID.Core.Entities
{
    public class ServicioProducto : AuditableEntity, ISoftDelete
    {
        public int Id { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public decimal TasaITBIS { get; set; }
        public bool Activo { get; set; } = true;
        public string? DuracionEntrega { get; set; }

        public bool IsDeleted { get; set; }
        public System.DateTimeOffset? DeletedAt { get; set; }
        public string? DeletedBy { get; set; }
    }
}
