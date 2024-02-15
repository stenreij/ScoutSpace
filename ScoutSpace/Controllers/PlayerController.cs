using Core.Domain;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ScoutSpace.Models;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Tesseract;

namespace ScoutSpace.Controllers
{
    public class PlayerController : Controller
    {
        private readonly ILogger<PlayerController> _logger;
        Uri baseAddress = new Uri("https://localhost:7296/api");
        HttpClient client;

        public PlayerController(ILogger<PlayerController> logger)
        {
            _logger = logger;
            client = new HttpClient();
            client.BaseAddress = baseAddress;
        }

        [HttpGet]
        public IActionResult PlayerList()
        {
            List<Player> playerList = new List<Player>();
            HttpResponseMessage response = client.GetAsync(client.BaseAddress + "/players").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                playerList = JsonConvert.DeserializeObject<List<Player>>(data);
            }
            return View(playerList);
        }

        [HttpGet]
        public IActionResult PlayerDetail(int id)
        {
            Console.WriteLine("playerdetail action called with id: " + id);
            Player player = new Player();

            HttpResponseMessage response = client.GetAsync($"{client.BaseAddress}/player/{id}").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                player = JsonConvert.DeserializeObject<Player>(data);
                return View(player);
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                return NotFound();
            }
        }

        [HttpGet]
        public IActionResult PlayerUpdate(int id)
        {
            Player player = new Player();
            HttpResponseMessage response = client.GetAsync($"{client.BaseAddress}/player/{id}").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                player = JsonConvert.DeserializeObject<Player>(data);

                var teamsResponse = client.GetAsync($"{client.BaseAddress}/teams").Result;
                if (teamsResponse.IsSuccessStatusCode)
                {
                    string teamsData = teamsResponse.Content.ReadAsStringAsync().Result;
                    var teams = JsonConvert.DeserializeObject<List<Team>>(teamsData);

                    ViewBag.Teams = teams;
                }
                else
                {
                    Console.WriteLine($"Error: {teamsResponse.StatusCode} - {teamsResponse.ReasonPhrase}");
                    return NotFound();
                }

                return View("PlayerUpdate", player);
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                return NotFound();
            }
        }

        [HttpPost]
        public IActionResult PlayerUpdate(Player updatedPlayer)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var id = updatedPlayer.playerId;
                    HttpResponseMessage getPlayerResponse = client.GetAsync($"{client.BaseAddress}/player/{updatedPlayer.playerId}").Result;

                    if (!getPlayerResponse.IsSuccessStatusCode)
                    {
                        Console.WriteLine($"Error: {getPlayerResponse.StatusCode} - {getPlayerResponse.ReasonPhrase}");
                        ViewBag.ErrorMessage = "Error retrieving player data";
                        return View();
                    }

                    string playerData = getPlayerResponse.Content.ReadAsStringAsync().Result;
                    Player currentPlayer = JsonConvert.DeserializeObject<Player>(playerData);

                    if(currentPlayer == null)
                    {
                        return NotFound();
                    }

                    currentPlayer.firstName = updatedPlayer.firstName;
                    currentPlayer.lastName = updatedPlayer.lastName;
                    currentPlayer.birthDate = updatedPlayer.birthDate;
                    currentPlayer.residence = updatedPlayer.residence;
                    currentPlayer.email = updatedPlayer.email;
                    currentPlayer.phoneNr = updatedPlayer.phoneNr;
                    currentPlayer.line = updatedPlayer.line;
                    currentPlayer.position = updatedPlayer.position;
                    currentPlayer.preferedFoot = updatedPlayer.preferedFoot;
                    currentPlayer.teamId = updatedPlayer.teamId;

                    HttpResponseMessage updateResponse = client.PutAsJsonAsync($"{client.BaseAddress}/player/{updatedPlayer.playerId}", currentPlayer).Result;

                    if (updateResponse.IsSuccessStatusCode)
                    {
                        return RedirectToAction("PlayerList");
                    }
                    else
                    {
                        Console.WriteLine($"Error: {getPlayerResponse.StatusCode} - {getPlayerResponse.ReasonPhrase}");
                        ViewBag.ErrorMessage = "Error retrieving player data";
                        return View();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return View();
            }

            return RedirectToAction("PlayerList"); 
        }

        [HttpPost]
        public IActionResult PlayerDelete(int id)
        {
            try
            {
                HttpResponseMessage response = client.DeleteAsync($"{client.BaseAddress}/player/{id}").Result;

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("PlayerList");
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
        public IActionResult PlayerAdd()
        {
            try
            {
                HttpResponseMessage teamsResponse = client.GetAsync($"{client.BaseAddress}/teams").Result;

                if (teamsResponse.IsSuccessStatusCode)
                {
                    string teamsData = teamsResponse.Content.ReadAsStringAsync().Result;
                    var teams = JsonConvert.DeserializeObject<List<Team>>(teamsData);

                    ViewBag.Teams = teams;
                }
                else
                {
                    Console.WriteLine($"Error: {teamsResponse.StatusCode} - {teamsResponse.ReasonPhrase}");
                    return NotFound();
                }

                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return View();
            }
        }
        [HttpPost]
        public IActionResult PlayerAdd(Player newPlayer)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    HttpResponseMessage teamsResponse = client.GetAsync($"{client.BaseAddress}/teams").Result;

                    if (!teamsResponse.IsSuccessStatusCode)
                    {
                        Console.WriteLine($"Error: {teamsResponse.StatusCode} - {teamsResponse.ReasonPhrase}");
                        ViewBag.ErrorMessage = "Error retrieving team data";
                        return View();
                    }

                    string teamsData = teamsResponse.Content.ReadAsStringAsync().Result;
                    var teams = JsonConvert.DeserializeObject<List<Team>>(teamsData);

                    ViewBag.Teams = teams;

                    HttpResponseMessage addPlayerResponse = client.PostAsJsonAsync($"{client.BaseAddress}/player", newPlayer).Result;

                    if (addPlayerResponse.IsSuccessStatusCode)
                    {
                        return RedirectToAction("PlayerList");
                    }
                    else
                    {
                        Console.WriteLine($"Error: {addPlayerResponse.StatusCode} - {addPlayerResponse.ReasonPhrase}");
                        ViewBag.ErrorMessage = "Error adding player";
                        return View();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return View();
            }

            return View();
        }

        //TRYING TO CONVERT IMAGE TO TEXT
        [HttpPost]
        public IActionResult ProcessImage(IFormFile imageUpload)
        {
            if (imageUpload != null && imageUpload.Length > 0)
            {
                try
                {
                    // Voer OCR uit om alleen de kop/header te extraheren
                    string extractedHeader = ExtractHeader(imageUpload);

                    // Stel de ViewBag in met de geëxtraheerde kop/header
                    ViewBag.ExtractedText = extractedHeader;

                    return View("ExtractedText");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    return RedirectToAction("PlayerAdd");
                }
            }

            return View();
        }

        private string ExtractHeader(IFormFile imageUpload)
        {
            using (var stream = imageUpload.OpenReadStream())
            {
                // Gebruik een OCR-bibliotheek om alleen de kop/header te extraheren
                string extractedText = YourOCRFunction(stream);

                // Splits de tekst op basis van regelovergangen
                string[] lines = extractedText.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                // Neem alleen de eerste twee regels (de kop/header)
                string extractedHeader = string.Join("\n", lines.Take(2));

                return extractedHeader;
            }
        }

        private string YourOCRFunction(Stream imageStream)
        {
            try
            {
                using (var engine = new TesseractEngine(@"tessdata", "eng", EngineMode.Default))
                {
                    using (var img = Pix.LoadFromMemory(ReadStream(imageStream)))
                    {
                        using (var page = engine.Process(img))
                        {
                            // Haal de ruwe geëxtraheerde tekst op
                            string rawText = page.GetText();

                            // Schrijf de ruwe tekst naar de console om te inspecteren
                            Console.WriteLine("Raw Text:");
                            Console.WriteLine(rawText);

                            // Retourneer de ruwe tekst
                            return rawText;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during OCR processing: {ex.Message}");
                throw;
            }
        }

        private byte[] ReadStream(Stream stream)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                return ms.ToArray();
            }
        }

    }
}