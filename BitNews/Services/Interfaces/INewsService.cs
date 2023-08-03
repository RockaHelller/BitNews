using BitNews.Areas.Admin.ViewModels.News;
using BitNews.Models;

namespace BitNews.Services.Interfaces
{
    public interface INewsService
    {
        Task<IEnumerable<News>> GetAllWithIncludesAsync();
        Task<News> GetByIdAsync(int? id);
        Task<News> GetByIdWithAllIncludes(int id);
        NewsDetailVM GetMappedData(News news);
        Task CreateAsync(NewsCreateVM model);
        Task EditAsync(NewsEditVM model);
    }
}
