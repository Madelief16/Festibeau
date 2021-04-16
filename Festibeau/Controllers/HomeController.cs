using Festibeau.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Festibeau.Controllers
{
  public class HomeController : Controller
    {
         private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Route("contact")]
        public IActionResult contact()
        {
            return View();
        }

        [Route("Festivals")]
        public IActionResult Festivals()
        {
            return View();
        }

        [Route("regels")]
        public IActionResult regels()
        {
            return View();
        }

        [Route("prijzen")]
        public IActionResult prijzen()
        {
            return View();
        }

        [Route("Locaties")]
        public IActionResult Locaties()
        {
            return View();
        }

        [Route("Lowlands")]
        public IActionResult Lowlands()
        {
            return View();
        }

        [Route("Hockeyloverz")]
        public IActionResult Hockeyloverz()
        {
            return View();
        }

        [Route("MysteryLand")]
        public IActionResult MysteryLand()
        {
            return View();
        }

        [Route("Pinkpop")]
        public IActionResult Pinkpop()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
