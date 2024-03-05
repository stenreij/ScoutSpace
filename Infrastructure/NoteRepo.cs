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

        public async Task<Note> GetNoteByIdAsync(int id)
        {
            return await _context.Note
                .FirstAsync(n => n.noteId == id);
        }

        public async Task AddNoteAsync(Note note)
        {
            await _context.Note.AddAsync(note);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteNoteAsync(int id)
        {
            var noteToDelete = await _context.Note.FindAsync(id);

            if (noteToDelete != null)
            {
                _context.Remove(noteToDelete);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateNoteAsync(Note note)
        {
            var noteToUpdate = await _context.Note.FindAsync(note.noteId);

            if (noteToUpdate != null)
            {
                _context.Entry(noteToUpdate).CurrentValues.SetValues(note);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> IsNoteTitleUniqueAsync(string noteTitle, int playerId)
        {
            return !await _context.Note.AnyAsync(n => n.title == noteTitle && n.playerId == playerId);
        }

    }
}