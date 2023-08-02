using BitNews.Models;

namespace BitNews.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<List<Category>> GetAll();
        Task<Category> GetByIdAsync(int id);
    }
}
