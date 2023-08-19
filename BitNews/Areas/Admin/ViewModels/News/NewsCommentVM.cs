namespace BitNews.Areas.Admin.ViewModels.News
{
    public class NewsCommentVM
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Name { get; set; }
        public string CreatorName { get; set; }
        public string Email { get; set; }
        public string Text { get; set; }
        public string CreateDate { get; set; }
    }
}
