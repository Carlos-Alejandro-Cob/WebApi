namespace MiMangaBot.Domain.Models
{
    public class UsuarioRegisterDto
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string Rol { get; set; } // "Usuario" o "Admin"
    }
} 