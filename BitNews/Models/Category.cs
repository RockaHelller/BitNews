namespace BitNews.Models
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public ICollection<News> News { get; set; }
    }
}
