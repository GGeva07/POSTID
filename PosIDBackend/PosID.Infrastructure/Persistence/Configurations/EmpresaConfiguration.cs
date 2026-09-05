using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PosID.Core.Entities;

namespace PosID.Infrastructure.Persistence.Configurations
{
    public class EmpresaConfiguration : IEntityTypeConfiguration<Empresa>
    {
        public void Configure(EntityTypeBuilder<Empresa> builder)
        {
            builder.ToTable("Empresas");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.RNC).IsRequired().HasMaxLength(20);
            builder.Property(e => e.RazonSocial).IsRequired().HasMaxLength(150);
            builder.HasIndex(e => e.RNC).IsUnique();
        }
    }
}
