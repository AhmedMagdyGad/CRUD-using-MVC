
using GameZone.Models;

namespace GameZone.Services
{
    public class GamesService:IGamesService
    {
        private readonly ApplicationDBContext context;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly string _imagePath;

        public GamesService(ApplicationDBContext _context, IWebHostEnvironment _webHostEnvironment)
        {
            context = _context;
            webHostEnvironment = _webHostEnvironment;
            _imagePath = $"{_webHostEnvironment.WebRootPath}{FileSettings.ImagesPath}";
        }
        public IEnumerable<Game> GetAll()
        {
            return   context.Games
                            .Include(G => G.Category)
                            .Include(G => G.Devices)
                            .ThenInclude(D => D.Device)
                            .AsNoTracking()
                            .ToList();

        }
        public Game? GetGame(int id)
        {
            return   context.Games
                            .Include(G => G.Category)
                            .Include(G => G.Devices)
                            .ThenInclude(D => D.Device)
                            .AsNoTracking()
                            .SingleOrDefault(G => G.ID == id);
        }

        public async Task Create(CreateGameFormViewModel game)
        {
            var coverName = await SaveCover(game.Cover);

            Game NewGame = new()
            {
                Name = game.Name,
                Description = game.Description,
                Category_ID = game.Category_ID,
                Cover = coverName,
                Devices = game.SelectedDevices.Select(D => new GameDevice { DeviceID = D}).ToList()
            };

            context.Games.Add(NewGame);
            context.SaveChanges();
        }

        public async Task<Game?> Update(EditGameFormViewModel model)
        {
            var game = context.Games
                .Include(G => G.Devices)
                .SingleOrDefault(G => G.ID == model.ID);
            if (game == null)
            {
                return null;
            }

            var hasNewCover = model.Cover is not null;
            var oldCover = game.Cover;

            game.Name = model.Name;
            game.Description = model.Description;
            game.Category_ID = model.Category_ID;
            game.Devices = model.SelectedDevices.Select(d => new GameDevice { DeviceID = d}).ToList();

            if(hasNewCover)
            {
                game.Cover = await SaveCover(model.Cover!);
            }
            var effectedrows = context.SaveChanges();
            if(effectedrows > 0)
            {
                if(hasNewCover)
                {
                    var cover = Path.Combine(_imagePath,oldCover);
                    File.Delete(cover);
                }
                return game;
            }else
            {
                var cover = Path.Combine(_imagePath,game.Cover);
                File.Delete(cover);
                return null;
            }
        }

        public bool Delete(int id)
        {
            var isDeleted = false;
            Game? game = context.Games.Find(id);
            if (game is null)
            {
                return isDeleted;
            }
            context.Remove(game);
            var effectedrows = context.SaveChanges();
            if(effectedrows > 0)
            {
                isDeleted = true;
                var cover = Path.Combine(_imagePath,game.Cover);
                File.Delete(cover);
            }
            return isDeleted;
        }

        private async Task<string> SaveCover(IFormFile cover)
        {
            var coverName = $"{Guid.NewGuid()}{Path.GetExtension(cover.FileName)}";
            var path = Path.Combine(_imagePath,coverName);

            using var stream = File.Create(path);
            await cover.CopyToAsync(stream);
            return coverName;
        }

    }
}
