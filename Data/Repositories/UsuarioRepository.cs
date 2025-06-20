using MiMangaBot.Domain.Entities;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace MiMangaBot.Data.Repositories
{
    public class UsuarioRepository
    {
        private readonly ApplicationDbContext _context;
        public UsuarioRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(Usuario usuario)
        {
            usuario.Password = HashPassword(usuario.Password);
            _context.Usuarios.Add(usuario);
            _context.SaveChanges();
        }

        public Usuario? GetByUsername(string username)
        {
            return _context.Usuarios.FirstOrDefault(u => u.Username == username);
        }

        public bool ValidatePassword(string username, string password)
        {
            var usuario = GetByUsername(username);
            if (usuario == null) return false;
            return usuario.Password == HashPassword(password);
        }

        public void Update(Usuario usuario)
        {
            _context.Usuarios.Update(usuario);
            _context.SaveChanges();
        }

        public bool IsTokenValid(string username, string token)
        {
            var usuario = GetByUsername(username);
            return usuario != null && usuario.Token == token;
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
} 