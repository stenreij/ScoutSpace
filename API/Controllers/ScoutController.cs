using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Core.Domain;
using Core.DomainServices;

namespace API.Controllers
{
    [ApiController]
    [Route("api/")]
    public class ScoutController : Controller
    {
        private readonly ILogger<ScoutController> _logger;
        private readonly IScoutService _scoutService;

        public ScoutController(ILogger<ScoutController> logger, IScoutService scoutService)
        {
            _logger = logger;
            _scoutService = scoutService;
        }

        [HttpGet("scouts")]
        public async Task<IActionResult> GetAllScouts()
        {
            _logger.LogInformation("GetAllScouts() aangeroepen");

            try
            {
                var scouts = await _scoutService.GetAllScoutsAsync();
                return Ok(scouts);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fout bij het ophalen van scouts: {ex.Message}");
                return StatusCode(500, "Er is een interne fout opgetreden bij het ophalen van scouts.");
            }
        }
    }
}
