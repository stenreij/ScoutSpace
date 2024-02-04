using Microsoft.AspNetCore.Mvc;
using ScoutSpace.Models;
using System.Diagnostics;

namespace ScoutSpace.Controllers
{
    public class PlayerController : Controller
    {
        private readonly ILogger<PlayerController> _logger;

        public PlayerController(ILogger<PlayerController> logger)
        {
            _logger = logger;
        }
        public IActionResult PlayerList()
        {
            return View();
        }
    }
}