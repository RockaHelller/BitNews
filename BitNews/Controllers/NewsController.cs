using BitNews.Areas.Admin.ViewModels.News;
using BitNews.ViewModels;
using BitNews.Data;
using BitNews.Models;
using BitNews.Services.Interfaces;
using BitNews.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BitNews.Services;
using System.Data;

namespace BitNews.Controllers
{
    public class NewsController : Controller
    {
        private readonly INewsService _newsService;
        private readonly ISettingService _settingService;
        private readonly ICategoryService _categoryService;
        private readonly AppDbContext _context;
        private readonly ILayoutService _layoutService;



        public NewsController(INewsService newsService,
                                     ISettingService settingService,
                                     ICategoryService categoryService,
                                     AppDbContext context,
                                     ILayoutService layoutService)
        {
            _newsService = newsService;
            _settingService = settingService;
            _categoryService = categoryService;
            _context = context;
            _layoutService = layoutService;
        }

        public async Task<IActionResult> Index(string[] selectedTags, string sort, string searchText = null, int page = 1, int pageSize = 10)
        {
            ViewBag.PageSize = pageSize;

            IEnumerable<News> news;

            if (!string.IsNullOrEmpty(searchText))
            {
                news = await _newsService.GetSearchedNews(searchText);
            }
            else
            {
                news = await _newsService.GetAllWithIncludesAsync();
            }

            int totalItems = news.Count();
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var pagedNews = news
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            IEnumerable<Category> categories = await _categoryService.GetAll();
            List<NewsVM> model = new List<NewsVM>();
            var datas = await _layoutService.GetAllDatas();

            foreach (var item in pagedNews)
            {
                model.Add(new NewsVM
                {
                    Id = item.Id,
                    Title = item.Title,
                    Description = item.Description,
                    Image = item.Images?.FirstOrDefault()?.Image,
                    CategoryName = item.Category.Name,
                    ViewCount = item.ViewCount,
                    View = datas,
                    CreateDate = item.CreateDate.ToString("dd MMMM yyyy"),
                    Tag = string.Join(", ", item.NewsTags.Select(tag => tag.Tag.Name)),
                    CreatorName = item.CreatorName,
                });
            }

            if (selectedTags != null && selectedTags.Length > 0)
            {
                model = model.Where(n => selectedTags.Contains(n.CategoryName)).ToList();
            }

            var paginatedModel = new Helpers.Paginate<NewsVM>(model, page, totalPages);

            return View(paginatedModel);
        }


        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null) return BadRequest();

            News news = await _context.News
                .Include(m => m.Images)
                .Include(m => m.Category)
                .Include(m => m.Comments)
                .Include(m => m.NewsTags)
                    .ThenInclude(nt => nt.Tag) // Include the related Tag entity
                .Where(m => !m.SoftDelete)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (news is null) return NotFound();

            var datas = await _layoutService.GetAllDatas();

            ViewModels.NewsDetailVM model = new()
            {
                Id = news.Id,
                Title = news.Title,
                Description = news.Description,
                Article = news.Article,
                CategoryName = news.Category.Name,
                Image = news.Images,
                NewsTags = news.NewsTags, // Set the NewsTags collection in the NewsDetailVM model
                CreateDate = news.CreateDate.ToString("dddd, dd MMMM yyyy"),
                View = datas,
                News = datas.News,
                CreatorName = news.CreatorName,
                Comments = news.Comments,
            };

            return View(model);
        }

        //int newsId, string text,
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostComment(int id, ViewModels.NewsDetailVM model)
        {
            
                var comment = new Comment
                {
                    Name = model.Name,
                    Text = model.Text,
                    CreatorName = model.CreatorName,
                    NewsId = model.Id,
                    
                };

                _context.Comments.Add(comment);
                await _context.SaveChangesAsync();

                return RedirectToAction("Detail", new { id = model.Id });

            // If ModelState is not valid, return the view with the model
            //return View("Detail", model); // You might need to specify the full view name if needed
        }






    }
}
