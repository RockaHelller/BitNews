namespace BitNews.Areas.Admin.ViewModels.News
{
    public class NewsVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Article { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string CategoryName { get; set; }
        public int ViewCount { get; set; }
        public string Tag { get; set; }
    }
}
