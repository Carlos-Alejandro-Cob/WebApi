// home/MiMangaBot/Domain/Interfaces/IPrestamoRepository.cs
using MiMangaBot.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MiMangaBot.Domain.Interfaces
{
    public interface IPrestamoRepository
    {
        Task<IEnumerable<Prestamo>> GetAllAsync();
        Task<Prestamo?> GetByIdAsync(int id);
        Task AddAsync(Prestamo prestamo);
        Task UpdateAsync(Prestamo prestamo);
        Task DeleteAsync(int id);
        // Puedes añadir métodos específicos, ej. para buscar por nombre de cliente o manga
        Task<IEnumerable<Prestamo>> GetLoansByCustomerNameAsync(string customerName);
        Task<IEnumerable<Prestamo>> GetLoansByMangaIdAsync(int mangald);
    }
}