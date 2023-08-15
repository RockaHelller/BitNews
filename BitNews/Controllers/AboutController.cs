using BitNews.Data;
using BitNews.Services.Interfaces;
using BitNews.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BitNews.Controllers
{
    public class AboutController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILayoutService _layoutService;

        public AboutController(AppDbContext context, ILayoutService layoutService)
        {
            _context = context;
            _layoutService = layoutService;
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
