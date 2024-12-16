using GestionReportes.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionReportes.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Vecino> Vecinos { get; set; }
        public DbSet<FuncionarioMunicipal> FuncionariosMunicipal { get; set; }
        public DbSet<Reporte> Reportes { get; set; }
        public DbSet<EstadoReporte> EstadosReporte { get; set; }
        public DbSet<TipoReporte> TiposReporte { get; set; }
        public DbSet<HistorialReporte> HistorialReportes { get; set; }
        public DbSet<Documento> Documentos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración de Vecino y su relación con Usuario
            modelBuilder.Entity<Vecino>()
                .HasOne(v => v.Usuario)
                .WithOne()
                .HasForeignKey<Vecino>(v => v.id); // FK = PK
            modelBuilder.Entity<FuncionarioMunicipal>()
                .HasOne(v => v.Usuario)
                .WithOne()
                .HasForeignKey<FuncionarioMunicipal>(v => v.id);
            modelBuilder.Entity<Reporte>(entity =>
            {
                entity.HasOne(r => r.Vecino)
                      .WithMany(v => v.Reportes) // O relación inversa, como Vecino.Reportes
                      .HasForeignKey(r => r.idVecino); // Define clave foránea

                entity.HasOne(r => r.Estado)
                      .WithMany()
                      .HasForeignKey("idEstado");

                entity.HasOne(r => r.Tipo)
                      .WithMany()
                      .HasForeignKey("idTipo");

                entity.HasMany(r => r.HistorialReportes)
                      .WithOne(hr => hr.Reporte)
                      .HasForeignKey(hr => hr.idReporte);
                      
            });
            modelBuilder.Entity<HistorialReporte>(entity =>
            {
            //    entity.HasOne(h => h.Reporte)
            //          .WithMany()
            //          .HasForeignKey(h => h.Reporte.id);
                entity.HasOne(hr => hr.FuncionarioMunicipal)
                      .WithMany()
                      .HasForeignKey(hr => hr.idFuncionario);
            });
        }
    }
}
