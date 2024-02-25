using Core.Domain;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ScoutSpace.Models;
using System.Diagnostics;
using System.Net;

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

        [HttpPost]
        public IActionResult TeamAdd(Team newTeam)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    HttpResponseMessage addTeamResponse = client.PostAsJsonAsync($"{client.BaseAddress}/team", newTeam).Result;

                    if (addTeamResponse.IsSuccessStatusCode)
                    {
                        return RedirectToAction("TeamList");
                    }
                    else
                    {
                        Console.WriteLine($"Error: {addTeamResponse.StatusCode} - {addTeamResponse.ReasonPhrase}");
                        ViewBag.ErrorMessage = "Error adding team";
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

        [HttpGet]
        public IActionResult TeamPromotionRelegation()
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

        [HttpGet("team/update/{id}")]
        public async Task<IActionResult> TeamUpdate(int id)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync($"{client.BaseAddress}/team/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        return NotFound($"Team met ID {id} niet gevonden.");
                    }
                    else
                    {
                        return StatusCode((int)response.StatusCode, "Er is een interne fout opgetreden bij het ophalen van team.");
                    }
                }

                string data = await response.Content.ReadAsStringAsync();
                Team team = JsonConvert.DeserializeObject<Team>(data);

                return View("TeamUpdate", team);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fout bij het ophalen van team: {ex.Message}");
                return StatusCode(500, "Er is een interne fout opgetreden bij het ophalen van team.");
            }
        }

    }
}