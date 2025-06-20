using MiMangaBot.Data.Repositories;
using MiMangaBot.Domain.Entities;
using MiMangaBot.Domain.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MiMangaBot.Data;

namespace MiMangaBot.Services.Features.Usuarios
{
    public class UsuarioService
    {
        private readonly UsuarioRepository _usuarioRepository;
        public UsuarioService(ApplicationDbContext context)
        {
            _usuarioRepository = new UsuarioRepository(context);
        }

        public bool Register(UsuarioRegisterDto dto, out string error)
        {
            error = string.Empty;
            if (_usuarioRepository.GetByUsername(dto.Username) != null)
            {
                error = "El nombre de usuario ya existe.";
                return false;
            }
            var usuario = new Usuario
            {
                Username = dto.Username,
                Password = dto.Password, // El repositorio la hashea
                Rol = dto.Rol
            };
            _usuarioRepository.Add(usuario);
            return true;
        }

        public string? Login(string username, string password, string jwtKey, out string error)
        {
            error = string.Empty;
            // Usuario y contraseña hardcodeados
            const string hardcodedUser = "admin";
            const string hardcodedPass = "admin123";
            const string hardcodedRol = "Admin";
            if (username != hardcodedUser || password != hardcodedPass)
            {
                error = "Usuario o contraseña incorrectos.";
                return null;
            }
            // Crear claims
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, hardcodedUser),
                new Claim(ClaimTypes.Role, hardcodedRol)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds
            );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenString;
        }

        public bool ValidarToken(string username, string token)
        {
            return _usuarioRepository.IsTokenValid(username, token);
        }
    }
} 