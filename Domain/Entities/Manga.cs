// MiMangaBot/Domain/Entities/Manga.cs
using System; // Necesario para Guid.NewGuid().ToString()
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; // ¡ESTE USING ES CRUCIAL para [Column]!

namespace MiMangaBot.Domain.Entities;

public class Manga
{
    [Key] // Indica que esta es la clave primaria de la entidad
    [Column("MangaId")]
    public int MangaId { get; set; }

    [Column("Name_Main")]
    public string? Name_Main { get; set; }

    [Column("Autor")]
    public string? Autor { get; set; }

    [Column("Fecha_Emision")]
    public DateTime? Fecha_Emision { get; set; }

    [Column("Fecha_Publicacion")]
    public DateTime? Fecha_Publicacion { get; set; }

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
        Name_Main = string.Empty; // Inicializa Name_Main ya que es 'required' y string
    }
}
