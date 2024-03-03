using Core.Domain;

namespace Core.DomainServices.Interfaces
{
    public interface INoteRepo
    {
        Task<IEnumerable<Note>> GetAllNotesAsync();
        Task AddNoteAsync(Note note);
        Task<bool> IsNoteTitleUniqueAsync(string noteTitle, int playerId);

    }
}