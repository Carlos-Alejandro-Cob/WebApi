// home/MiMangaBot/Controllers/V1/PrestamoController.cs
using MiMangaBot.Domain.Entities;
using MiMangaBot.Services.Features.Prestamos;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using MiMangaBot.Domain.Models;

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
        public async Task<ActionResult<IEnumerable<PrestamoGetDto>>> GetAllPrestamos()
        {
            var prestamos = await _prestamoService.GetAllPrestamos();
            var result = prestamos.Select(p => new PrestamoGetDto
            {
                ID = p.ID,
                Name_Customer = p.Name_Customer,
                MangaId = p.MangaId,
                LoanDate = p.LoanDate,
                ReturnDate = p.ReturnDate,
                MangaName = p.Manga?.Name_Main ?? string.Empty
            });
            return Ok(result);
        }

        // GET: api/v1/Prestamo/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<PrestamoGetDto>> GetPrestamoById(int id)
        {
            var prestamo = await _prestamoService.GetPrestamoById(id);
            if (prestamo == null)
            {
                return NotFound();
            }
            var result = new PrestamoGetDto
            {
                ID = prestamo.ID,
                Name_Customer = prestamo.Name_Customer,
                MangaId = prestamo.MangaId,
                LoanDate = prestamo.LoanDate,
                ReturnDate = prestamo.ReturnDate,
                MangaName = prestamo.Manga?.Name_Main ?? string.Empty
            };
            return Ok(result);
        }

        // POST: api/v1/Prestamo
        [HttpPost]
        public async Task<ActionResult<Prestamo>> AddPrestamo(PrestamoDto prestamoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var prestamo = new Prestamo
            {
                Name_Customer = prestamoDto.Name_Customer,
                MangaId = prestamoDto.MangaId,
                LoanDate = prestamoDto.LoanDate,
                ReturnDate = prestamoDto.ReturnDate
            };
            await _prestamoService.AddPrestamo(prestamo);
            return CreatedAtAction(nameof(GetPrestamoById), new { id = prestamo.ID }, prestamo);
        }

        // PUT: api/v1/Prestamo/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePrestamo(int id, PrestamoDto prestamoDto)
        {
            var existingPrestamo = await _prestamoService.GetPrestamoById(id);
            if (existingPrestamo == null)
            {
                return NotFound($"Préstamo con ID {id} no encontrado.");
            }
            existingPrestamo.Name_Customer = prestamoDto.Name_Customer;
            existingPrestamo.MangaId = prestamoDto.MangaId;
            existingPrestamo.LoanDate = prestamoDto.LoanDate;
            existingPrestamo.ReturnDate = prestamoDto.ReturnDate;
            await _prestamoService.UpdatePrestamo(existingPrestamo);
            return NoContent();
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

        // GET: api/v1/Prestamo/byMangaId/{mangald}
        [HttpGet("byMangaId/{mangald}")]
        public async Task<ActionResult<IEnumerable<Prestamo>>> GetPrestamosByMangaId(int mangald)
        {
            var prestamos = await _prestamoService.GetPrestamosByMangaId(mangald);
            if (!prestamos.Any())
            {
                return NotFound($"No se encontraron préstamos para el Manga ID: {mangald}");
            }
            return Ok(prestamos);
        }
    }
}