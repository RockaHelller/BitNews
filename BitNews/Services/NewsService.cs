using BitNews.Areas.Admin.ViewModels.News;
using BitNews.Data;
using BitNews.Helpers;
using BitNews.Models;
using BitNews.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BitNews.Services
{
    public class NewsService : INewsService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public NewsService(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }


        public async Task<IEnumerable<News>> GetAllWithIncludesAsync()
        {
            return await _context.News.Include(m => m.NewsTags).
                ThenInclude(m => m.Tag).Include(m => m.Category).
                Include(m => m.Images).ToListAsync();
        }



        public async Task<News> GetByIdAsync(int? id) => await _context.News.Include(m => m.Images).
                                                                                            Include(m => m.NewsTags).
                                                                                            Include(m => m.Category).
                                                                                            Include(m => m.NewsTags).ThenInclude(m => m.Tag).
                                                                                            FirstOrDefaultAsync(m => m.Id == id);



        public async Task<News> GetByIdWithAllIncludes(int id)
        {
            return await _context.News
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Include(p => p.NewsTags)
                .ThenInclude(p => p.Tag)
                .FirstOrDefaultAsync(p => p.Id == id);
        }



        public NewsDetailVM GetMappedData(News news)
        {
            NewsDetailVM newsDetail = new NewsDetailVM
            {
                Title = news.Title,
                Image = news.Images,
                Article = news.Article,
                Description = news.Description,
                Category = news.Category?.Name,
                NewsTags = news.NewsTags,
                CreateDate = news.CreateDate.ToString("dddd, dd MMMM yyyy"),
                CreatorName = news.CreatorName,
                
            };
            return newsDetail;
        }


        public async Task CreateAsync(NewsCreateVM model)
        {
            if (model.Image != null)
            {
                string fileName = Guid.NewGuid().ToString() + "_" + model.Image.FileName;
                await model.Image.SaveFileAsync(fileName, _env.WebRootPath, "assets/img/News");



                List<NewsImage> images = new List<NewsImage>
                {
                    new NewsImage { Image = fileName, IsMain = true }
                };


                News news = new News
                {
                    Title = model.Title,
                    Article = model.Article,
                    Description = model.Description,
                    CategoryId = model.CategoryId,
                    Images = images,
                    CreatorName = model.CreatorName,
                };

                await _context.News.AddAsync(news);
                await _context.SaveChangesAsync();

                foreach (var item in model.Tags)
                {
                    if (item.IsChecked)
                    {
                        NewsTag newsTag = new NewsTag
                        {
                            NewsId = news.Id,
                            TagId = item.Id,
                        };

                        await _context.NewsTags.AddAsync(newsTag);
                    }
                }

                await _context.SaveChangesAsync();
            }
        }



        public async Task EditAsync(NewsEditVM model)
        {
            // Retrieve the news with its related tags and images
            News news = await _context.News
                .Include(n => n.NewsTags)
                .Include(n => n.Images)
                .FirstOrDefaultAsync(n => n.Id == model.Id);

            if (news == null)
            {
                // Handle the case when the news with the specified ID is not found
                // You can return an error or redirect to another action.
                return;
            }

            if (model.Image != null)
            {
                // Delete the existing image file if it exists
                if (!string.IsNullOrEmpty(news.Images?.FirstOrDefault()?.Image))
                {
                    string imagePath = Path.Combine(_env.WebRootPath, "assets/img/News", news.Images.FirstOrDefault().Image);
                    //if (System.IO.File.Exists(imagePath))
                    //{
                    //    System.IO.File.Delete(imagePath);
                    //}
                }

                // Save the new image file
                string fileName = Guid.NewGuid().ToString() + "_" + model.Image.FileName;
                await model.Image.SaveFileAsync(fileName, _env.WebRootPath, "assets/img/News");

                // Update the news image
                news.Images = new List<NewsImage>
        {
            new NewsImage { Image = fileName, IsMain = true }
        };
            }

            // Update other news properties
            news.Title = model.Title;
            news.Article = model.Article;
            news.Description = model.Description;
            news.CategoryId = model.CategoryId;

            // Update the NewsTags
            news.NewsTags.Clear();
            foreach (var item in model.Tags)
            {
                if (item.IsChecked)
                {
                    news.NewsTags.Add(new NewsTag
                    {
                        NewsId = news.Id,
                        TagId = item.Id,
                       
                    });
                }
            }

            await _context.SaveChangesAsync();
        }



    }
}