using Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainServices.Interfaces
{
    public interface INoteService
    {
        Task<IEnumerable<Note>> GetAllNotesAsync();
        Task<Note> GetNoteByIdAsync(int id);
        Task DeleteNoteAsync(int id);
        Task UpdateNoteAsync(Note note);
        Task AddNoteAsync(Note note);
        Task<bool> IsNoteTitleUniqueAsync(string noteTitle, int playerId, int noteId);

    }
}
