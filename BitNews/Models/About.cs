﻿namespace BitNews.Models
{
    public class About : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Mission { get; set; }
        public string WhatWeDo { get; set; }
    }
}
