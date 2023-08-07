using BitNews.Areas.Admin.ViewModels.Category;
using BitNews.Areas.Admin.ViewModels.Tag;
using BitNews.Data;
using BitNews.Helpers;
using BitNews.Models;
using BitNews.Services.Interfaces;
using BitNews.ViewModels;
using BitNews.ViewModels.Profile;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BitNews.Controllers
{
    public class ProfileController : Controller
    {
        private readonly INewsService _newsService;
        private readonly ISettingService _settingService;
        private readonly ICategoryService _categoryService;
        private readonly AppDbContext _context;
        private readonly ILayoutService _layoutService;



        public ProfileController(INewsService productService,
                                     ISettingService settingService,
                                     ICategoryService categoryService,
                                     AppDbContext context,
                                     ILayoutService layoutService)
        {
            _newsService = productService;
            _settingService = settingService;
            _categoryService = categoryService;
            _context = context;
            _layoutService = layoutService;
        }

        public async Task<IActionResult> Index()
        {
            var datas = await _layoutService.GetAllDatas();
            return View(datas); // Assuming datas is of type LayoutVM
        }

        [HttpGet]
        public async Task<IActionResult> EditImage(int? id)
        {

            if (id is null) return BadRequest();
            var existProfile = await _context.Categories.FirstOrDefaultAsync(m => m.Id == id);
            if (existProfile is null) return NotFound();

            EditImageVM model = new()
            {
                Id = existProfile.Id,
                Image = existProfile.Image,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditImage(int? id, EditImageVM request)
        {
            if (id is null)
                return BadRequest();

            var existProfile = await _context.Users.FirstOrDefaultAsync();
            if (existProfile is null)
                return NotFound();

            // Get the old image file path from the existing category
            var oldImagePath = Path.Combine("wwwroot/assets/img/user_profile_pic", existProfile.Image);

            if (request.NewImage != null)
            {
                if (!request.NewImage.CheckFileType("image/"))
                {
                    ModelState.AddModelError("NewImage", "Please select only an image file");
                    return View();
                }

                if (request.NewImage.CheckFileSize(2000))
                {
                    ModelState.AddModelError("NewImage", "Image size must be a maximum of 200 KB");
                    return View();
                }

                var imageName = Guid.NewGuid().ToString() + Path.GetExtension(request.NewImage.FileName);

                var imagePath = Path.Combine("wwwroot/assets/img/user_profile_pic", imageName);
                using (var fileStream = new FileStream(imagePath, FileMode.Create))
                {
                    await request.NewImage.CopyToAsync(fileStream);
                }

                // If a new image is provided, delete the old image from the folder
                //if (System.IO.File.Exists(oldImagePath))
                //{
                //    System.IO.File.Delete(oldImagePath);
                //}

                existProfile.Image = imageName;
            }

            _context.Update(existProfile);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }



        //[HttpGet]
        //public async Task<IActionResult> EditInfo(int? id)
        //{
        //    if (id is null) return BadRequest();
        //    var existUser = await _context.Users.FirstOrDefaultAsync();
        //    if (existUser is null) return NotFound();

        //    ProfileEditVM model = new()
        //    {
        //        FullName = existUser.FullName,
        //        UserName = existUser.UserName,
        //        Email = existUser.Email,
        //        Phone = existUser.PhoneNumber,
        //        Address = existUser.UserAddress
        //    };

        //    return PartialView("_EditInfoModal", model);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> EditInfo(ProfileEditVM model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        // If the model state is not valid, return to the view with the model to show validation errors.
        //        return View(model);
        //    }

        //    // Retrieve the current user (or get the user using their ID) from your data source.
        //    var existingUser = await _context.Users.FirstOrDefaultAsync();
        //    if (existingUser == null)
        //    {
        //        // Handle the case where the user is not found (optional).
        //        return NotFound();
        //    }

        //    // Update the user's profile information with the edited values.
        //    existingUser.FullName = model.FullName;
        //    existingUser.Email = model.Email;
        //    existingUser.PhoneNumber = model.Phone;
        //    existingUser.UserAddress = model.Address;

        //    // Save the changes to the data source.
        //    _context.Update(existingUser);
        //    await _context.SaveChangesAsync();

        //    // Redirect to the profile page or any other desired action.
        //    return RedirectToAction(nameof(Index));
        //}


    }
}
