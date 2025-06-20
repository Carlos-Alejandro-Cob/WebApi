using MiMangaBot.Services.Features.Mangas;
using MiMangaBot.Services.Features.Prestamos; // Asegúrate de que este using esté presente
using MiMangaBot.Domain.Interfaces;
using MiMangaBot.Data;
using MiMangaBot.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Configura el DbContext para MySQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("La cadena de conexión 'DefaultConnection' no está configurada.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySQL(connectionString)
    .LogTo(Console.WriteLine, LogLevel.Information) // Opcional: para ver las queries en la consola durante el desarrollo
    .EnableSensitiveDataLogging() // Opcional: para ver parámetros de queries en consola (solo desarrollo)
    .EnableDetailedErrors()); // Opcional: para errores más detallados de EF Core

// Registra la interfaz y la implementación del Repositorio de Mangas
builder.Services.AddScoped<IMangaRepository, MangaRepository>();

// Registra tu MangaService (que ahora depende de IMangaRepository)
builder.Services.AddScoped<MangaService>();

// --- NUEVA LÍNEA AÑADIDA PARA REGISTRAR PRESTAMOSERVICE ---
builder.Services.AddScoped<PrestamoService>();
// --- FIN DE LA NUEVA LÍNEA ---

// Registra la interfaz y la implementación del Repositorio de Préstamos
builder.Services.AddScoped<IPrestamoRepository, PrestamoRepository>();

// Clave secreta para JWT (mínimo 32 caracteres)
var key = builder.Configuration["Jwt:Key"] ?? "EstaEsUnaClaveSuperSecretaJWT123456!";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
    };
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "v1",
        Title = "MangaBot API",
        Description = "Una API para gestionar una increíble colección de mangas",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Tu Nombre/Equipo",
            Url = new Uri("https://tuwebsite.com")
        }
    });
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Introduce el token JWT con el prefijo 'Bearer '"
    });
    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// --- CONFIGURACIÓN DEL VERSIONADO DE API ---
builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
});

// Configuración para la integración de Swagger con el versionado de API
builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});
// --- FIN DE LA CONFIGURACIÓN DEL VERSIONADO DE API ---

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "MangaBot API V1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();