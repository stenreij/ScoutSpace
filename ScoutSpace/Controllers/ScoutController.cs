using Core.Domain;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ScoutSpace.Models;
using System.Diagnostics;

namespace ScoutSpace.Controllers
{
    public class ScoutController : Controller
    {
        private readonly ILogger<ScoutController> _logger;
        Uri baseAddress = new Uri("https://localhost:7296/api");
        HttpClient client;

        public ScoutController(ILogger<ScoutController> logger)
        {
            _logger = logger;
            client = new HttpClient();
            client.BaseAddress = baseAddress;
        }
        public IActionResult ScoutList()
        {
            List<Scout> scoutList = new List<Scout>();
            HttpResponseMessage response = client.GetAsync(client.BaseAddress + "/scouts").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                scoutList = JsonConvert.DeserializeObject<List<Scout>>(data);
            }
            return View(scoutList);
        }
    
        public IActionResult Profile()
        {
            return View();
        }
    }
}