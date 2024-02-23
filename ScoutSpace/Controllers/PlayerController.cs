using Core.Domain;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ScoutSpace.Models;
using System.Diagnostics;
using System.Net.Http.Headers;

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

                    if (currentPlayer == null)
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

        [HttpGet]
        public IActionResult PlayerTransfer()
        {
            try
            {
                HttpResponseMessage playersResponse = client.GetAsync($"{client.BaseAddress}/players").Result;
                HttpResponseMessage teamsResponse = client.GetAsync($"{client.BaseAddress}/teams").Result;

                if (playersResponse.IsSuccessStatusCode)
                {
                    string playersData = playersResponse.Content.ReadAsStringAsync().Result;
                    var players = JsonConvert.DeserializeObject<List<Player>>(playersData);

                    ViewBag.Players = players;


                    string teamsData = teamsResponse.Content.ReadAsStringAsync().Result;
                    var teams = JsonConvert.DeserializeObject<List<Team>>(teamsData);

                    ViewBag.Teams = teams;


                    return View();
                }
                else
                {
                    Console.WriteLine($"Error: {playersResponse.StatusCode} - {playersResponse.ReasonPhrase}");
                    Console.WriteLine($"Error: {teamsResponse.StatusCode} - {teamsResponse.ReasonPhrase}");

                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Transfer(int selectedPlayerId, TransferPlayer transferModel)
        {
            try
            {
                if (transferModel == null || transferModel.newTeamId == null)
                {
                    return RedirectToAction("TransferError");
                }

                var playerId = selectedPlayerId;


                HttpResponseMessage response = client.PutAsJsonAsync($"{client.BaseAddress}/player/{playerId}/transfer", transferModel).Result;

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("PlayerList");
                }
                else
                {
                    return View("PlayerTransfer");
                }
            }
            catch (Exception ex)
            {
                return View("PlayerTransfer");
            }
        }

    }
}
