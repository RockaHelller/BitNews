using BitNews.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace BitNews.Areas.Admin.ViewModels.News
{
    public class NewsCreateVM
    {
        [Required]
        public string Title { get; set; }
        [Required]
		public IFormFile Image { get; set; }
		public string Article { get; set; }
		public string Description { get; set; }
		public int CategoryId { get; set; }
		public List<TagCheckBox> Tags { get; set; }
        public string CreatorName { get; set; } // Add this property
        //public LayoutVM View { get; set; }





    }
}
