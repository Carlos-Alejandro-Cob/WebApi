// MiMangaBot/Domain/Interfaces/IMangaRepository.cs
using MiMangaBot.Domain.Entities; // Asegúrate de usar el namespace correcto de tu entidad Manga
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MiMangaBot.Domain.Interfaces; // El namespace debería ser el de tu proyecto + .Domain.Interfaces

public interface IMangaRepository
{
    Task<IEnumerable<Manga>> GetAllAsync();
    Task<Manga?> GetByIdAsync(string id);
    Task<Manga> AddAsync(Manga manga);
    Task<bool> UpdateAsync(Manga manga);
    Task<bool> DeleteAsync(string id);
}