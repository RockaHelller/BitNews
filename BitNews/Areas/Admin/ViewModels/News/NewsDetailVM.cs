using BitNews.Models;

namespace BitNews.Areas.Admin.ViewModels.News
{
    public class NewsDetailVM
    {
        public string Title { get; set; }
		public string Article { get; set; }
        public string Description { get; set; }
        public ICollection<NewsImage> Image { get; set; }
		public ICollection<NewsTag> NewsTags { get; set; }
		public string Category { get; set; }
		public int ViewCount { get; set; }
		public string CreateDate { get; set; }
		
	}
}
