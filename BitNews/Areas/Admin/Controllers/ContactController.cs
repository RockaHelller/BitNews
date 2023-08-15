using BitNews.Areas.Admin.ViewModels.Contact;
using BitNews.Data;
using BitNews.Models;
using BitNews.Services;
using BitNews.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BitNews.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
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
            List<Contact> contacts = await _context.Contacts.ToListAsync(); // Fetch all contacts

            return View(contacts);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            Contact contact = await _context.Contacts.FirstOrDefaultAsync(c => c.Id == id);

            if (contact == null)
            {
                return NotFound();
            }

            ContactDetailVM model = new ContactDetailVM
            {
                Name = contact.Name,
                Email = contact.Email,
                Subject = contact.Subject,
                Message = contact.Message,
                CreatorName = contact.CreatorName,
                
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var existMessage = await _context.Contacts.FirstOrDefaultAsync(m => m.Id == id);

            _context.Remove(existMessage);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }





    }
}
