using Core.Domain;
using Core.DomainServices.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class ScoutRepo : IScoutRepo
    {
        private readonly ScoutSpaceDbContext _context;

        public ScoutRepo(ScoutSpaceDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Scout>> GetAllScoutsAsync()
        {
            return await _context.Scout
                .ToListAsync();
        }
    }
}