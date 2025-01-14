namespace GameZone.Services
{
    public interface ICategoriesServcie
    {
        IEnumerable<SelectListItem> GetSelectList();
    }
}
