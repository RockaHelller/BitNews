using Microsoft.AspNetCore.Mvc;

namespace BitNews.Areas.Admin.Controllers
{
    public class NewsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
