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

    // El nombre de este DbSet *por defecto* mapeará a una tabla llamada "Mangas"
    // Si tu tabla existente se llama "mangas" (minúscula), EF Core la encontrará.
    public DbSet<Manga> Mangas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Opcional: Si por alguna razón quieres forzar el nombre de la tabla a "mangas" (minúscula)
        // Puedes usar fluent API:
        modelBuilder.Entity<Manga>().ToTable("mangas");

        // Otras configuraciones del modelo, si las necesitas, van aquí.
        // Las configuraciones de HasKey y Property().IsRequired() ya las estás manejando
        // en gran medida con los atributos en la entidad Manga.
    }
}