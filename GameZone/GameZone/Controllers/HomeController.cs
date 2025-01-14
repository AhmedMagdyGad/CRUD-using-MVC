using System.Diagnostics;

namespace GameZone.Controllers
{
    public class HomeController:Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IGamesService gamesService;

        public HomeController(ILogger<HomeController> logger, IGamesService _gamesService)
        {
            _logger = logger;
            gamesService = _gamesService;
        }

        public IActionResult Index()
        {
            var games = gamesService.GetAll();
            return View(games);
        }


        [ResponseCache(Duration = 0,Location = ResponseCacheLocation.None,NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
