using BitNews.Data;
using BitNews.Services.Interfaces;
using BitNews.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BitNews.Controllers
{
    public class AboutController : Controller
    {
        private readonly AppDbContext _context;

        public AboutController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var about = _context.Abouts.FirstOrDefault();
            
            AboutVM model = new()
            {
                Title = about.Title,
                Description = about.Description,
                Mission = about.Mission,
                WhatWeDo = about.WhatWeDo,
                Image = about.Image,
            };

            return View(model);
        }
    }
}