
using BitNews.Areas.Admin.ViewModels.News;
using BitNews.Data;
using BitNews.Helpers;
using BitNews.Models;
using BitNews.Services.Interfaces;
using BitNews.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BitNews.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class NewsController : Controller
    {
        private readonly INewsService _newsService;
        private readonly ISettingService _settingService;
        private readonly ICategoryService _categoryService;
        private readonly ITagService _tagsService;
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;



        public NewsController(INewsService newsService,
                                     ISettingService settingService,
                                     ICategoryService categoryService,
                                     AppDbContext context,
                                     IWebHostEnvironment env,
                                     ITagService tagsService)
        {
            _newsService = newsService;
            _settingService = settingService;
            _categoryService = categoryService;
            _context = context;
            _env = env;
            _tagsService = tagsService;
        }

        private async Task GetAllSelectOptions()
        {
            ViewBag.categories = await GetCategories();
        }
        private async Task<SelectList> GetCategories()
        {
            IEnumerable<Category> categories = await _categoryService.GetAll();
            return new SelectList(categories, "Id", "Name");
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 10;

            var news = await _newsService.GetAllWithIncludesAsync();

            int totalItems = news.Count();
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var pagedNews = news
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            List<NewsVM> model = new List<NewsVM>();
            foreach (var item in pagedNews)
            {
                model.Add(new NewsVM
                {
                    Id = item.Id,
                    Image = item.Images?.FirstOrDefault()?.Image,
                    Title = item.Title,
                    Description = item.Description,
                    Article = item.Article,
                    CategoryName = item.Category?.Name,
                    CreatorName = item.CreatorName
                });
            }

            var paginatedModel = new Paginate<NewsVM>(model, page, totalPages);

            // Pass the categories data to the view directly using the model
            var categories = await _categoryService.GetAll();
            var tags = await _tagsService.GetAll();

            ViewBag.PaginatedModel = paginatedModel;

            // Convert the list of Category objects to a list of SelectListItem
            var categorySelectListItems = categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();

            ViewBag.Categories = categorySelectListItems;

            var tagSelectListItems = tags.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();

            ViewBag.Tag = tagSelectListItems;

            return View(paginatedModel);
        }




        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Create()
        {
            var categories = await _categoryService.GetAll();

            var tags = await _context.Tags.ToListAsync();
            List<TagCheckBox> tagCheckBoxes = new();

            foreach (var item in tags)
            {
                tagCheckBoxes.Add(new TagCheckBox { Id = item.Id, Value = item.Name, IsChecked = false });
            }

            ViewBag.categories = categories.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }).ToList();


            var model = new NewsCreateVM
            {
                Tags = tagCheckBoxes,
                CreatorName = User.Identity.Name,

            };

            return View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NewsCreateVM model)
        {
            await GetAllSelectOptions();
            string creatorName = $"{User.Identity.Name}"; // Modify this based on your authentication setup
            model.CreatorName = creatorName;

            if (!ModelState.IsValid)
            {
                var categories = await _categoryService.GetAll();
                ViewBag.categories = categories.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }).ToList();
                return View(model);
            }

            if (model.Image == null)
            {
                ModelState.AddModelError("Image", "Please select an image file");
                return View();
            }

            if (!model.Image.CheckFileType("image/"))
            {
                ModelState.AddModelError("Image", "Please select only an image file");
                return View();
            }

            if (model.Image.CheckFileSize(20000))
            {
                ModelState.AddModelError("Image", "Image size must be max 2000 KB");
                return View();
            }

            await _newsService.CreateAsync(model);
            return RedirectToAction("Index");
        }



        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null)
                return BadRequest();

            var existNews = await _newsService.GetByIdAsync(id);
            if (existNews is null)
                return NotFound();

            var tags = await _context.Tags.ToListAsync();
            List<TagCheckBox> tagCheckBoxes = new();

            foreach (var item in tags)
            {
                tagCheckBoxes.Add(new TagCheckBox { Id = item.Id, Value = item.Name, IsChecked = IsCheckedTag(id, item) });
            }

            var model = new NewsEditVM
            {
                Id = existNews.Id,
                Title = existNews.Title,
                Article = existNews.Article,
                Description = existNews.Description,
                CategoryId = existNews.CategoryId,
                Tags = tagCheckBoxes,

            };

            var categories = await _context.Categories.ToListAsync();
            ViewBag.categories = categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, NewsEditVM model)
        {
            if (id == null)
                return BadRequest();

            if (!ModelState.IsValid)
            {
                var categories = await _context.Categories.ToListAsync();
                ViewBag.categories = categories.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList();

                return View(model);
            }

            await _newsService.EditAsync(model);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Comments(int id)
        {
            var news = await _newsService.GetByIdAsync(id);

            if (news == null)
            {
                return NotFound();
            }

            var comments = await _context.Comments
                .Where(comment => comment.NewsId == id)
                .ToListAsync();

            var newsComments = comments.Select(comment => new NewsCommentVM
            {
                Id = comment.Id,
                Name = comment.Name,
                Text = comment.Text,
                CreateDate = comment.CreateDate.ToString("dddd, dd MMMM yyyy"),
                CreatorName = comment.CreatorName
            }).ToList();

            ViewBag.NewsTitle = news.Title;
            ViewBag.NewsId = id;

            return View(newsComments);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteComment(int id, int newsId)
        {

            var news = await _context.News.FirstOrDefaultAsync(m => m.Id == newsId);

            var comment = await _context.Comments.FirstOrDefaultAsync(m => m.Id == id);

            if (comment == null)
            {
                return NotFound();
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            // Redirect back to the comments page for the specific news
            return RedirectToAction("Comments", new { id = newsId });
        }




        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null)
                return BadRequest();

            var news = await _newsService.GetByIdWithAllIncludes(id.Value);

            if (news is null)
                return NotFound();

            var newsDetail = _newsService.GetMappedData(news);

            return View(newsDetail);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            News news = await _context.News
                .Include(n => n.Images)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (news == null)
            {
                return NotFound();
            }

            // Delete the associated image file if it exists
            //if (!string.IsNullOrEmpty(news.Images?.FirstOrDefault()?.Image))
            //{
            //    string imagePath = Path.Combine(_env.WebRootPath, "assets/img/News", news.Images.FirstOrDefault().Image);
            //    if (System.IO.File.Exists(imagePath))
            //    {
            //        System.IO.File.Delete(imagePath);
            //    }
            //}

            _context.News.Remove(news);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }





        private bool IsImageFile(IFormFile file)
        {
            return file.ContentType.StartsWith("image/");
        }


        private bool IsCheckedTag(int? id, Tag item)
        {
            if (item.NewsTag != null)
            {
                foreach (var tag in item.NewsTag)
                {
                    if (tag.NewsId == id)
                    {
                        return true;

                    }
                    else
                    {
                        return false;
                    }

                }
            }

            return false;

        }


    }
}