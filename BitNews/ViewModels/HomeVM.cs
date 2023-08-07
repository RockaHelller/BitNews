using BitNews.Areas.Admin.ViewModels.News;
using BitNews.Models;

namespace BitNews.ViewModels
{
	public class HomeVM
	{
		public List<Slider> Sliders { get; set; }
		public List<News> News { get; set; }
		public List<Category> Categories { get; set; }
        public LayoutVM View { get; set; }


    }
}
