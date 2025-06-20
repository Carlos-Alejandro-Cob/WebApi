// home/MiMangaBot/Data/Repositories/PrestamoRepository.cs
using MiMangaBot.Domain.Entities;
using MiMangaBot.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiMangaBot.Data.Repositories
{
    public class PrestamoRepository : IPrestamoRepository
    {
        private readonly ApplicationDbContext _context;

        public PrestamoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Prestamo>> GetAllAsync()
        {
            // Incluye el objeto Manga relacionado con el préstamo
            return await _context.Prestamos.Include(p => p.Manga).ToListAsync();
        }

        public async Task<Prestamo?> GetByIdAsync(int id)
        {
            // Incluye el objeto Manga relacionado con el préstamo
            return await _context.Prestamos.Include(p => p.Manga).FirstOrDefaultAsync(p => p.ID == id);
        }

        public async Task AddAsync(Prestamo prestamo)
        {
            prestamo.LoanDate = DateTime.UtcNow; // Establece la fecha del préstamo al añadir
            _context.Prestamos.Add(prestamo);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Prestamo prestamo)
        {
            _context.Entry(prestamo).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var prestamo = await _context.Prestamos.FindAsync(id);
            if (prestamo != null)
            {
                _context.Prestamos.Remove(prestamo);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Prestamo>> GetLoansByCustomerNameAsync(string customerName)
        {
            return await _context.Prestamos
                                 .Include(p => p.Manga)
                                 .Where(p => p.Name_Customer != null && p.Name_Customer.Contains(customerName))
                                 .ToListAsync();
        }

        public async Task<IEnumerable<Prestamo>> GetLoansByMangaIdAsync(int mangaId)
        {
            return await _context.Prestamos
                                 .Include(p => p.Manga)
                                 .Where(p => p.MangaId == mangaId)
                                 .ToListAsync();
        }
    }
}