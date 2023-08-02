using System.ComponentModel.DataAnnotations;

namespace BitNews.Areas.Admin.Views.News
{
    public class NewsCreateVM
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public IFormFile Image { get; set; }
		public string Article { get; set; }
		public int CategoryId { get; set; }
		public List<TagCheckBox> Tags { get; set; }






	}
}
