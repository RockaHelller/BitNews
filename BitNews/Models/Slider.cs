namespace BitNews.Models
{
    public class Slider : BaseEntity
    {
        public string Image { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; } = true;
    }
}
