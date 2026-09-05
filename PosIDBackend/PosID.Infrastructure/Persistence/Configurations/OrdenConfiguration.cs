using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PosID.Core.Entities;

namespace PosID.Infrastructure.Persistence.Configurations
{
    public class OrdenConfiguration : IEntityTypeConfiguration<Orden>
    {
        public void Configure(EntityTypeBuilder<Orden> builder)
        {
            builder.ToTable("Ordenes");
            builder.HasKey(e => e.Id);
            
            builder.Property(e => e.NumeroInterno).IsRequired().HasMaxLength(50);
            builder.HasIndex(e => e.NumeroInterno).IsUnique();

            builder.Property(e => e.Estado).HasConversion<string>().HasMaxLength(30);

            builder.Property(e => e.Subtotal).HasColumnType("decimal(18,2)");
            builder.Property(e => e.DescuentoTotal).HasColumnType("decimal(18,2)");
            builder.Property(e => e.ImpuestoTotal).HasColumnType("decimal(18,2)");
            builder.Property(e => e.Total).HasColumnType("decimal(18,2)");

            builder.HasOne(e => e.Cliente)
                .WithMany(c => c.Ordenes)
                .HasForeignKey(e => e.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Sucursal)
                .WithMany()
                .HasForeignKey(e => e.SucursalId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Creador)
                .WithMany()
                .HasForeignKey(e => e.CreadorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
