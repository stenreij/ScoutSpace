using Core.Domain;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ScoutSpace.Models;
using System.Diagnostics;

namespace ScoutSpace.Controllers
{
    public class NoteController : Controller
    {
        private readonly ILogger<NoteController> _logger;
        Uri baseAddress = new Uri("https://localhost:7296/api");
        HttpClient client;

        public NoteController(ILogger<NoteController> logger)
        {
            _logger = logger;
            client = new HttpClient();
            client.BaseAddress = baseAddress;
        }

        [HttpGet]
        public IActionResult NoteAdd(int playerId)
        {
            ViewData["playerId"] = playerId;
            return View();
        }

        [HttpPost]
        public IActionResult NoteAdd(Note note)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    HttpResponseMessage addNoteResponse = client.PostAsJsonAsync($"{client.BaseAddress}/note", note).Result;

                    if (addNoteResponse.IsSuccessStatusCode)
                    {
                        return RedirectToAction("PlayerDetail", "Player", new { id = note.playerId });
                    }
                    else
                    {
                        Console.WriteLine($"Error: {addNoteResponse.StatusCode} - {addNoteResponse.ReasonPhrase}");
                        ViewBag.ErrorMessage = "Notitie met deze titel bestaat al.";
                        ViewData["playerId"] = note.playerId;
                        return View(note);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                ViewBag.ErrorMessage = "Er is een fout opgetreden bij het toevoegen van de notitie.";
                ViewData["playerId"] = note.playerId;
                return View(note);
            }
            ViewData["playerId"] = note.playerId;
            return View(note);
        }

        [HttpPost]
        public IActionResult NoteDelete(int id, int playerId)
        {
            try
            {
                HttpResponseMessage response = client.DeleteAsync($"{client.BaseAddress}/note/{id}").Result;

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("PlayerDetail", "Player", new { id = playerId });
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return View();
            }
        }

        [HttpGet]
        public IActionResult NoteUpdate(int id, int playerId)
        {
            Note note = new Note();
            HttpResponseMessage response = client.GetAsync($"{client.BaseAddress}/note/{id}").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                note = JsonConvert.DeserializeObject<Note>(data);
                ViewData["playerId"] = playerId;

                return View("NoteUpdate", note);
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> NoteUpdate(Note updatedNote)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    HttpResponseMessage getAllNotesResponse = await client.GetAsync($"{client.BaseAddress}/notes");
                    if (getAllNotesResponse.IsSuccessStatusCode)
                    {
                        string notesData = await getAllNotesResponse.Content.ReadAsStringAsync();
                        List<Note> allNotes = JsonConvert.DeserializeObject<List<Note>>(notesData);

                        var id = updatedNote.noteId;
                        HttpResponseMessage getNoteResponse = await client.GetAsync($"{client.BaseAddress}/note/{updatedNote.noteId}");

                        if (!getNoteResponse.IsSuccessStatusCode)
                        {
                            Console.WriteLine($"Error: {getNoteResponse.StatusCode} - {getNoteResponse.ReasonPhrase}");
                            ViewBag.ErrorMessage = "Error retrieving notitie data";
                            ViewData["playerId"] = updatedNote.playerId;
                            return View();
                        }

                        string noteData = await getNoteResponse.Content.ReadAsStringAsync();
                        Note currentNote = JsonConvert.DeserializeObject<Note>(noteData);

                        if (currentNote == null)
                        {
                            return NotFound();
                        }

                        currentNote.title = updatedNote.title;
                        currentNote.description = updatedNote.description;

                        HttpResponseMessage updateResponse = await client.PutAsJsonAsync($"{client.BaseAddress}/note/{updatedNote.noteId}", currentNote);

                        if (updateResponse.IsSuccessStatusCode)
                        {
                            return RedirectToAction("PlayerDetail", "Player", new { id = currentNote.playerId });
                        }
                        else
                        {
                            Console.WriteLine($"Error: {updateResponse.StatusCode} - {updateResponse.ReasonPhrase}");
                            ViewBag.ErrorMessage = "Notitie met deze titel bestaat al voor deze speler.";
                            ViewData["playerId"] = updatedNote.playerId;
                            return View(currentNote);
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Error: {getAllNotesResponse.StatusCode} - {getAllNotesResponse.ReasonPhrase}");
                        ViewBag.ErrorMessage = "Error retrieving notitie data";
                        ViewData["playerId"] = updatedNote.playerId;
                        return View(updatedNote);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                ViewBag.ErrorMessage = "An unexpected error occurred";
                return View();
            }
            ViewData["playerId"] = updatedNote.playerId;
            return View(updatedNote);
        }

    }
}