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
        Uri baseAddress = new Uri("https://localhost:7296/api/players");
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
            HttpResponseMessage response = client.GetAsync(client.BaseAddress).Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                playerList = JsonConvert.DeserializeObject<List<Player>>(data);
            }
            return View(playerList);
        }

        public IActionResult PlayerDetail(int id)
        {
            Player player = new Player();
            HttpResponseMessage response = client.GetAsync($"/{id}").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                player = JsonConvert.DeserializeObject<Player>(data);
                return View(player);
            }
            else
            {
                return NotFound();
            }
        }
    }
}