// MiMangaBot/Controllers/V1/MangaController.cs
using MiMangaBot.Domain.Entities;
using MiMangaBot.Services.Features.Mangas;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks; // Necesario para Task<IActionResult>
using System; // Necesario para Guid

namespace MiMangaBot.Controllers.V1;

[ApiController]
[Route("api/v1/[controller]")]
public class MangaController : ControllerBase
{
    private readonly MangaService _mangaService;

    public MangaController(MangaService mangaService)
    {
        _mangaService = mangaService;
    }

    // GET api/v1/manga
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var mangas = await _mangaService.GetAll();
        return Ok(mangas);
    }

    // GET api/v1/manga/{id}
    [HttpGet("{id}")] // Cambiado a {id} sin :int porque ahora es string
    public async Task<IActionResult> GetById([FromRoute] string id) // Cambiado a string id
    {
        var manga = await _mangaService.GetById(id);
        if (manga == null)
        {
            return NotFound(new { Message = $"Manga con MangadexId {id} no encontrado." });
        }
        return Ok(manga);
    }

    // POST api/v1/manga
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] Manga manga)
    {
        // Si el cliente no envía un MangadexId, generamos uno nuevo.
        // Si lo envía, lo usará, pero ten cuidado con duplicados en tu DB.
        if (string.IsNullOrEmpty(manga.MangadexId))
        {
             manga.MangadexId = Guid.NewGuid().ToString();
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var newManga = await _mangaService.Add(manga);
        // Usamos MangadexId en CreatedAtAction
        return CreatedAtAction(nameof(GetById), new { id = newManga.MangadexId }, newManga);
    }

    // PUT api/v1/manga/{id}
    [HttpPut("{id}")] // Cambiado a {id} sin :int
    public async Task<IActionResult> Update([FromRoute] string id, [FromBody] Manga mangaToUpdate) // Cambiado a string id
    {
        // Es crucial que el ID del cuerpo coincida con el ID de la ruta para seguridad y consistencia.
        if (id != mangaToUpdate.MangadexId) // Comparamos MangadexId
        {
            return BadRequest(new { Message = "El ID de la ruta no coincide con el MangadexId del cuerpo." });
        }
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var success = await _mangaService.Update(mangaToUpdate); // Añadido await
        if (!success)
        {
            return NotFound(new { Message = $"Manga con MangadexId {id} no encontrado para actualizar." });
        }
        return NoContent();
    }

    // DELETE api/v1/manga/{id}
    [HttpDelete("{id}")] // Cambiado a {id} sin :int
    public async Task<IActionResult> Delete([FromRoute] string id) // Cambiado a string id
    {
        var success = await _mangaService.Delete(id); // Añadido await
        if (!success)
        {
            return NotFound(new { Message = $"Manga con MangadexId {id} no encontrado para eliminar." });
        }
        return NoContent();
    }
}