using BitNews.Models;

namespace BitNews.ViewModels
{
    public class NewsDetailVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Article { get; set; }
        public string Description { get; set; }
        public string CategoryName { get; set; }
        public ICollection<NewsImage> Image { get; set; }
        public ICollection<NewsTag> NewsTags { get; set; }
        public ICollection<News> News { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public LayoutVM View { get; set; }
        public int ViewCount { get; set; }
        public string CreateDate { get; set; }
        public string CreatorName { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }

    }
}
