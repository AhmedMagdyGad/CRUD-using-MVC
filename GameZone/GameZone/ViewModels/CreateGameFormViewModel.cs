using GameZone.Attributes;

namespace GameZone.ViewModels
{
    public class CreateGameFormViewModel:GameFormViewModel
    {
        [AllowedExtensions(FileSettings.AllowedExtinsions),
            MaxFileSize(FileSettings.MaxFileSizeinBytes)]
        public IFormFile Cover { get; set; } = default!;
    }
}
