// MiMangaBot/Data/ApplicationDbContext.cs
using MiMangaBot.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MiMangaBot.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // DbSet existente para Mangas
    public DbSet<Manga> Mangas { get; set; }

    // ¡Nuevo! DbSet para la entidad Prestamo
    public DbSet<Prestamo> Prestamos { get; set; }

    // ¡Nuevo! DbSet para la entidad Usuario
    public DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Opcional: Si por alguna razón quieres forzar el nombre de la tabla a "mangas" (minúscula)
        // Puedes usar fluent API. Esto es útil si tu tabla en la BD es 'mangas' y tu DbSet es 'Mangas'.
        modelBuilder.Entity<Manga>().ToTable("mangas");

        // Configuración para la entidad Prestamo
        modelBuilder.Entity<Prestamo>()
            .HasKey(p => p.ID);

        // Configuración de la relación entre Prestamo y Manga (muchos a uno)
        // Un Prestamo tiene un Manga
        modelBuilder.Entity<Prestamo>()
            .HasOne(p => p.Manga)
            .WithMany()
            .HasForeignKey(p => p.MangaId)
            .HasPrincipalKey(m => m.MangaId)
            .OnDelete(DeleteBehavior.Restrict); // Comportamiento al borrar un Manga: Restringe la eliminación si hay préstamos asociados
        
        // No hay configuración para Name_Customer aquí, ya que es una propiedad simple
        // y no forma parte de una relación de clave foránea.

        modelBuilder.Entity<Prestamo>().ToTable("prestamos");
    }
}