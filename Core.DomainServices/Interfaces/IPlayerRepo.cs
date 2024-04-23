using Core.Domain;

namespace Core.DomainServices.Interfaces
{
    public interface IPlayerRepo
    {
        Task<IEnumerable<Player>> GetAllPlayersAsync();
        Task<Player> GetPlayerByIdAsync(int id);
        Task UpdatePlayerAsync(Player player);
        Task DeletePlayerAsync(int id);
        Task AddPlayerAsync(Player player);
        Task<bool> IsNotExistingPlayerAsync(string firstName, string lastName, DateTime birthDate);
        Task<bool> IsNotExistingPlayerUpdateAsync(string firstName, string lastName, DateTime birthDate, int playerId);
    }
}