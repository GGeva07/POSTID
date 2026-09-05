using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PosID.Core.Entities;

namespace PosID.Infrastructure.Persistence.Configurations
{
    public class PagoConfiguration : IEntityTypeConfiguration<Pago>
    {
        public void Configure(EntityTypeBuilder<Pago> builder)
        {
            builder.ToTable("Pagos");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Estado).HasConversion<string>().HasMaxLength(30);
            builder.Property(e => e.Monto).HasColumnType("decimal(18,2)");
            builder.Property(e => e.ReferenciaExterna).HasMaxLength(150);

            builder.HasOne(e => e.Orden)
                .WithMany(o => o.Pagos)
                .HasForeignKey(e => e.OrdenId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.MetodoPago)
                .WithMany()
                .HasForeignKey(e => e.MetodoPagoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Usuario)
                .WithMany()
                .HasForeignKey(e => e.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
