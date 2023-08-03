using BitNews.Areas.Admin.ViewModels.News;
using BitNews.Data;
using BitNews.Models;
using BitNews.Services.Interfaces;
using BitNews.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BitNews.Controllers
{
    public class NewsController : Controller
    {
        private readonly INewsService _newsService;
        private readonly ISettingService _settingService;
        private readonly ICategoryService _categoryService;
        private readonly AppDbContext _context;



        public NewsController(INewsService productService,
                                     ISettingService settingService,
                                     ICategoryService categoryService,
                                     AppDbContext context)
        {
            _newsService = productService;
            _settingService = settingService;
            _categoryService = categoryService;
            _context = context;

        }
        public async Task<IActionResult> Index(int id)
        {
            IEnumerable<News> news = await _newsService.GetAllWithIncludesAsync();

            List<NewsVM> model = new List<NewsVM>();
            foreach (var item in news)
            {
                model.Add(new NewsVM
                {
                    Id = item.Id,
                    Title = item.Title,
                    Image = item.Images?.FirstOrDefault()?.Image,
                });
            }

            return View(model);
        }

    }
}
