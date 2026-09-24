using Microsoft.EntityFrameworkCore;
using UTNConecta.API.Models;

namespace UTNConecta.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Carrera> Carreras { get; set; }
        public DbSet<Habilidad> Habilidades { get; set; }
        public DbSet<Graduado> Graduados { get; set; }
        public DbSet<Organizacion> Organizaciones { get; set; }
        public DbSet<Oportunidad> Oportunidades { get; set; }
        public DbSet<Postulacion> Postulaciones { get; set; }
        public DbSet<GraduadoHabilidad> GraduadoHabilidades { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Carrera>().ToTable("carrera");
            modelBuilder.Entity<Carrera>().Property(c => c.Id).HasColumnName("id");
            modelBuilder.Entity<Carrera>().Property(c => c.Nombre).HasColumnName("nombre");
            modelBuilder.Entity<Carrera>().Property(c => c.Area).HasColumnName("area");

            modelBuilder.Entity<Habilidad>().ToTable("habilidad");
            modelBuilder.Entity<Habilidad>().Property(h => h.Id).HasColumnName("id");
            modelBuilder.Entity<Habilidad>().Property(h => h.Nombre).HasColumnName("nombre");
            modelBuilder.Entity<Habilidad>().Property(h => h.Area).HasColumnName("area");

            modelBuilder.Entity<Graduado>().ToTable("graduado");
            modelBuilder.Entity<Graduado>().Property(g => g.Id).HasColumnName("id");
            modelBuilder.Entity<Graduado>().Property(g => g.Nombre).HasColumnName("nombre");
            modelBuilder.Entity<Graduado>().Property(g => g.Apellido).HasColumnName("apellido");
            modelBuilder.Entity<Graduado>().Property(g => g.Email).HasColumnName("email");
            modelBuilder.Entity<Graduado>().Property(g => g.Contrasena).HasColumnName("contrasena");
            modelBuilder.Entity<Graduado>().Property(g => g.IdCarrera).HasColumnName("id_carrera");
            modelBuilder.Entity<Graduado>().Property(g => g.AnioGraduacion).HasColumnName("anio_graduacion");
            modelBuilder.Entity<Graduado>()
                .HasOne(g => g.Carrera)
                .WithMany()
                .HasForeignKey(g => g.IdCarrera);

            modelBuilder.Entity<GraduadoHabilidad>()
                .HasKey(gh => new { gh.IdGraduado, gh.IdHabilidad });
        }
    }
}