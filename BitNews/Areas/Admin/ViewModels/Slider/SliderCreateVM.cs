using System.ComponentModel.DataAnnotations;

namespace BitNews.Areas.Admin.ViewModels.Slider
{
	public class SliderCreateVM
	{
		[Required]
		public List<IFormFile> Images { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
	}
}
