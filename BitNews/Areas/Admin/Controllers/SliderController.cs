using BitNews.Areas.Admin.ViewModels.Slider;
using BitNews.Areas.Admin.ViewModels.Slider;
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
    public class SliderController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly ISliderService _sliderService;

        public SliderController(IWebHostEnvironment env, ISliderService sliderService)
        {
            _env = env;
            _sliderService = sliderService;
        }

        [HttpGet]
        public async Task<IActionResult> Index() =>  View(await _sliderService.GetAllMappedDatasAsync());

        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null) return BadRequest();

            Slider dbSlider = await _sliderService.GetByIdAsync((int)id);

            if (dbSlider is null) return NotFound();

            return View(_sliderService.GetMappedData(dbSlider));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SliderCreateVM request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            foreach (var item in request.Images)
            {
                if (!item.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Image", "Please select only image file");
                    return View();
                }

                if (item.CheckFileSize(20000))
                {
                    ModelState.AddModelError("Image", "Image size must be max 200 KB");
                    return View();
                }
            }

            await _sliderService.CreateAsync(request.Images, request.Title, request.Description);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _sliderService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return BadRequest();

            Slider dbSlider = await _sliderService.GetByIdAsync((int)id);

            if (dbSlider is null) return NotFound();

            return View(new SliderEditVM { Image = dbSlider.Image, NewTitle = dbSlider.Title, NewDesc = dbSlider.Description });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, SliderEditVM request)
        {
            if (id is null) return BadRequest();

            Slider dbSlider = await _sliderService.GetWithIncludesAsync((int)id);
            if (dbSlider is null) return NotFound();


            var oldImagePath = Path.Combine("wwwroot/assets/img/Slider", dbSlider.Image);

            if (request.NewImage != null)
            {
                foreach (var item in request.NewImage)
                {
                    if (!item.CheckFileType("image/"))
                    {
                        ModelState.AddModelError("Image", "Please select only image file");
                        request.Image = dbSlider.Image;
                        return View(request);
                    }

                    if (item.CheckFileSize(20000))
                    {
                        ModelState.AddModelError("Image", "Image size must be max 20 MB");
                        request.Image = dbSlider.Image;
                        return View(request);
                    }
                }
            }

            await _sliderService.EditAsync((int)id, request);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> ChangeStatus(int? id)
        {
            if (id == null) return BadRequest();

            Slider slider = await _sliderService.GetByIdAsync((int)id);

            if (slider is null) return NotFound();

            return Ok(await _sliderService.ChangeStatusAsync(slider));
        }
    }
}
