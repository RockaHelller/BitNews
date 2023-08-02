using BitNews.Areas.Admin.ViewModels.Category;
using BitNews.Data;
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

        public ArchiveController(AppDbContext context)
        {
            _context = context;
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
    }
}
