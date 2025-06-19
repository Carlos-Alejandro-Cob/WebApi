// MiMangaBot/Services/Features/Mangas/MangaService.cs
using MiMangaBot.Domain.Entities;
using MiMangaBot.Domain.Interfaces;
using MiMangaBot.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MiMangaBot.Services.Features.Mangas
{
    public class MangaService
    {
        private readonly IMangaRepository _mangaRepository;

        public MangaService(IMangaRepository mangaRepository)
        {
            _mangaRepository = mangaRepository;
        }

        // Operaciones CRUD

        public async Task<IEnumerable<Manga>> GetAll()
        {
            return await _mangaRepository.GetAllAsync();
        }

        public async Task<PagedResult<Manga>> GetPaged(PaginationParameters paginationParameters)
        {
            return await _mangaRepository.GetPagedAsync(paginationParameters);
        }

        public async Task<Manga?> GetById(string id)
        {
            return await _mangaRepository.GetByIdAsync(id);
        }

        public async Task<Manga> Add(Manga manga)
        {
            return await _mangaRepository.AddAsync(manga);
        }

        public async Task<bool> Update(Manga mangaToUpdate)
        {
            return await _mangaRepository.UpdateAsync(mangaToUpdate);
        }

        public async Task<bool> Delete(string id)
        {
            return await _mangaRepository.DeleteAsync(id);
        }
    }
}