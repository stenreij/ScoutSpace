using Microsoft.AspNetCore.Mvc;
using ScoutSpace.Models;
using System.Diagnostics;

namespace ScoutSpace.Controllers
{
    public class ScoutController : Controller
    {
        private readonly ILogger<ScoutController> _logger;

        public ScoutController(ILogger<ScoutController> logger)
        {
            _logger = logger;
        }
        public IActionResult ScoutList()
        {
            return View();
        }

        public IActionResult Profile()
        {
            return View();
        }
    }
}