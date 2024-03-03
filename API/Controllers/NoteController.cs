using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Core.Domain;
using Core.DomainServices.Interfaces;

namespace API.Controllers
{
    [ApiController]
    [Route("api/")]
    public class NoteController : Controller
    {
        private readonly ILogger<NoteController> _logger;
        private readonly INoteService _noteService;

        public NoteController(ILogger<NoteController> logger, INoteService noteService)
        {
            _logger = logger;
            _noteService = noteService;
        }

        [HttpGet("notes")]
        public async Task<IActionResult> GetAllNotes()
        {
            _logger.LogInformation("GetAllNotes() aangeroepen");

            try
            {
                var notes = await _noteService.GetAllNotesAsync();
                return Ok(notes);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fout bij het ophalen van notes: {ex.Message}");
                return StatusCode(500, "Er is een interne fout opgetreden bij het ophalen van notes.");
            }
        }

        [HttpPost("note")]
        public async Task<IActionResult> AddNoteAsync([FromBody] Note note)
        {
            _logger.LogInformation($"AddNoteAsync() aangeroepen");

            try
            {
                await _noteService.AddNoteAsync(note);
                return Ok(note);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError($"Notitie met titel {note.title} bestaat al.");
                return BadRequest($"Een notitie met de naam {note.title} bestaat al. Kies een andere titel.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fout bij het toevoegen van notitie: {ex.Message}");
                return StatusCode(500, "Er is een interne fout opgetreden bij het toevoegen van notitie.");
            }
        }
      
    }
}
