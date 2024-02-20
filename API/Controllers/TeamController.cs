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
    public class TeamController : Controller
    {
        private readonly ILogger<TeamController> _logger;
        private readonly ITeamService _teamService;

        public TeamController(ILogger<TeamController> logger, ITeamService teamService)
        {
            _logger = logger;
            _teamService = teamService;
        }

        [HttpGet("teams")]
        public async Task<IActionResult> GetAllTeams()
        {
            _logger.LogInformation("GetAllTeams() aangeroepen");

            try
            {
                var teams = await _teamService.GetAllTeamsAsync();
                return Ok(teams);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fout bij het ophalen van teams: {ex.Message}");
                return StatusCode(500, "Er is een interne fout opgetreden bij het ophalen van teams.");
            }
        }

        [HttpGet("team/{id}")]
        public async Task<IActionResult> getTeamById(int id)
        {
            _logger.LogInformation("GetTeamById() aangeroepen");

            try
            {
                var team = await _teamService.GetTeamByIdAsync(id);
                return Ok(team);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fout bij het ophalen van team: {ex.Message}");
                return StatusCode(500, "Er is een interne fout opgetreden bij het ophalen van team.");
            }
        }
    }
}
