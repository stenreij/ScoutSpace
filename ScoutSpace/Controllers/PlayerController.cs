using Core.Domain;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ScoutSpace.Models;
using System.Diagnostics;

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

            // Construct the URL using the base address and id

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
    }
}