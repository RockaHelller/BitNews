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

        public Task AddAsync(NewsCreateVM model)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(int id)
        {
            News news = await GetByIdAsync(id);

            _context.News.Remove(news);

            await _context.SaveChangesAsync();

            string path = Path.Combine(_env.WebRootPath, "wwwroot/assets/img/News", news.Images.FirstOrDefault().Image);

            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        //public Task EditAsync(int newsId, NewsEditVM model)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task EditAsync(NewsEditVM model)
        {
            List<NewsImage> images = new List<NewsImage>();

            if (model.Image != null)
            {
                foreach (var file in model.Image)
                {
                    string fileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                    await file.SaveFileAsync(fileName, _env.WebRootPath, "wwwroot/assets/img/News");
                    images.Add(new NewsImage { Image = fileName, IsMain = false });
                }
                var existImage = await _context.NewsImages.ToListAsync();

                foreach (var item in existImage)
                {
                    if (item.NewsId == model.Id)
                    {
                        _context.Remove(item);
                    }
                }
                await _context.SaveChangesAsync();
            }

            if (images.Count > 0)
            {
                images[0].IsMain = true; // Set the first image as the main image
            }

            News product = new News
            {
                Id = model.Id,
                CategoryId = model.CategoryId,
                Article = model.Article,
                Images = images,
                Title = model.Title,
            };

            foreach (var item in model.Tags)
            {

                var existTag = await _context.NewsTags.ToListAsync();

                foreach (var tags in existTag)
                {
                    if (tags.NewsId == product.Id)
                    {
                        _context.Remove(tags);
                    }
                }
            }



            foreach (var item in model.Tags)
            {
                if (item.IsChecked)
                {
                    NewsTag NewsTag = new NewsTag
                    {
                        NewsId = product.Id,
                        TagId = item.Id
                    };
                    await _context.NewsTags.AddAsync(NewsTag);
                }
            }




            _context.Update(product);
            await _context.SaveChangesAsync();
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
                .Include(p => p.NewsTags) // Include NewsTags
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
                Category = news.Category?.Name,
                NewsTags = news.NewsTags,
                CreateDate = news.CreateDate.ToString("dddd, dd MMMM yyyy")
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
                    CategoryId = model.CategoryId,
                    Images = images
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
                            TagId = item.Id
                        };

                        await _context.NewsTags.AddAsync(newsTag);
                    }
                }

                await _context.SaveChangesAsync();
            }
        }

        public Task EditAsync(int productId, NewsEditVM model)
        {
            throw new NotImplementedException();
        }
    }
}