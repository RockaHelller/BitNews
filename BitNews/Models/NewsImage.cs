namespace BitNews.Models
{
    public class NewsImage : BaseEntity
    {
        public string Image { get; set; }
        public bool IsMain { get; set; }
        public int NewsId { get; set; }
        public News News { get; set; }
    }
}
