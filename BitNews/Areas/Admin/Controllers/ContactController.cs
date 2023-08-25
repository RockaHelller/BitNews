using BitNews.Areas.Admin.ViewModels.Contact;
using BitNews.Data;
using BitNews.Models;
using BitNews.Services;
using BitNews.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Net.Mail;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace BitNews.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class ContactController : Controller
    {
        private readonly AppDbContext _context;


        public ContactController(AppDbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            List<Contact> contacts = await _context.Contacts.ToListAsync();

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
                EmailModel = new MailModel(),
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Detail(int id, ContactDetailVM model)
        {
            Contact contact = await _context.Contacts.FirstOrDefaultAsync(c => c.Id == id);

            if (contact == null)
            {
                return NotFound();
            }


            MailMessage mail = new MailMessage();
            mail.To.Add(contact.Email);
            mail.From = new MailAddress("muradjj@code.edu.az");
            mail.Subject = contact.Subject;
            string Body = model.EmailModel.Body;
            mail.Body = Body;
            mail.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential("muradjj@code.edu.az", "bmdihsnjpfswqwvb");
            smtp.EnableSsl = true;

            smtp.Send(mail);

            return RedirectToAction(nameof(Index));
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAll()
        {
            var allContacts = await _context.Contacts.ToListAsync();

            if (allContacts.Count == 0)
            {
                return RedirectToAction(nameof(Index));
            }

            _context.Contacts.RemoveRange(allContacts);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
