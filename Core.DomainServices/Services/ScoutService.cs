using Core.Domain;
using Core.DomainServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainServices.Services
{
    public class ScoutService : IScoutService
    {
        private readonly IScoutRepo _scoutRepo;

        public ScoutService(IScoutRepo scoutRepo)
        {
            _scoutRepo = scoutRepo;
        }
        Task<IEnumerable<Scout>> IScoutService.GetAllScoutsAsync()
        {
            return _scoutRepo.GetAllScoutsAsync();
        }
    }
}
