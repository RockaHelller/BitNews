using System.ComponentModel.DataAnnotations;

namespace BitNews.Areas.Admin.ViewModels.Category
{
    public class EditImageVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public IFormFile NewImage { get; set; }
    }
}
