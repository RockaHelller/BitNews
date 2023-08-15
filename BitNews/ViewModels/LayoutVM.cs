using BitNews.Areas.Admin.ViewModels.News;
using BitNews.Models;
using BitNews.ViewModels.Profile;

namespace BitNews.ViewModels
{
	public class LayoutVM
    {
        public string UserFullName { get; set; }
		public string UserEmail { get; set; }
		public string UserAddress { get; set; }
		public string UserPhone { get; set; }
        public string Image { get; set; }
        public Dictionary<string, string> SettingDatas { get; set; }
		public List<Category> Categories { get; set; }
		public List<News> News { get; set; }
		public ImageEditVM ImageEditVM { get; set; }
		public List<NewsVM> NewsVM { get; set; }
	}
}
