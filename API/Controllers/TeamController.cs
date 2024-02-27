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

        [HttpPost("team")]
        public async Task<IActionResult> AddTeamAsync([FromBody] Team team)
        {
            _logger.LogInformation($"AddTeamAsync() aangeroepen");

            try
            {
                await _teamService.AddTeamAsync(team);
                return Ok(team);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError($"Team met naam {team.teamName} bestaat al.");
                return BadRequest($"Een team met de naam {team.teamName} bestaat al. Kies een andere naam.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fout bij het toevoegen van team: {ex.Message}");
                return StatusCode(500, "Er is een interne fout opgetreden bij het toevoegen van team.");
            }
        }

        [HttpPut("teams/divisionupdate")]
        public async Task<IActionResult> UpdateTeamDivision([FromBody] TeamPromotionRelegation updateModel)
        {
            int teamId = updateModel.teamId;

            try
            {
                var existingTeam = await _teamService.GetTeamByIdAsync(teamId);
                if (existingTeam == null)
                {
                    return NotFound($"Team met ID {teamId} niet gevonden.");
                }

                if (updateModel.Promotion)
                {
                    if (existingTeam.division > Division.Eredivisie)
                    {
                        existingTeam.division -= 1;
                    }
                }
                else if (updateModel.Relegation)
                {
                    if (existingTeam.division < Division.VijfdeKlasse)
                    {
                        existingTeam.division += 1;
                    }
                }

                await _teamService.UpdateTeamAsync(existingTeam);

                return Ok(existingTeam);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fout bij het bijwerken van team: {ex.Message}");
                return StatusCode(500, "Er is een interne fout opgetreden bij het bijwerken van team.");
            }
        }

        [HttpPut("team/{id}")]
        public async Task<IActionResult> UpdateTeamAsync(int id, [FromBody] Team updatedTeam)
        {
            _logger.LogInformation($"UpdateTeamAsync() aangeroepen voor team met ID: {id}");

            if (updatedTeam == null)
            {
                return BadRequest();
            }

            updatedTeam.teamId = id;

            try
            {
                await _teamService.UpdateTeamAsync(updatedTeam);

                if (updatedTeam == null)
                {
                    return NotFound();
                }

                return Ok(updatedTeam);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fout bij het bijwerken van team: {ex.Message}");
                return NotFound($"Team met ID {id} niet gevonden.");
            }
        }

        [HttpDelete("team/{id}")]
        public async Task<IActionResult> DeleteTeamAsync(int id)
        {
            _logger.LogInformation($"DeleteTeamAsync() aangeroepen voor team met ID: {id}");

            try
            {
                var team = await _teamService.GetTeamByIdAsync(id);
                if (team == null)
                {
                    _logger.LogWarning($"Team met id {id} niet gevonden");
                    return NotFound("Team met dit ID is niet gevonden.");
                }

                await _teamService.DeleteTeamAsync(id);
                return Ok("Team met ID " + id + " verwijderd");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fout bij het verwijderen van het team: {ex.Message}");
                return StatusCode(500, "Er is een interne fout opgetreden bij het verwijderen van het team.");
            }
        }

    }
}
