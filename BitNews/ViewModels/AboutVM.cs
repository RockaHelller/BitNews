namespace BitNews.ViewModels
{
    public class AboutVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; } // Correct data type for file upload
        //public IFormFile Image { get; set; }
        public string Mission { get; set; }
        public string WhatWeDo { get; set; }
    }
}
