using BitNews.Areas.Admin.ViewModels.News;
using BitNews.Data;
using BitNews.Models;
using BitNews.Services.Interfaces;
using BitNews.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BitNews.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ISliderService _sliderService;
        private readonly INewsService _newsService;
        private readonly ICategoryService _categoryService;

        public HomeController(AppDbContext context, ISliderService sliderService,
            INewsService newsService, ICategoryService categoryService)
        {
            _context = context;
            _sliderService = sliderService;
            _newsService = newsService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Slider> sliders = await _sliderService.GetAllByStatusAsync();
            IEnumerable<News> news = await _newsService.GetAllWithIncludesAsync();
            IEnumerable<Category> categories = await _categoryService.GetAll();
            var settings = await _context.Settings.ToListAsync();

            news = news.OrderByDescending(n => n.ViewCount);

            HomeVM model = new HomeVM
            {
                Sliders = sliders.ToList(),
                News = news.ToList(),
                Categories = categories.ToList(),
                SettingDatas = settings.AsEnumerable().ToDictionary(m => m.Key, m => m.Value),
            };

            return View(model);
        }
    }
}