using BitNews.Areas.Admin.ViewModels.News;
using BitNews.Models;

namespace BitNews.ViewModels
{
	public class LayoutVM
	{
		public string UserFullName { get; set; }
		public string UserEmail { get; set; }
		public Dictionary<string, string> SettingDatas { get; set; }
		public List<Category> Categories { get; set; }
		public List<News> News { get; set; }
		public List<NewsVM> NewsVM { get; set; }
	}
}
