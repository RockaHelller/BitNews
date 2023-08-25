using BitNews.Data;
using BitNews.Models;
using BitNews.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BitNews.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;

        public CategoryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Category>> GetAll()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            return await _context.Categories
                .Include(c => c.News)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
