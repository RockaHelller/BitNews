using BitNews.Areas.Admin.ViewModels.Category;
using BitNews.Data;
using BitNews.Helpers;
using BitNews.Models;
using BitNews.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BitNews.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ICategoryService _categoryService;
        private readonly IWebHostEnvironment _env;


        public CategoryController(AppDbContext context, IWebHostEnvironment env, ICategoryService categoryService)
        {
            _context = context;
            _categoryService = categoryService;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {

            List<CategoryVM> list = new();

            var datas = await _context.Categories.OrderByDescending(m => m.Id).ToListAsync();

            foreach (var item in datas)
            {
                list.Add(new CategoryVM
                {
                    Id = item.Id,
                    Name = item.Name,
                    Image = item.Image,
                });
            }

            return View(list);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            Category category = await _categoryService.GetByIdAsync(id);

            if (category is null) return NotFound();

            return View(category);
        }


        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryCreateVM request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            if (!request.Image.CheckFileType("image/"))
            {
                ModelState.AddModelError("Image", "Please select only image file");
                return View(request);
            }

            if (request.Image.CheckFileSize(2000))
            {
                ModelState.AddModelError("Image", "Image size must be max 200 KB");
                return View(request);
            }

            string imageName = null;

            if (request.Image != null && request.Image.Length > 0)
            {
                var image = request.Image;

                // Generate a unique image name or use a naming convention that suits your requirements
                imageName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);

                // Save the image to a specified location or a database, depending on your implementation
                var imagePath = Path.Combine("wwwroot/assets/img/Category", imageName);
                using (var fileStream = new FileStream(imagePath, FileMode.Create))
                {
                    await image.CopyToAsync(fileStream);
                }
            }

            Category newCategory = new()
            {
                Name = request.Name,
                Image = imageName
            };

            await _context.Categories.AddAsync(newCategory);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {

            if (id is null) return BadRequest();
            var existCategory = await _context.Categories.FirstOrDefaultAsync(m => m.Id == id);
            if (existCategory is null) return NotFound();

            CategoryEditVM model = new()
            {
                Id = existCategory.Id,
                Name = existCategory.Name,
                Image = existCategory.Image,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, CategoryEditVM request)
        {
            if (id is null)
                return BadRequest();

            var existCategory = await _context.Categories.FirstOrDefaultAsync(m => m.Id == id);
            if (existCategory is null)
                return NotFound();

            if (existCategory.Name.Trim() != request.Name.Trim())
            {
                existCategory.Name = request.Name;
            }

            // Get the old image file path from the existing category
            var oldImagePath = Path.Combine("wwwroot/assets/img/Category", existCategory.Image);

            if (request.NewImage != null)
            {
                if (!request.NewImage.CheckFileType("image/"))
                {
                    ModelState.AddModelError("NewImage", "Please select only an image file");
                    return View();
                }

                if (request.NewImage.CheckFileSize(2000))
                {
                    ModelState.AddModelError("NewImage", "Image size must be a maximum of 200 KB");
                    return View();
                }

                var imageName = Guid.NewGuid().ToString() + Path.GetExtension(request.NewImage.FileName);

                var imagePath = Path.Combine("wwwroot/assets/img/Category", imageName);
                using (var fileStream = new FileStream(imagePath, FileMode.Create))
                {
                    await request.NewImage.CopyToAsync(fileStream);
                }

                // If a new image is provided, delete the old image from the folder
                //if (System.IO.File.Exists(oldImagePath))
                //{
                //    System.IO.File.Delete(oldImagePath);
                //}

                existCategory.Image = imageName;
            }

            _context.Update(existCategory);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }




        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    var existCategory = await _context.Categories.FirstOrDefaultAsync(m => m.Id == id);


        //    if (!string.IsNullOrEmpty(existCategory.Image))
        //    {
        //        string imagePath = Path.Combine(_env.WebRootPath, "assets", "img", "Category", existCategory.Image);

        //        if (System.IO.File.Exists(imagePath))
        //        {
        //            System.IO.File.Delete(imagePath);
        //        }
        //    }

        //    _context.Remove(existCategory);
        //    await _context.SaveChangesAsync();

        //    return RedirectToAction(nameof(Index));
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            var existCategory = await _context.Categories.FirstOrDefaultAsync(m => m.Id == id);

            existCategory.SoftDelete = true;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
