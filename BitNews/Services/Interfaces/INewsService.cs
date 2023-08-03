using BitNews.Areas.Admin.ViewModels.News;
using BitNews.Models;

namespace BitNews.Services.Interfaces
{
    public interface INewsService
    {
        Task<IEnumerable<News>> GetAllWithIncludesAsync();
        Task<News> GetByIdAsync(int? id);
        Task<News> GetByIdWithAllIncludes(int id);
        NewsDetailVM GetMappedData(News product);
        Task EditAsync(int productId, NewsEditVM model);
        Task AddAsync(NewsCreateVM model);
        Task CreateAsync(NewsCreateVM model);
        Task EditAsync(NewsEditVM model);



    }
}
