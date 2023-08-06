using BitNews.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BitNews.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "SuperAdmin,Admin")]
	public class DashboardController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}