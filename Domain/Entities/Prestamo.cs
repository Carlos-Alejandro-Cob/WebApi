using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiMangaBot.Domain.Entities
{
    public class Prestamo
    {
        [Key]
        [Column("ID")]
        public int ID { get; set; }

        [Column("MangaId")]
        public int? MangaId { get; set; }

        [Column("Name_Customer")]
        public string? Name_Customer { get; set; }

        [Column("LoanDate")]
        public DateTime? LoanDate { get; set; }

        [Column("ReturnDate")]
        public DateTime? ReturnDate { get; set; }

        // Propiedad de navegación
        public Manga? Manga { get; set; }
    }
}