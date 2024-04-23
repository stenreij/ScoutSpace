using Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainServices.Interfaces
{
    public interface IPlayerService
    {
        Task<IEnumerable<Player>> GetAllPlayersAsync();
        Task<Player> GetPlayerByIdAsync(int id);
        Task UpdatePlayerAsync(Player player);
        Task DeletePlayerAsync(int id);
        Task AddPlayerAsync(Player player);
        Task<bool> IsNewPlayerAsync(string firstName, string lastName, DateTime birthDate);
        Task<bool> IsNotExistingPlayerUpdateAsync(string firstName, string lastName, DateTime birthDate, int playerId);
    }
}
