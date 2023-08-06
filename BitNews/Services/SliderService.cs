using BitNews.Areas.Admin.ViewModels.Slider;
using BitNews.Data;
using BitNews.Helpers;
using BitNews.Models;
using BitNews.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace BitNews.Services
{
    public class SliderService : ISliderService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SliderService(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<int> GetCountAsync() => await _context.Sliders.Where(m => m.Status).CountAsync();

        public async Task CreateAsync(List<IFormFile> images, string title, string description)
        {
            foreach (var item in images)
            {
                string fileName = Guid.NewGuid().ToString() + "_" + item.FileName;

                await item.SaveFileAsync(fileName, _env.WebRootPath, "assets/img/Slider");

                Slider slider = new()
                {
                    Image = fileName,
                    Title = title,
                    Description = description,
                };

                await _context.Sliders.AddAsync(slider);
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                Slider slider = await GetByIdAsync(id);

                if (slider == null)
                {
                    // Handle the case where the slider with the given ID doesn't exist
                    // or return an appropriate response
                    return;
                }

                string path = Path.Combine(_env.WebRootPath, "assets", "img", "Slider", slider.Image);

                if (File.Exists(path))
                {
                    File.Delete(path);
                }

                _context.Sliders.Remove(slider);

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var entry = ex.Entries.Single();
                await entry.ReloadAsync();
                await DeleteAsync(id);
            }
        }

        public async Task EditAsync(int id, SliderEditVM model)
        {
            var slider = await GetWithIncludesAsync(id);

            if (slider == null)
            {
                // Handle the case where the slider with the given ID doesn't exist
                // or return an appropriate response
                return;
            }

            if (model.NewImage != null)
            {
                // Delete the old image file if it exists
                if (!string.IsNullOrEmpty(slider.Image))
                {
                    string oldImagePath = Path.Combine(_env.WebRootPath, "assets", "img", "Slider", slider.Image);
                    if (File.Exists(oldImagePath))
                    {
                        File.Delete(oldImagePath);
                    }
                }
            }


            if (model.NewImage != null)
            {
                foreach (var item in model.NewImage)
                {
                    string fileName = Guid.NewGuid().ToString() + "_" + item.FileName;
                    await item.SaveFileAsync(fileName, _env.WebRootPath, "assets/img/Slider");
                    slider.Image = fileName;
                }
            }

            slider.Title = model.NewTitle;
            slider.Description = model.NewDesc;

            await _context.SaveChangesAsync();
        }



        public async Task<List<Slider>> GetAllAsync()
        {
            return await _context.Sliders.ToListAsync();
        }

        public async Task<List<Slider>> GetAllByStatusAsync()
        {
            return await _context.Sliders.Where(m => m.Status).ToListAsync();
        }

        public async Task<List<SliderVM>> GetAllMappedDatasAsync()
        {
            List<SliderVM> sliderList = new();

            List<Slider> sliders = await GetAllAsync();

            foreach (Slider slider in sliders)
            {
                SliderVM model = new()
                {
                    Id = slider.Id,
                    Image = slider.Image,
                    Status = slider.Status,
                    CreateDate = slider.CreateDate.ToString("dddd, dd MMMM yyyy"),
                };

                sliderList.Add(model);
            }

            return sliderList;
        }

        public async Task<Slider> GetByIdAsync(int id)
        {
            return await _context.Sliders.FirstOrDefaultAsync(m => m.Id == id);
        }

        public SliderDetailVM GetMappedData(Slider slider)
        {
            SliderDetailVM model = new()
            {
                Image = slider.Image,
                Status = slider.Status,
                Title = slider.Title,
                Description = slider.Description,
                CreateDate = slider.CreateDate.ToString("dddd, dd MMMM yyyy"),

            };

            return model;
        }

        public async Task<bool> ChangeStatusAsync(Slider slider)
        {
            if (slider.Status && await GetCountAsync() != 1)
            {
                slider.Status = false;
            }
            else
            {
                slider.Status = true;
            }

            await _context.SaveChangesAsync();

            return slider.Status;
        }


        public async Task<Slider> GetWithIncludesAsync(int id)
        {
            return await _context.Sliders.FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}
