using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PosID.Core.Entities;

namespace PosID.Infrastructure.Persistence.Configurations
{
    public class DocumentoFiscalConfiguration : IEntityTypeConfiguration<DocumentoFiscal>
    {
        public void Configure(EntityTypeBuilder<DocumentoFiscal> builder)
        {
            builder.ToTable("DocumentosFiscales");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Estado).HasConversion<string>().HasMaxLength(30);
            builder.Property(e => e.NCF).HasMaxLength(19);
            builder.HasIndex(e => e.NCF).IsUnique().HasFilter("[NCF] IS NOT NULL AND [NCF] != ''");

            builder.Property(e => e.TotalDocumento).HasColumnType("decimal(18,2)");
            builder.Property(e => e.TotalITBIS).HasColumnType("decimal(18,2)");

            builder.HasOne(e => e.Orden)
                .WithMany(o => o.DocumentosFiscales)
                .HasForeignKey(e => e.OrdenId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.TipoComprobante)
                .WithMany()
                .HasForeignKey(e => e.TipoComprobanteId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.DocumentoRelacionado)
                .WithMany()
                .HasForeignKey(e => e.DocumentoRelacionadoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
