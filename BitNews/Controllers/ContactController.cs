using BitNews.Data;
using BitNews.Models;
using BitNews.Services.Interfaces;
using BitNews.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BitNews.Controllers
{
    public class ContactController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILayoutService _layoutService;

        public ContactController(AppDbContext context, ILayoutService layoutService)
        {
            _context = context;
            _layoutService = layoutService;
        }

        public async Task<IActionResult> Index()
        {

            //Contact contact = await _context.Contacts.FirstOrDefaultAsync();

            //Contact model = contact;

            ////ContactVM model = new ContactVM
            ////{
            ////    Contacts = new List<Contact> { contact }
            ////};

            //return View(model);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateContact(Contact model)
        {


            if (ModelState.IsValid)
            {
                _context.Contacts.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(model);
        }








    }

}
