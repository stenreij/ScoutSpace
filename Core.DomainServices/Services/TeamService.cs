using Core.Domain;
using Core.DomainServices.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainServices.Services
{
    public class TeamService : ITeamService
    {
        private readonly ITeamRepo _teamRepo;

        public TeamService(ITeamRepo teamRepo)
        {
            _teamRepo = teamRepo;
        }

        Task<IEnumerable<Team>> ITeamService.GetAllTeamsAsync()
        {
            return _teamRepo.GetAllTeamsAsync();
        }
        Task<Team> ITeamService.GetTeamByIdAsync(int id)
        {
            return _teamRepo.GetTeamByIdAsync(id);
        }

        public async Task AddTeamAsync(Team team)
        {
            bool isTeamNameUnique = await _teamRepo.IsTeamNameUniqueAsync(team.teamName);

            if (!isTeamNameUnique)
            {
                throw new InvalidOperationException($"Team met naam {team.teamName} bestaat al.");
            }

            await _teamRepo.AddTeamAsync(team);
        }

        public Task<bool> IsTeamNameUniqueAsync(string teamName)
        {
            return _teamRepo.IsTeamNameUniqueAsync(teamName);
        }
    }
}
