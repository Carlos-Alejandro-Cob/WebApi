// MiMangaBot/Controllers/V1/MangaController.cs
using MiMangaBot.Domain.Entities;
using MiMangaBot.Domain.Models;
using MiMangaBot.Services.Features.Mangas;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;

namespace MiMangaBot.Controllers.V1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class MangaController : ControllerBase
    {
        private readonly MangaService _mangaService;

        public MangaController(MangaService mangaService)
        {
            _mangaService = mangaService;
        }

        // GET api/v1/manga (sin paginación - mantener para compatibilidad)
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var mangas = await _mangaService.GetAll();
            return Ok(mangas);
        }

        // GET api/v1/manga/paged?pageNumber=1&pageSize=10
        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged([FromQuery] PaginationParameters paginationParameters)
        {
            var pagedResult = await _mangaService.GetPaged(paginationParameters);

            // Opcionalmente, puedes agregar headers HTTP para la información de paginación
            Response.Headers.Add("X-Pagination-TotalCount", pagedResult.Pagination.TotalCount.ToString());
            Response.Headers.Add("X-Pagination-TotalPages", pagedResult.Pagination.TotalPages.ToString());
            Response.Headers.Add("X-Pagination-CurrentPage", pagedResult.Pagination.CurrentPage.ToString());
            Response.Headers.Add("X-Pagination-PageSize", pagedResult.Pagination.PageSize.ToString());
            Response.Headers.Add("X-Pagination-HasNext", pagedResult.Pagination.HasNext.ToString());
            Response.Headers.Add("X-Pagination-HasPrevious", pagedResult.Pagination.HasPrevious.ToString());

            return Ok(pagedResult);
        }

        // GET api/v1/manga/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] string id)
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
            if (string.IsNullOrEmpty(manga.MangadexId))
            {
                manga.MangadexId = Guid.NewGuid().ToString();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newManga = await _mangaService.Add(manga);
            return CreatedAtAction(nameof(GetById), new { id = newManga.MangadexId }, newManga);
        }

        // PUT api/v1/manga/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] string id, [FromBody] Manga mangaToUpdate)
        {
            if (id != mangaToUpdate.MangadexId)
            {
                return BadRequest(new { Message = "El ID de la ruta no coincide con el MangadexId del cuerpo." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var success = await _mangaService.Update(mangaToUpdate);
            if (!success)
            {
                return NotFound(new { Message = $"Manga con MangadexId {id} no encontrado para actualizar." });
            }
            return NoContent();
        }

        // DELETE api/v1/manga/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            var success = await _mangaService.Delete(id);
            if (!success)
            {
                return NotFound(new { Message = $"Manga con MangadexId {id} no encontrado para eliminar." });
            }
            return NoContent();
        }
    }
}