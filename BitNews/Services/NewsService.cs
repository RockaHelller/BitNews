using BitNews.Areas.Admin.Views.News;
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

		public Task EditAsync(int newsId, NewsEditVM model)
		{
			throw new NotImplementedException();
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

		public int Generate5DigitNumber()
		{
			Random random = new Random();
			int min = 10000;
			int max = 99999;
			return random.Next(min, max);
		}

		public async Task CreateAsync(NewsCreateVM model)
		{
			// Handle the single image file
			var file = model.Image;
			if (file != null)
			{
				string fileName = Guid.NewGuid().ToString() + "_" + file.FileName;
				await file.SaveFileAsync(fileName, _env.WebRootPath, "assets/img/News");

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

	}
}