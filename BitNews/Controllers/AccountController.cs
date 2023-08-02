using Microsoft.AspNetCore.Mvc;

namespace BitNews.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }











        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }
    }
}
