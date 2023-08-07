namespace BitNews.ViewModels.Profile
{
    public class ImageEditVM
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public List<IFormFile> NewImage { get; set; }
    }
}
