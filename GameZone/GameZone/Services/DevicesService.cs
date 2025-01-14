
namespace GameZone.Services
{
    public class DevicesService:IDevicesService
    {
        private readonly ApplicationDBContext context;

        public DevicesService(ApplicationDBContext _context)
        {
            context = _context;
        }
        public IEnumerable<SelectListItem> GetSelectList()
        {
            return  context.Devices
                    .Select(d => new SelectListItem { Value = d.ID.ToString(),Text = d.Name })
                    .OrderBy(d => d.Text)
                    .AsNoTracking()
                    .ToList();
        }
    }
}
