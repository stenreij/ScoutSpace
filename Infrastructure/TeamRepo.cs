using Core.Domain;
using Core.DomainServices;
using Core.DomainServices.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class TeamRepo : ITeamRepo
    {
        private readonly ScoutSpaceDbContext _context;

        public TeamRepo(ScoutSpaceDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Team>> GetAllTeamsAsync()
        {
            return await _context.Team
                .ToListAsync();
        }

        public async Task<Team> GetTeamByIdAsync(int id)
        {
            return await _context.Team
                .FirstAsync(t => t.teamId == id);
        }

        public async Task AddTeamAsync(Team team)
        {
            await _context.Team.AddAsync(team);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsTeamNameUniqueAsync(string teamName)
        {
            return !await _context.Team.AnyAsync(t => t.teamName == teamName);
        }

        public async Task UpdateTeamAsync(Team team)
        {
            var teamToUpdate = await _context.Team.FindAsync(team.teamId);

            if (teamToUpdate != null)
            {
                _context.Entry(teamToUpdate).CurrentValues.SetValues(team);
                await _context.SaveChangesAsync();
            }
        }

    }
}