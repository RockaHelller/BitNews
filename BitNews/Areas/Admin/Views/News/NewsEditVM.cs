namespace BitNews.Areas.Admin.Views.News
{
    public class NewsEditVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int ViewCount { get; set; }
        public string Article { get; set; }
        public int CategoryId { get; set; }
        public List<IFormFile>? Image { get; set; }
        public List<TagCheckBox> Tags { get; set; }
    }
}
