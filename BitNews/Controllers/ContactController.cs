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
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateContact(Contact model)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (ModelState.IsValid)
                {
                    model.CreatorName = User.Identity.Name; // Set the CreatorName from authenticated user

                    _context.Contacts.Add(model);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            else
            {
                // Handle the case where the user is not authenticated (e.g., show an error message)
                ModelState.AddModelError("", "You must be logged in to submit a contact.");
            }

            return View("Index", model); // Return to the contact form with errors
        }

    }

}
