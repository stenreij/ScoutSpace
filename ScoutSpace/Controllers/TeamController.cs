using Microsoft.AspNetCore.Mvc;
using ScoutSpace.Models;
using System.Diagnostics;

namespace ScoutSpace.Controllers
{
    public class TeamController : Controller
    {
        private readonly ILogger<TeamController> _logger;

        public TeamController(ILogger<TeamController> logger)
        {
            _logger = logger;
        }
        public IActionResult TeamList()
        {
            return View();
        }
    }
}