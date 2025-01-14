using GameZone.Services;

namespace GameZone.Controllers
{
    public class GamesController:Controller
    {
        private readonly ICategoriesServcie categoriesServcie;
        private readonly IDevicesService devicesService;
        private readonly IGamesService gamesService;

        public GamesController( ICategoriesServcie _categoriesServcie, 
                                IDevicesService _devicesService,
                                IGamesService _gamesService)
        {
            categoriesServcie = _categoriesServcie;
            devicesService = _devicesService;
            gamesService = _gamesService;
        }
        public IActionResult Index()
        {
            var games = gamesService.GetAll();
            return View(games);
        }
        public IActionResult Details(int id)
        {
            var game = gamesService.GetGame(id);
            if(game is null)
                return NotFound();
            return View(game);
        }

        [HttpGet]
        public IActionResult Create()
        {
            CreateGameFormViewModel viewmodel = new CreateGameFormViewModel()
            {
                Categroies = categoriesServcie.GetSelectList(),
                Devices    = devicesService.GetSelectList() 
            };
            return View(viewmodel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateGameFormViewModel model)
        {
            if(!ModelState.IsValid)
            {
                model.Categroies = categoriesServcie.GetSelectList();
                model.Devices = devicesService.GetSelectList();
                return View(model);
            }
            await gamesService.Create(model);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var game = gamesService.GetGame(id);
            if (game is null)
                return NotFound();
            EditGameFormViewModel viewModel = new()
            {
                ID = id,
                Name = game.Name,
                Description = game.Description,
                Category_ID = game.Category_ID,
                SelectedDevices = game.Devices.Select(d => d.DeviceID).ToList(),
                Categroies = categoriesServcie.GetSelectList(),
                Devices = devicesService.GetSelectList(),
                CurrentCover = game.Cover
            };
            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditGameFormViewModel model)
        {
            if(!ModelState.IsValid)
            {
                model.Categroies = categoriesServcie.GetSelectList();
                model.Devices = devicesService.GetSelectList();
                return View(model);
            }
            var game = await gamesService.Update(model);
            if (game is null)
                return BadRequest();
            return RedirectToAction(nameof(Index));
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var isDeleted = gamesService.Delete(id);
            return isDeleted ? Ok() : BadRequest();
        }
    }
}
