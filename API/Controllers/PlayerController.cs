using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Core.Domain;
using Core.DomainServices;
using Core.DomainServices.Interfaces;
using Core.DomainServices.Services;

namespace API.Controllers
{
    [ApiController]
    [Route("api/")]
    public class PlayerController : Controller
    {
        private readonly ILogger<PlayerController> _logger;
        private readonly IPlayerService _playerService;
        private readonly ITeamService _teamService;

        public PlayerController(
            ILogger<PlayerController> logger, 
            IPlayerService playerService,
            ITeamService teamService)
        {
            _logger = logger;
            _playerService = playerService;
            _teamService = teamService;
        }

        [HttpGet("players")]
        public async Task<IActionResult> GetAllPlayers()
        {
            _logger.LogInformation("GetAllPlayers() aangeroepen");

            try
            {
                var players = await _playerService.GetAllPlayersAsync();
                return Ok(players);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fout bij het ophalen van players: {ex.Message}");
                return StatusCode(500, "Er is een interne fout opgetreden bij het ophalen van players.");
            }
        }

        [HttpGet("player/{id}")]
        public async Task<IActionResult> GetPlayerByIdAsync(int id)
        {
            _logger.LogInformation("GetPlayerById() aangeroepen");

            try
            {
                var player = await _playerService.GetPlayerByIdAsync(id);
                return Ok(player);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fout bij het ophalen van player: {ex.Message}");
                return StatusCode(500, "Er is een interne fout opgetreden bij het ophalen van player.");
            }
        }

        [HttpPut("player/{id}")]
        public async Task<IActionResult> UpdatePlayerAsync(int id, [FromBody] Player updatedPlayer)
        {
            _logger.LogInformation($"UpdatePlayerAsync() aangeroepen voor speler met ID: {id}");

            if (updatedPlayer == null)
            {
                return BadRequest();
            }

            updatedPlayer.playerId = id;

            try
            {
                await _playerService.UpdatePlayerAsync(updatedPlayer);

                if (updatedPlayer == null)
                {
                    return NotFound();
                }

                return Ok(updatedPlayer);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fout bij het bijwerken van de speler: {ex.Message}");
                return NotFound($"Speler met ID {id} niet gevonden.");
            }
        }

        [HttpPut("player/{id}/transfer")]
        public async Task<IActionResult> TransferPlayerAsync(int id, [FromBody] TransferPlayer transferModel)
        {
            _logger.LogInformation($"TransferPlayerAsync() aangeroepen voor speler met ID: {id}");
            _logger.LogInformation($"TransferPlayerAsync() aangeroepen voor team met ID: {transferModel.newTeamId}");

            try
            {
                var player = await _playerService.GetPlayerByIdAsync(id);
                if (player == null)
                {
                    return NotFound($"Speler met ID {id} niet gevonden.");
                }

                var newTeam = await _teamService.GetTeamByIdAsync(transferModel.newTeamId);
                if (newTeam == null)
                {
                    _logger.LogError($"Team met ID {transferModel.newTeamId} niet gevonden.");
                    return NotFound($"Team met ID {transferModel.newTeamId} niet gevonden.");
                }

                player.teamId = transferModel.newTeamId;
                player.team = newTeam;

                await _playerService.UpdatePlayerAsync(player);
                return Ok(player);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fout bij het overdragen van de speler naar een ander team: {ex.ToString()}");
                return StatusCode(500, "Er is een interne fout opgetreden bij het overdragen van de speler naar een ander team.");
            }
        }

        [HttpDelete("player/{id}")]
        public async Task<IActionResult> DeletePlayerAsync(int id)
        {
            _logger.LogInformation($"DeletePlayerAsync() aangeroepen voor speler met ID: {id}");

            try
            {
                var player = await _playerService.GetPlayerByIdAsync(id);
                if (player == null)
                {
                    _logger.LogWarning($"Speler met id {id} niet gevonden");
                    return NotFound("Speler met dit ID is niet gevonden.");
                }

                await _playerService.DeletePlayerAsync(id);
                return Ok("Speler met ID " + id + " verwijderd");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fout bij het verwijderen van de speler: {ex.Message}");
                return StatusCode(500, "Er is een interne fout opgetreden bij het verwijderen van de speler.");
            }
        }

        [HttpPost("player")]
        public async Task <IActionResult> AddPlayerAsync([FromBody] Player player)
        {
            _logger.LogInformation($"AddPlayerAsync() aangeroepen");

            try
            {
                await _playerService.AddPlayerAsync(player);
                return Ok(player);
            }
            catch(InvalidOperationException)
            {
                _logger.LogError($"Speler met {player.firstName} {player.lastName} {player.birthDate} bestaat al.");
                return BadRequest($"Een speler met {player.firstName} {player.lastName} {player.birthDate}");
            }
            catch(Exception ex)
            {
                _logger.LogError($"Fout bij het toevoegen van de speler: {ex.Message}");
                return StatusCode(500, "Er is een interne fout opgetreden bij het toevoegen van een speler.");
            }
        }
        
    }
}
