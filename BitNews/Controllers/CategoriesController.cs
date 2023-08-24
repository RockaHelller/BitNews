using BitNews.Data;
using BitNews.Services.Interfaces;
using BitNews.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BitNews.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ICategoryService _categoryService;

        public CategoriesController(AppDbContext context, ICategoryService categoryService)
        {
            _context = context;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var category = await _categoryService.GetAll();

            CategoryVM model = new()
            {
                Categories = category
            }; 
            
            
            return View(model);
        }
    }
}
