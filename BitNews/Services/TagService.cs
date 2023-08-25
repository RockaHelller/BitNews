using BitNews.Data;
using BitNews.Models;
using BitNews.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BitNews.Services
{
    public class TagService : ITagService
    {
        private readonly AppDbContext _context;

        public TagService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Tag>> GetAll()
        {
            return await _context.Tags.ToListAsync();
        }

        public async Task<Tag> GetByIdAsync(int id)
        {
            return await _context.Tags
                .Include(t => t.NewsTag)
                .FirstOrDefaultAsync(t => t.Id == id);
        }
    }
}
