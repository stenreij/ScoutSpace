using Core.Domain;
using Core.DomainServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainServices.Services
{
    public class NoteService : INoteService
    {
        private readonly INoteRepo _noteRepo;

        public NoteService(INoteRepo noteRepo)
        {
            _noteRepo = noteRepo;
        }
        Task<IEnumerable<Note>> INoteService.GetAllNotesAsync()
        {
            return _noteRepo.GetAllNotesAsync();
        }
        Task<Note> INoteService.GetNoteByIdAsync(int id)
        {
            return _noteRepo.GetNoteByIdAsync(id);
        }

        public async Task AddNoteAsync(Note note)
        {
            bool isNoteTitleUnique = await _noteRepo.IsNoteTitleUniqueAsync(note.title, note.playerId);

            if (!isNoteTitleUnique)
            {
                throw new InvalidOperationException($"Notitie met titel '{note.title}' bestaat al.");
            }

            await _noteRepo.AddNoteAsync(note);
        }

        public async Task UpdateNoteAsync(Note note)
        {
            bool isNoteTitleUnique = await _noteRepo.IsNoteTitleUniqueAsync(note.title, note.playerId);

            if (!isNoteTitleUnique)
            {
                throw new InvalidOperationException($"Notitie met titel '{note.title}' bestaat al voor deze speler.");
            }

            await _noteRepo.UpdateNoteAsync(note);
        }

        Task INoteService.DeleteNoteAsync(int id)
        {
            return _noteRepo.DeleteNoteAsync(id);
        }

        public Task<bool> IsNoteTitleUniqueAsync(string noteTitle, int playerId)
        {
            return _noteRepo.IsNoteTitleUniqueAsync(noteTitle, playerId);
        }

    }
}
