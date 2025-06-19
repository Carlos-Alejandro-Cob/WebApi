// home/MiMangaBot/Controllers/V1/PrestamoController.cs
using MiMangaBot.Domain.Entities;
using MiMangaBot.Services.Features.Prestamos;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MiMangaBot.Controllers.V1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class PrestamoController : ControllerBase
    {
        private readonly PrestamoService _prestamoService;

        public PrestamoController(PrestamoService prestamoService)
        {
            _prestamoService = prestamoService;
        }

        // GET: api/v1/Prestamo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Prestamo>>> GetAllPrestamos()
        {
            var prestamos = await _prestamoService.GetAllPrestamos();
            return Ok(prestamos);
        }

        // GET: api/v1/Prestamo/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Prestamo>> GetPrestamoById(int id)
        {
            var prestamo = await _prestamoService.GetPrestamoById(id);
            if (prestamo == null)
            {
                return NotFound();
            }
            return Ok(prestamo);
        }

        // POST: api/v1/Prestamo
        [HttpPost]
        public async Task<ActionResult<Prestamo>> AddPrestamo(Prestamo prestamo)
        {
            // Puedes añadir validaciones de datos del modelo aquí si no están en el servicio
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _prestamoService.AddPrestamo(prestamo);
            // Devuelve 201 CreatedAtAction para indicar que se creó un nuevo recurso
            return CreatedAtAction(nameof(GetPrestamoById), new { id = prestamo.Id }, prestamo);
        }

        // PUT: api/v1/Prestamo/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePrestamo(int id, Prestamo prestamo)
        {
            if (id != prestamo.Id)
            {
                return BadRequest("El ID en la URL no coincide con el ID del préstamo en el cuerpo de la solicitud.");
            }

            // Aquí puedes optar por buscar el préstamo existente primero si necesitas actualizar campos específicos
            // en lugar de sobrescribir el objeto completo si el cliente envía un payload parcial.
            var existingPrestamo = await _prestamoService.GetPrestamoById(id);
            if (existingPrestamo == null)
            {
                return NotFound($"Préstamo con ID {id} no encontrado.");
            }

            // Actualiza las propiedades necesarias del préstamo existente
            existingPrestamo.Name_Customer = prestamo.Name_Customer;
            existingPrestamo.MangadexId = prestamo.MangadexId;
            existingPrestamo.ReturnDate = prestamo.ReturnDate;
            // No actualizar LoanDate aquí, ya que es la fecha de creación original.

            await _prestamoService.UpdatePrestamo(existingPrestamo); // Pasa el objeto actualizado
            return NoContent(); // 204 No Content para una actualización exitosa
        }

        // DELETE: api/v1/Prestamo/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePrestamo(int id)
        {
            var existingPrestamo = await _prestamoService.GetPrestamoById(id);
            if (existingPrestamo == null)
            {
                return NotFound($"Préstamo con ID {id} no encontrado.");
            }
            await _prestamoService.DeletePrestamo(id);
            return NoContent(); // 204 No Content para una eliminación exitosa
        }

        // GET: api/v1/Prestamo/byCustomerName?name={customerName}
        [HttpGet("byCustomerName")]
        public async Task<ActionResult<IEnumerable<Prestamo>>> GetPrestamosByCustomerName([FromQuery] string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest("El nombre del cliente no puede estar vacío.");
            }
            var prestamos = await _prestamoService.GetPrestamosByCustomerName(name);
            if (!prestamos.Any())
            {
                return NotFound($"No se encontraron préstamos para el cliente: {name}");
            }
            return Ok(prestamos);
        }

        // GET: api/v1/Prestamo/byMangaId/{mangaId}
        [HttpGet("byMangaId/{mangaId}")]
        public async Task<ActionResult<IEnumerable<Prestamo>>> GetPrestamosByMangaId(string mangaId)
        {
            var prestamos = await _prestamoService.GetPrestamosByMangaId(mangaId);
            if (!prestamos.Any())
            {
                return NotFound($"No se encontraron préstamos para el Manga ID: {mangaId}");
            }
            return Ok(prestamos);
        }
    }
}