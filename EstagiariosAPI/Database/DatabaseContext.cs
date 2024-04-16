using Microsoft.EntityFrameworkCore;
using EstagiariosAPI.Entities;
using System.Reflection.Metadata;
using Microsoft.Extensions.Hosting;

namespace EstagiariosAPI.Database
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Filme> Filmes { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) 
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Filme>()
                .HasMany(e => e.Usuario)
                .WithOne(e => e.Filme)
                .HasForeignKey(e => e.FilmeId)
                .IsRequired(false);

            modelBuilder.Entity<Usuario>()
                .HasMany(e => e.FilmesWatchList)
                .WithMany(e => e.UsuarioWatchList)
                .UsingEntity("WatchList");

            modelBuilder.Entity<Filme>()
                .HasMany(e => e.Usuarios)
                .WithMany(e => e.Filmes)
                .UsingEntity("Vistos");
        }
    }
}
