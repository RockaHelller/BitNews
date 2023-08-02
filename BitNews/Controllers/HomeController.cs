

using BitNews.Data;
using BitNews.Models;
using BitNews.Services.Interfaces;
using BitNews.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BitNews.Controllers
{
    public class HomeController : Controller
    {
		private readonly AppDbContext _context;
		private readonly ISliderService _sliderService;

		public HomeController(AppDbContext context, ISliderService sliderService)
		{
			_context = context;
			_sliderService = sliderService;
		}

		public async Task<IActionResult> Index()
		{
			IEnumerable<Slider> sliders = await _sliderService.GetAllByStatusAsync();
			HomeVM model = new HomeVM
			{
				Sliders = sliders.ToList()
			};

			return View(model); // Pass the single HomeVM object to the view
		}




	}
}