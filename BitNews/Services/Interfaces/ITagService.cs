using BitNews.Models;

namespace BitNews.Services.Interfaces
{
    public interface ITagService
    {
        Task<List<Tag>> GetAll();
        Task<Tag> GetByIdAsync(int id);
    }
}
