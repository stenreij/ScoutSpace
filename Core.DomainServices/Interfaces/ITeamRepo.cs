using Core.Domain;

namespace Core.DomainServices.Interfaces
{
    public interface ITeamRepo
    {
        Task<IEnumerable<Team>> GetAllTeamsAsync();
        Task<Team> GetTeamByIdAsync(int id);
        Task AddTeamAsync(Team team);
        Task<bool> IsTeamNameUniqueAsync(string teamName);
        Task UpdateTeamAsync(Team team);
    }
}