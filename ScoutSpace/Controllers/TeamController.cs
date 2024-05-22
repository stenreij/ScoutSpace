using Azure;
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
        public async Task<IActionResult> TeamAdd(Team newTeam)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    HttpResponseMessage getAllTeamsResponse = await client.GetAsync($"{client.BaseAddress}/teams");

                    if (getAllTeamsResponse.IsSuccessStatusCode)
                    {
                        string teamsData = await getAllTeamsResponse.Content.ReadAsStringAsync();
                        List<Team> allTeams = JsonConvert.DeserializeObject<List<Team>>(teamsData);

                        if (allTeams.Any(team => team.teamName.Equals(newTeam.teamName, StringComparison.OrdinalIgnoreCase) && team.teamId != newTeam.teamId))
                        {
                            ViewBag.ErrorMessage = "Team met deze naam bestaat al.";
                            return View(newTeam);
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Error: {getAllTeamsResponse.StatusCode} - {getAllTeamsResponse.ReasonPhrase}");
                        ViewBag.ErrorMessage = "Error retrieving team data";
                        return View(newTeam);
                    }

                    HttpResponseMessage addTeamResponse = await client.PostAsJsonAsync($"{client.BaseAddress}/team", newTeam);

                    if (addTeamResponse.IsSuccessStatusCode)
                    {
                        return RedirectToAction("TeamList");
                    }
                    else
                    {
                        Console.WriteLine($"Error: {addTeamResponse.StatusCode} - {addTeamResponse.ReasonPhrase}");
                        ViewBag.ErrorMessage = "Error adding team";
                        return View(newTeam);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                ViewBag.ErrorMessage = "An unexpected error occurred";
                return View(newTeam);
            }

            return View(newTeam);
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

        [HttpPost]
        public async Task<IActionResult> TeamPromotionRelegation(Dictionary<int, TeamPromotionRelegation> updateModels)
        {
            try
            {
                foreach (var (teamId, updateModel) in updateModels)
                {
                    HttpResponseMessage existingTeam = await client.GetAsync($"{client.BaseAddress}/team/{teamId}");

                    if (existingTeam == null)
                    {
                        return NotFound($"Team met ID {teamId} niet gevonden.");
                    }

                    HttpResponseMessage response = client.PutAsJsonAsync($"{client.BaseAddress}/teams/divisionupdate", updateModel).Result;
                    response.EnsureSuccessStatusCode();
                }

                return RedirectToAction("TeamPromotionRelegation");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fout bij het bijwerken van divisies: {ex.Message}");
                return StatusCode(500, "Er is een interne fout opgetreden bij het bijwerken van divisies.");
            }
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

        [HttpPost]
        public async Task<IActionResult> TeamUpdate(Team updatedTeam)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    HttpResponseMessage getAllTeamsResponse = await client.GetAsync($"{client.BaseAddress}/teams");
                    if (getAllTeamsResponse.IsSuccessStatusCode)
                    {
                        string teamsData = await getAllTeamsResponse.Content.ReadAsStringAsync();
                        List<Team> allTeams = JsonConvert.DeserializeObject<List<Team>>(teamsData);

                        if (allTeams.Any(team => team.teamName.Equals(updatedTeam.teamName, StringComparison.OrdinalIgnoreCase) && team.teamId != updatedTeam.teamId))
                        {
                            ViewBag.ErrorMessage = "Team met deze naam bestaat al.";
                            return View(updatedTeam);
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Error: {getAllTeamsResponse.StatusCode} - {getAllTeamsResponse.ReasonPhrase}");
                        ViewBag.ErrorMessage = "Error retrieving team data";
                        return View(updatedTeam);
                    }

                    var id = updatedTeam.teamId;
                    HttpResponseMessage getTeamResponse = await client.GetAsync($"{client.BaseAddress}/team/{updatedTeam.teamId}");

                    if (!getTeamResponse.IsSuccessStatusCode)
                    {
                        Console.WriteLine($"Error: {getTeamResponse.StatusCode} - {getTeamResponse.ReasonPhrase}");
                        ViewBag.ErrorMessage = "Error retrieving team data";
                        return View();
                    }

                    string teamData = await getTeamResponse.Content.ReadAsStringAsync();
                    Team currentTeam = JsonConvert.DeserializeObject<Team>(teamData);

                    if (currentTeam == null)
                    {
                        return NotFound();
                    }

                    currentTeam.teamName = updatedTeam.teamName;
                    currentTeam.contactNr = updatedTeam.contactNr;
                    currentTeam.division = updatedTeam.division;
                    currentTeam.city = updatedTeam.city;

                    HttpResponseMessage updateResponse = await client.PutAsJsonAsync($"{client.BaseAddress}/team/{updatedTeam.teamId}", currentTeam);

                    if (updateResponse.IsSuccessStatusCode)
                    {
                        return RedirectToAction("TeamList");
                    }
                    else
                    {
                        Console.WriteLine($"Error: {updateResponse.StatusCode} - {updateResponse.ReasonPhrase}");
                        ViewBag.ErrorMessage = "Error updating team data";
                        return View();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                ViewBag.ErrorMessage = "An unexpected error occurred";
                return View();
            }

            return RedirectToAction("TeamList");
        }

        [HttpPost]
        public IActionResult TeamDelete(int id)
        {
            try
            {
                HttpResponseMessage response = client.DeleteAsync($"{client.BaseAddress}/team/{id}").Result;

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("TeamList");
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
        public IActionResult TeamDetail(int id)
        {
            Console.WriteLine("teamdetail action called with id: " + id);
            Team team = new Team();
            List<Player> players = new List<Player>();

            HttpResponseMessage teamResponse = client.GetAsync($"{client.BaseAddress}/team/{id}").Result;

            if (teamResponse.IsSuccessStatusCode)
            {
                string teamData = teamResponse.Content.ReadAsStringAsync().Result;
                team = JsonConvert.DeserializeObject<Team>(teamData);
            }
            else
            {
                Console.WriteLine($"Error: {teamResponse.StatusCode} - {teamResponse.ReasonPhrase}");
                return NotFound();
            }

            HttpResponseMessage playersResponse = client.GetAsync($"{client.BaseAddress}/players").Result;
            if (playersResponse.IsSuccessStatusCode)
            {
                string playersData = playersResponse.Content.ReadAsStringAsync().Result;
                players = JsonConvert.DeserializeObject<List<Player>>(playersData);

                players = players.Where(p => p.teamId == id).ToList();

                ViewBag.Keepers = players.Where(p => p.line == Line.Keeper).ToList();
                ViewBag.Verdedigers = players.Where(p => p.line == Line.Verdediger).ToList();
                ViewBag.Middenvelders = players.Where(p => p.line == Line.Middenvelder).ToList();
                ViewBag.Aanvallers = players.Where(p => p.line == Line.Aanvaller).ToList();
            }
            else
            {
                Console.WriteLine($"Error: {playersResponse.StatusCode} - {playersResponse.ReasonPhrase}");
            }

            return View(team);
        }

    }
}