// MiMangaBot/Program.cs
using MiMangaBot.Services.Features.Mangas;
using MiMangaBot.Domain.Interfaces; // Para IMangaRepository
using MiMangaBot.Data; // Para ApplicationDbContext
using MiMangaBot.Data.Repositories; // Para MangaRepository
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// --- ¡NUEVAS LÍNEAS PARA EL DBContext Y EL REPOSITORIO! ---
// 1. Configura el DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("La cadena de conexión 'DefaultConnection' no está configurada.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySQL(connectionString)
           .LogTo(Console.WriteLine, LogLevel.Information) // Opcional: para ver las queries en la consola durante el desarrollo
           .EnableSensitiveDataLogging() // Opcional: para ver parámetros de queries en consola (solo desarrollo)
           .EnableDetailedErrors()); // Opcional: para errores más detallados de EF Core

// 2. Registra la interfaz y la implementación del Repositorio
builder.Services.AddScoped<IMangaRepository, MangaRepository>();

// 3. Registra tu MangaService (que ahora depende de IMangaRepository)
builder.Services.AddScoped<MangaService>();
// --- FIN DE LAS NUEVAS LÍNEAS ---


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
});


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
app.UseAuthorization();
app.MapControllers();

app.Run();