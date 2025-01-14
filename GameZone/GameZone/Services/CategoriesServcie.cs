
namespace GameZone.Services
{
    public class CategoriesServcie:ICategoriesServcie
    {
        private readonly ApplicationDBContext context;

        public CategoriesServcie(ApplicationDBContext _context)
        {
            context = _context;
        }
        public IEnumerable<SelectListItem> GetSelectList()
        {
            return  context.Gategories
                    .Select(c => new SelectListItem { Value = c.ID.ToString(),Text = c.Name })
                    .OrderBy(c => c.Text)
                    .AsNoTracking()
                    .ToList();
        }
    }
}
