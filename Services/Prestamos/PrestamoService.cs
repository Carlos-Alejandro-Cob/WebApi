// home/MiMangaBot/Services/Features/Prestamos/PrestamoService.cs
using MiMangaBot.Domain.Entities;
using MiMangaBot.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MiMangaBot.Services.Features.Prestamos
{
    public class PrestamoService
    {
        private readonly IPrestamoRepository _prestamoRepository;

        public PrestamoService(IPrestamoRepository prestamoRepository)
        {
            _prestamoRepository = prestamoRepository;
        }

        public async Task<IEnumerable<Prestamo>> GetAllPrestamos()
        {
            return await _prestamoRepository.GetAllAsync();
        }

        // CORRECCIÓN: Se marca el tipo de retorno Prestamo como nullable
        public async Task<Prestamo?> GetPrestamoById(int id)
        {
            return await _prestamoRepository.GetByIdAsync(id);
        }

        public async Task AddPrestamo(Prestamo prestamo)
        {
            // Aquí puedes añadir lógica de negocio antes de guardar, por ejemplo:
            // - Validar que el MangaId exista
            // - Verificar disponibilidad del manga (si tienes ese concepto)
            await _prestamoRepository.AddAsync(prestamo);
        }

        public async Task UpdatePrestamo(Prestamo prestamo)
        {
            // Aquí puedes añadir lógica de negocio antes de actualizar
            await _prestamoRepository.UpdateAsync(prestamo);
        }

        public async Task DeletePrestamo(int id)
        {
            await _prestamoRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Prestamo>> GetPrestamosByCustomerName(string customerName)
        {
            return await _prestamoRepository.GetLoansByCustomerNameAsync(customerName);
        }

        public async Task<IEnumerable<Prestamo>> GetPrestamosByMangaId(string mangadexId)
        {
            return await _prestamoRepository.GetLoansByMangaIdAsync(mangadexId);
        }
    }
}