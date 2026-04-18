using Microsoft.EntityFrameworkCore;
using sistema_de_informacion_bibliotecaria_sib;
using sistema_de_informacion_bibliotecaria_sib.Models;

public class BibliotecaContext : DbContext
{
    public BibliotecaContext(DbContextOptions<BibliotecaContext> options)
        : base(options)
    {
    }

    // Tablas
    public DbSet<Prestamo> Prestamos { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Libro> Libros { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 🔗 Relación: Prestamo → Usuario (muchos a uno)
        modelBuilder.Entity<Prestamo>()
            .HasOne(p => p.Usuario)
            .WithMany(u => u.Prestamos)
            .HasForeignKey(p => p.IdUsuario)
            .OnDelete(DeleteBehavior.Restrict);

        // 🔗 Relación: Prestamo → Libro (muchos a uno)
        modelBuilder.Entity<Prestamo>()
            .HasOne(p => p.Libro)
            .WithMany(l => l.Prestamo)
            .HasForeignKey(p => p.IdLibro)
            .OnDelete(DeleteBehavior.Restrict);
    }
}