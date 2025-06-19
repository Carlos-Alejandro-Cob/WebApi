// MiMangaBot/Domain/Entities/Manga.cs
using System; // Necesario para Guid.NewGuid().ToString()
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; // ¡ESTE USING ES CRUCIAL para [Column]!

namespace MiMangaBot.Domain.Entities;

public class Manga
{
    [Key] // Indica que esta es la clave primaria de la entidad
    [Column("mangadex_id")] // Mapea esta propiedad a la columna 'mangadex_id' en la DB
    public string MangadexId { get; set; } = Guid.NewGuid().ToString(); // Cambiado a string para mangadex_id, genera un nuevo GUID por defecto

    [Required] // Asegura que el título sea requerido y no nulo en la DB
    [Column("title_main")] // Mapea a la columna 'title_main' en la DB
    public string Title { get; set; } // Nombre de la propiedad más descriptivo para el título principal

    [Column("alternative_titles")] // Mapea a 'alternative_titles'
    public string? AlternativeTitles { get; set; }

    [Column("authors")] // Mapea a la columna 'authors' en la DB
    public string? Author { get; set; } // Propiedad para autores

    [Column("artists")] // Mapea a la columna 'artists' en la DB
    public string? Artists { get; set; } // Propiedad para artistas

    [Column("description")] // Mapea a la columna 'description' en la DB
    public string? Description { get; set; } // Propiedad para la descripción/sinopsis

    [Column("status")] // Mapea a la columna 'status' en la DB
    public string? Status { get; set; } // Propiedad para el estado del manga

    [Column("publication_year")] // Mapea a la columna 'publication_year' en la DB (es un INT en tu DB, pero puede ser NULL)
    public int? PublicationYear { get; set; } // ¡Cambiado a int? para permitir valores nulos!

    [Column("original_language")] // Mapea a 'original_language'
    public string? OriginalLanguage { get; set; }

    [Column("genres_tags")] // Mapea a la columna 'genres_tags' en la DB
    public string? Genre { get; set; } // Propiedad para géneros/tags

    [Column("cover_image_url")] // ¡CORREGIDO! Mapea a la columna real en tu DB: 'cover_image_url'
    public string? CoverUrl { get; set; } // Propiedad para la URL de la portada

    [Column("total_chapters_on_mangadex")] // Añadido: Mapea a la columna real
    public int? TotalChaptersOnMangadex { get; set; } // También puede ser nulo

    [Column("source_api")] // Añadido: Mapea a la columna real
    public string? SourceApi { get; set; }

    // Las siguientes propiedades (created_at, updated_at) son gestionadas automáticamente por MySQL como TIMESTAMP,
    // y no necesitas mapearlas si no vas a interactuar con ellas directamente desde EF Core.
    // Si quisieras mapearlas, serían de tipo DateTime?
    // [Column("created_at")]
    // public DateTime? CreatedAt { get; set; }

    // [Column("updated_at")]
    // public DateTime? UpdatedAt { get; set; }

    // Las propiedades 'Volumes' e 'IsOngoing' han sido eliminadas ya que no existen en tu base de datos.

    public Manga()
    {
        Title = string.Empty; // Inicializa Title ya que es 'required' y string
    }
}
