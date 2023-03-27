using FolhaDePonto.Models;
using Microsoft.EntityFrameworkCore;

namespace FolhaDePonto.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<RegistroDePontoModel> RegistrosDePonto { get; set; }
        public DbSet<UsuarioModel> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração da chave primária composta da entidade RegistroDePonto
            modelBuilder.Entity<RegistroDePontoModel>()
                .HasKey(rp => new { rp.UserId, rp.Data });

            // Configuração da relação entre as entidades Usuario e RegistroDePonto
            modelBuilder.Entity<UsuarioModel>()
                .HasMany(u => u.RegistroDePonto)
                .WithOne(rp => rp.Usuario)
                .HasForeignKey(rp => rp.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

}




