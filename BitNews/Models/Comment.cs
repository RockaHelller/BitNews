namespace BitNews.Models
{
    public class Comment : BaseEntity
    {
        public string Name { get; set; }
        public string Text { get; set; }
        public string CreatorName { get; set; }
        public int NewsId { get; set; }
        public News News { get; set; }


    }
}
