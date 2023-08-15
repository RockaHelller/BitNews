namespace BitNews.Areas.Admin.ViewModels.News
{
    public class NewsEditVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int ViewCount { get; set; }
        public string Article { get; set; }
        public int CategoryId { get; set; }
        public IFormFile? Image { get; set; }
        public List<TagCheckBox> Tags { get; set; }

    }
}
