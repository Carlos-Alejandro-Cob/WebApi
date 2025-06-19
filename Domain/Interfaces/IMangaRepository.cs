// MiMangaBot/Domain/Interfaces/IMangaRepository.cs
using MiMangaBot.Domain.Entities;
using MiMangaBot.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MiMangaBot.Domain.Interfaces
{
    public interface IMangaRepository
    {
        Task<IEnumerable<Manga>> GetAllAsync();
        Task<PagedResult<Manga>> GetPagedAsync(PaginationParameters paginationParameters);
        Task<Manga?> GetByIdAsync(string id);
        Task<Manga> AddAsync(Manga manga);
        Task<bool> UpdateAsync(Manga manga);
        Task<bool> DeleteAsync(string id);
    }
}