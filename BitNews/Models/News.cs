namespace BitNews.Models
{
    public class News : BaseEntity
    {
        public string Title { get; set; }
        public string Article { get; set; }
        public Category Category { get; set; }
        public int CategoryId { get; set; }
        public int ViewCount { get; set; }
        public ICollection<NewsImage> Images { get; set; }
        public ICollection<NewsTag> NewsTags { get; set; }


    }
}
