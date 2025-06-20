using System.ComponentModel.DataAnnotations;

namespace MiMangaBot.Domain.Entities
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string Rol { get; set; } // "Usuario" o "Admin"
    }
} 