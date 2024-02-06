using Core.Domain;

namespace Core.DomainServices.Interfaces
{
    public interface ITeamRepo
    {
        Task<IEnumerable<Team>> GetAllTeamsAsync();
    }
}