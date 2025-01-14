namespace GameZone.Services;

public interface IGamesService
{
    IEnumerable<Game> GetAll();
    Game? GetGame(int id);
    Task Create(CreateGameFormViewModel game);
    Task<Game?> Update(EditGameFormViewModel model);
    bool Delete(int id);
}
