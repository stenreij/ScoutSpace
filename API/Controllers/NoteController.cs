using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Core.Domain;
using Core.DomainServices.Interfaces;
using Core.DomainServices.Services;

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

        [HttpGet("note/{id}")]
        public async Task<IActionResult> GetNoteByIdAsync(int id)
        {
            _logger.LogInformation("GetNoteById() aangeroepen");

            try
            {
                var note = await _noteService.GetNoteByIdAsync(id);
                return Ok(note);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fout bij het ophalen van notitie: {ex.Message}");
                return StatusCode(500, "Er is een interne fout opgetreden bij het ophalen van notitie.");
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

        [HttpPut("note/{id}")]
        public async Task<IActionResult> UpdateNoteAsync(int id, [FromBody] Note updatedNote)
        {
            _logger.LogInformation($"UpdateNoteAsync() aangeroepen voor notitie met ID: {id}");

            if (updatedNote == null)
            {
                return BadRequest();
            }

            updatedNote.noteId = id;

            try
            {
                await _noteService.UpdateNoteAsync(updatedNote);          
                return Ok(updatedNote);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError($"Notitie met titel {updatedNote.title} bestaat al.");
                return BadRequest($"Een notitie met de naam {updatedNote.title} bestaat al. Kies een andere titel.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fout bij het bijwerken van notitie: {ex.Message}");
                return NotFound($"Notitie met ID {id} niet gevonden.");
            }
        }

        [HttpDelete("note/{id}")]
        public async Task<IActionResult> DeleteNoteAsync(int id)
        {
            _logger.LogInformation($"DeleteNoteAsync() aangeroepen voor notitie met ID: {id}");

            try
            {
                var note = await _noteService.GetNoteByIdAsync(id);
                if (note == null)
                {
                    _logger.LogWarning($"Notitie met id {id} niet gevonden");
                    return NotFound("Notitie met dit ID is niet gevonden.");
                }

                await _noteService.DeleteNoteAsync(id);
                return Ok("Notitie met ID " + id + " verwijderd");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fout bij het verwijderen van notitie: {ex.Message}");
                return StatusCode(500, "Er is een interne fout opgetreden bij het verwijderen van notitie.");
            }
        }

    }
}
