using BitNews.Areas.Admin.ViewModels.Category;
using BitNews.Data;
using BitNews.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BitNews.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize]
    public class ArchiveController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ArchiveController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Categories()
        {
            List<CategoryVM> list = new();

            var datas = await _context.Categories.IgnoreQueryFilters().Where(m => m.SoftDelete).ToListAsync();

            foreach (var item in datas)
            {
                list.Add(new CategoryVM
                {
                    Id = item.Id,
                    Name = item.Name,
                    Image = item.Image
                });
            }

            return View(list);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExtractCategory(int id)
        {
            var existCategory = await _context.Categories.IgnoreQueryFilters().Where(m => m.SoftDelete).FirstOrDefaultAsync(m => m.Id == id);

            existCategory.SoftDelete = false;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Categories));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var existCategory = await _context.Categories.IgnoreQueryFilters().FirstOrDefaultAsync(m => m.Id == id);

            if (existCategory != null)
            {
                if (!string.IsNullOrEmpty(existCategory.Image))
                {
                    string imagePath = Path.Combine(_env.WebRootPath, "assets", "img", "Category", existCategory.Image);
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }

                _context.Categories.Remove(existCategory);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Categories));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAll()
        {
            var allCategories = await _context.Categories.IgnoreQueryFilters().ToListAsync();

            foreach (var category in allCategories)
            {
                if (!string.IsNullOrEmpty(category.Image))
                {
                    string imagePath = Path.Combine(_env.WebRootPath, "assets", "img", "Category", category.Image);

                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }

                _context.Categories.Remove(category);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Categories));
        }
    }
}
