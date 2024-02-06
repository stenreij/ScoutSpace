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
    }
}
