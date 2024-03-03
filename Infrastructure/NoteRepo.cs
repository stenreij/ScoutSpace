using Core.Domain;
using Core.DomainServices.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class NoteRepo : INoteRepo
    {
        private readonly ScoutSpaceDbContext _context;

        public NoteRepo(ScoutSpaceDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Note>> GetAllNotesAsync()
        {
            return await _context.Note
                .ToListAsync();
        }
    }
}