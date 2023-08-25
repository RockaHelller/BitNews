using BitNews.Areas.Admin.ViewModels.Setting;
using BitNews.Data;
using BitNews.Models;
using BitNews.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BitNews.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class SettingController : Controller
    {
        private readonly AppDbContext _context;

        public SettingController(AppDbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            var settings = await _context.Settings.OrderByDescending(x => x.Key).ToListAsync();

            List<SettingVM> modelList = new List<SettingVM>();

            foreach (var item in settings)
            {
                var settingVM = new SettingVM
                {
                    Id = item.Id,
                    Key = item.Key,
                    Value = item.Value
                };

                modelList.Add(settingVM);
            }

            return View(modelList);
        }



        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SettingCreateVM request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            Setting newSetting = new()
            {
                Key = request.Key,
                Value = request.Value
            };

            await _context.Settings.AddAsync(newSetting);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return BadRequest();
            var existSetting = await _context.Settings.FirstOrDefaultAsync(m => m.Id == id.Value);
            if (existSetting is null) return NotFound();

            SettingEditVM model = new()
            {
                Key = existSetting.Key,
                Value = existSetting.Value
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, SettingEditVM request)
        {
            if (id is null)
                return BadRequest();

            var existSetting = await _context.Settings.FirstOrDefaultAsync(m => m.Id == id);
            if (existSetting is null)
                return NotFound();

            if (existSetting.Key.Trim() != request.Key.Trim())
            {
                existSetting.Key = request.Key;
            }

            if (existSetting.Value.Trim() != request.Value.Trim())
            {
                existSetting.Value = request.Value;
            }

            _context.Update(existSetting);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var setting = await _context.Settings.FirstOrDefaultAsync(s => s.Id == id);

            if (setting != null)
            {
                _context.Settings.Remove(setting);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
