using BitNews.Areas.Admin.ViewModels.Category;
using BitNews.Areas.Admin.ViewModels.Tag;
using BitNews.Data;
using BitNews.Models;
using BitNews.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BitNews.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize]
    public class TagController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ITagService _tagService;
        private readonly IWebHostEnvironment _env;


        public TagController(AppDbContext context, IWebHostEnvironment env, ITagService tagService)
        {
            _context = context;
            _tagService = tagService;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {

            List<TagVM> list = new();

            var datas = await _context.Tags.OrderByDescending(m => m.Id).ToListAsync();

            foreach (var item in datas)
            {
                list.Add(new TagVM
                {
                    Id = item.Id,
                    Name = item.Name,
                });
            }

            return View(list);
        }

        [HttpGet]
        //[Authorize(Roles = "SuperAdmin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TagCreateVM request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }


            string imageName = null;


            Tag newTag = new()
            {
                Name = request.Name,
            };

            await _context.Tags.AddAsync(newTag);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {

            if (id is null) return BadRequest();
            var existTag = await _context.Tags.FirstOrDefaultAsync(m => m.Id == id);
            if (existTag is null) return NotFound();

            TagEditVM model = new()
            {
                Id = existTag.Id,
                Name = existTag.Name,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, TagEditVM request)
        {
            if (id is null)
                return BadRequest();

            var existTag = await _context.Tags.FirstOrDefaultAsync(m => m.Id == id);
            if (existTag is null)
                return NotFound();

            if (existTag.Name.Trim() != request.Name.Trim())
            {
                existTag.Name = request.Name;
            }


            _context.Update(existTag);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var existTag = await _context.Tags.FirstOrDefaultAsync(m => m.Id == id);

            _context.Remove(existTag);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
