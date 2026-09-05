using System;
using PosID.Core.Common;
using PosID.Core.Enums;

namespace PosID.Core.Entities
{
    public class DocumentoFiscal : AuditableEntity, ISoftDelete
    {
        public int Id { get; set; }
        
        public int OrdenId { get; set; }
        public Orden Orden { get; set; } = null!;

        public int TipoComprobanteId { get; set; }
        public TipoComprobante TipoComprobante { get; set; } = null!;

        public string NCF { get; set; } = string.Empty; // e-NCF
        public string Serie { get; set; } = string.Empty;
        public long Secuencia { get; set; }

        public DateTimeOffset FechaEmision { get; set; }
        public EstadoDocumentoFiscal Estado { get; set; } = EstadoDocumentoFiscal.Borrador;

        public decimal TotalDocumento { get; set; }
        public decimal TotalITBIS { get; set; }

        // e-CF específicos
        public string? EstadoDGII { get; set; } // Aceptado, Rechazado
        public string? XMLRepresentacion { get; set; }
        public string? HashFirma { get; set; }

        // Referencia para correcciones (e.g., Nota de Crédito apunta a Factura)
        public int? DocumentoRelacionadoId { get; set; }
        public DocumentoFiscal? DocumentoRelacionado { get; set; }

        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
        public string? DeletedBy { get; set; }
    }
}
