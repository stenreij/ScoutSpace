using Core.Domain;
using Core.DomainServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
