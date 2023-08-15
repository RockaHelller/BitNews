namespace BitNews.Areas.Admin.ViewModels.About
{
    public class AboutEditVM
    {

        public int Id { get; set; }
        public string Title { get; set; }

        public string Description { get; set; }
        public string Mission { get; set; }
        public string WhatWeDo { get; set; }
        public string Image { get; set; }
        public IFormFile NewImage { get; set; }



    }
}
