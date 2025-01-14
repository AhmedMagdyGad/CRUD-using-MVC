using GameZone.Attributes;

namespace GameZone.ViewModels
{
    public class EditGameFormViewModel:GameFormViewModel
    {
        public int ID { get; set; }
        public string? CurrentCover { get; set; }
        [AllowedExtensions(FileSettings.AllowedExtinsions),
            MaxFileSize(FileSettings.MaxFileSizeinBytes)]
        public IFormFile? Cover { get; set; } = default!;
    }
}
