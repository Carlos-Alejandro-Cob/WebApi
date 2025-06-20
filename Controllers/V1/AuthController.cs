using Microsoft.AspNetCore.Mvc;
using MiMangaBot.Domain.Models;
using MiMangaBot.Services.Features.Usuarios;
using Microsoft.Extensions.Configuration;
using MiMangaBot.Data;

namespace MiMangaBot.Controllers.V1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UsuarioService _usuarioService;
        public AuthController(ApplicationDbContext context)
        {
            _usuarioService = new UsuarioService(context);
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] UsuarioRegisterDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Username) || string.IsNullOrWhiteSpace(dto.Password) || string.IsNullOrWhiteSpace(dto.Rol))
                return BadRequest("Datos incompletos");
            if (dto.Password.Length < 6)
                return BadRequest("La contraseña debe tener al menos 6 caracteres.");
            if (_usuarioService.Register(dto, out string error))
                return Ok("Usuario registrado correctamente");
            else
                return BadRequest(error);
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UsuarioLoginDto dto, [FromServices] IConfiguration config)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Username) || string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest("Datos incompletos");
            string key = config["Jwt:Key"] ?? "clave_super_secreta_12345";
            var token = _usuarioService.Login(dto.Username, dto.Password, key, out string error);
            if (token == null)
                return Unauthorized(error);
            return Ok(new { token });
        }
    }
} 