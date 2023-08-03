using System.ComponentModel.DataAnnotations;

namespace BitNews.Areas.Admin.ViewModels.Tag
{
    public class TagCreateVM
    {
        [Required]
        public string Name { get; set; }
    }
}
