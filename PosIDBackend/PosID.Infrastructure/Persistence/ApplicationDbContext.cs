using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PosID.Core.Common;
using PosID.Core.Entities;

namespace PosID.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Empresa> Empresas => Set<Empresa>();
        public DbSet<Sucursal> Sucursales => Set<Sucursal>();
        public DbSet<Usuario> Usuarios => Set<Usuario>();
        public DbSet<Cliente> Clientes => Set<Cliente>();
        public DbSet<ServicioProducto> ServiciosProductos => Set<ServicioProducto>();
        public DbSet<MetodoPago> MetodosPago => Set<MetodoPago>();
        public DbSet<TipoComprobante> TiposComprobante => Set<TipoComprobante>();
        public DbSet<Orden> Ordenes => Set<Orden>();
        public DbSet<OrdenLinea> OrdenLineas => Set<OrdenLinea>();
        public DbSet<Pago> Pagos => Set<Pago>();
        public DbSet<DocumentoFiscal> DocumentosFiscales => Set<DocumentoFiscal>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Aplicar configuraciones desde IEntityTypeConfiguration
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            // Global Query Filter para Soft Delete
            modelBuilder.Entity<Empresa>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Sucursal>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Usuario>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Cliente>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<ServicioProducto>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<MetodoPago>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<TipoComprobante>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Orden>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<OrdenLinea>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Pago>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<DocumentoFiscal>().HasQueryFilter(x => !x.IsDeleted);

            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = DateTimeOffset.UtcNow;
                        // TODO: Set CreatedBy from CurrentUserService
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedAt = DateTimeOffset.UtcNow;
                        // TODO: Set UpdatedBy from CurrentUserService
                        break;
                }
            }

            foreach (var entry in ChangeTracker.Entries<ISoftDelete>())
            {
                switch (entry.State)
                {
                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entry.Entity.IsDeleted = true;
                        entry.Entity.DeletedAt = DateTimeOffset.UtcNow;
                        // TODO: Set DeletedBy
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
