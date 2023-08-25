using BitNews.Areas.Admin.ViewModels.About;
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
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class AboutController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILayoutService _layoutService;

        public AboutController(AppDbContext context, ILayoutService layoutService)
        {
            _context = context;
            _layoutService = layoutService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var about = await _context.Abouts.FirstOrDefaultAsync();

            var model = new AboutVM
            {
                Id = about.Id,
                Title = about.Title,
                Description = about.Description,
                Mission = about.Mission,
                WhatWeDo = about.WhatWeDo,
                Image = about.Image
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            var existAbout = await _context.Abouts.FirstOrDefaultAsync(m => m.Id == id);
            if (existAbout is null) return NotFound();

            AboutEditVM model = new()
            {
                Title = existAbout.Title,
                Description = existAbout.Description,
                Mission = existAbout.Mission,
                WhatWeDo = existAbout.WhatWeDo,
                Image = existAbout.Image,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, AboutEditVM request)
        {
            if (id is null)
                return BadRequest();

            var existAbout = await _context.Abouts.FirstOrDefaultAsync(m => m.Id == id);
            if (existAbout is null)
                return NotFound();

            if (existAbout.Title.Trim() != request.Title.Trim())
            {
                existAbout.Title = request.Title;
            }

            if (existAbout.Description.Trim() != request.Description.Trim())
            {
                existAbout.Description = request.Description;
            }

            if (existAbout.Mission.Trim() != request.Mission.Trim())
            {
                existAbout.Mission = request.Mission;
            }

            if (existAbout.WhatWeDo.Trim() != request.WhatWeDo.Trim())
            {
                existAbout.WhatWeDo = request.WhatWeDo;
            }

            var oldImagePath = Path.Combine("wwwroot/assets/img/About", existAbout.Image);

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

                var imagePath = Path.Combine("wwwroot/assets/img/About", imageName);
                using (var fileStream = new FileStream(imagePath, FileMode.Create))
                {
                    await request.NewImage.CopyToAsync(fileStream);
                }

                existAbout.Image = imageName;
            }

            _context.Update(existAbout);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
