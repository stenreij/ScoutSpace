using Core.Domain;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ScoutSpace.Models;
using System.Diagnostics;

namespace ScoutSpace.Controllers
{
    public class TeamController : Controller
    {
        private readonly ILogger<TeamController> _logger;
        Uri baseAddress = new Uri("https://localhost:7296/api");
        HttpClient client;

        public TeamController(ILogger<TeamController> logger)
        {
            _logger = logger;
            client = new HttpClient();
            client.BaseAddress = baseAddress;
        }
        public IActionResult TeamList()
        {
            List<Team> teamList = new List<Team>();
            HttpResponseMessage response = client.GetAsync(client.BaseAddress + "/teams").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                teamList = JsonConvert.DeserializeObject<List<Team>>(data);
            }
            return View(teamList);
        }

        [HttpGet]
        public IActionResult TeamAdd()
        {
            return View();
        }

    }
}