using Core.Domain;

namespace Core.DomainServices.Interfaces
{
    public interface IPlayerRepo
    {
        Task<IEnumerable<Player>> GetAllPlayersAsync();
    }
}