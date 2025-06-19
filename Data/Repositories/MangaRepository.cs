// MiMangaBot/Data/Repositories/MangaRepository.cs
using MiMangaBot.Domain.Entities;
using MiMangaBot.Domain.Interfaces;
using MiMangaBot.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiMangaBot.Data.Repositories
{
    public class MangaRepository : IMangaRepository
    {
        private readonly ApplicationDbContext _context;

        public MangaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Manga>> GetAllAsync()
        {
            return await _context.Mangas.ToListAsync();
        }

        public async Task<PagedResult<Manga>> GetPagedAsync(PaginationParameters paginationParameters)
        {
            var query = _context.Mangas.AsQueryable();

            // Contar el total de registros
            var totalCount = await query.CountAsync();

            // Aplicar paginación
            var mangas = await query
                .Skip((paginationParameters.PageNumber - 1) * paginationParameters.PageSize)
                .Take(paginationParameters.PageSize)
                .ToListAsync();

            return new PagedResult<Manga>(mangas, totalCount, paginationParameters.PageNumber, paginationParameters.PageSize);
        }

        public async Task<Manga?> GetByIdAsync(string id)
        {
            return await _context.Mangas.FirstOrDefaultAsync(m => m.MangadexId == id);
        }

        public async Task<Manga> AddAsync(Manga manga)
        {
            _context.Mangas.Add(manga);
            await _context.SaveChangesAsync();
            return manga;
        }

        public async Task<bool> UpdateAsync(Manga manga)
        {
            var existingManga = await _context.Mangas.FirstOrDefaultAsync(m => m.MangadexId == manga.MangadexId);
            if (existingManga != null)
            {
                _context.Entry(existingManga).CurrentValues.SetValues(manga);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var mangaToRemove = await _context.Mangas.FirstOrDefaultAsync(m => m.MangadexId == id);
            if (mangaToRemove != null)
            {
                _context.Mangas.Remove(mangaToRemove);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}