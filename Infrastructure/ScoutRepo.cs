using Core.Domain;
using Core.DomainServices;

namespace Infrastructure
{
    public class ScoutRepo : IScoutRepo
    {
        private readonly ScoutSpaceDbContext _context;

        public ScoutRepo(ScoutSpaceDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Scout> GetScouts()
        {
            return _context.Scout.ToList();
        }
    }
}