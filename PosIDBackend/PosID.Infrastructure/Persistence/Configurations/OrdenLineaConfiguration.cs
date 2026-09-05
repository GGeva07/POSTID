using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PosID.Core.Entities;

namespace PosID.Infrastructure.Persistence.Configurations
{
    public class OrdenLineaConfiguration : IEntityTypeConfiguration<OrdenLinea>
    {
        public void Configure(EntityTypeBuilder<OrdenLinea> builder)
        {
            builder.ToTable("OrdenLineas");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.PrecioUnitario).HasColumnType("decimal(18,4)");
            builder.Property(e => e.Subtotal).HasColumnType("decimal(18,4)");
            builder.Property(e => e.DescuentoMonto).HasColumnType("decimal(18,4)");
            builder.Property(e => e.TasaITBISAplicada).HasColumnType("decimal(6,4)");
            builder.Property(e => e.MontoITBIS).HasColumnType("decimal(18,4)");
            builder.Property(e => e.TotalLinea).HasColumnType("decimal(18,4)");

            builder.Property(e => e.MotivoDescuento).HasMaxLength(250);

            builder.HasOne(e => e.Orden)
                .WithMany(o => o.Lineas)
                .HasForeignKey(e => e.OrdenId)
                .OnDelete(DeleteBehavior.Cascade); // Si se elimina la orden (lógico), las líneas también (a nivel BD si fuera delete físico)

            builder.HasOne(e => e.ServicioProducto)
                .WithMany()
                .HasForeignKey(e => e.ServicioProductoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
