using Core.Domain;

namespace Core.DomainServices.Interfaces
{
    public interface INoteRepo
    {
        Task<IEnumerable<Note>> GetAllNotesAsync();
    }
}