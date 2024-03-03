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
    }
}
