using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Core.Domain;
using Core.DomainServices;
using Core.DomainServices.Interfaces;

namespace API.Controllers
{
    [ApiController]
    [Route("api/")]
    public class PlayerController : Controller
    {
        private readonly ILogger<PlayerController> _logger;
        private readonly IPlayerService _playerService;

        public PlayerController(ILogger<PlayerController> logger, IPlayerService playerService)
        {
            _logger = logger;
            _playerService = playerService;
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
            catch(Exception ex)
            {
                _logger.LogError($"Fout bij het toevoegen van de speler: {ex.Message}");
                return StatusCode(500, "Er is een interne fout opgetreden bij het toevoegen van een speler.");
            }
        }
        
    }
}
