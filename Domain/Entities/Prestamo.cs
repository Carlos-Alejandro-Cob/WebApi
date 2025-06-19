// home/MiMangaBot/Domain/Entities/Prestamo.cs
using System;

namespace MiMangaBot.Domain.Entities
{
    public class Prestamo
    {
        public int Id { get; set; } // Clave Primaria
        public string Name_Customer { get; set; } // Nombre del cliente
        public string MangadexId { get; set; } // Clave Foránea al Manga (ahora string y renombrada)
        public DateTime LoanDate { get; set; } // Fecha de préstamo
        public DateTime ReturnDate { get; set; } // Fecha de devolución esperada

        // Propiedad de navegación para el Manga (opcional, pero útil si quieres cargar el objeto Manga completo)
        public Manga Manga { get; set; } 

        public Prestamo()
        {
            Name_Customer = string.Empty;
            MangadexId = string.Empty;
            Manga = new Manga();
        }
    }
}