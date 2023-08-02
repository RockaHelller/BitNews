using Microsoft.AspNetCore.Mvc;

namespace BitNews.Controllers
{
	public class ContactController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
