using Microsoft.AspNetCore.Mvc;

namespace BitNews.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
