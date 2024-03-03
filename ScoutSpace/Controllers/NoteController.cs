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

    }
}