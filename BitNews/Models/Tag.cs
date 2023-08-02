namespace BitNews.Models
{
    public class Tag : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<NewsTag> NewsTag { get; set; }
    }
}
