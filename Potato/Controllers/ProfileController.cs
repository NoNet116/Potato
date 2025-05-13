using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Potato.DbContext.Models.Entity;
using Potato.ViewModels.Account;
using System.Security.Claims;

namespace Potato.Controllers
{
    public class ProfileController : Controller
    {

        private IMapper _mapper;

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public ProfileController(IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [Authorize]
        public async Task<IActionResult> Profile()
        {
            // Если не авторизован — отдаем гостевую-страницу
            if (!User.Identity.IsAuthenticated)
            {
                return View();
            }

            // Авторизованный пользователь
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var model = new UserViewModel(user);

            return View("~/Views/Home/Profile.cshtml", model);

        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UploadPhoto(IFormFile ProfileImage)
        {
            if (ProfileImage != null && ProfileImage.Length > 0)
            {
                var user = await _userManager.GetUserAsync(User);

                // Генерация уникального имени файла
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ProfileImage.FileName);
                var filePath = Path.Combine("wwwroot/images/profiles", fileName);

                // Создание директории при необходимости
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                // Сохранение файла
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ProfileImage.CopyToAsync(stream);
                }

                // Сохранение пути к изображению в базе
                user.Image = "/images/profiles/" + fileName;
                await _userManager.UpdateAsync(user);

                return Redirect(Request.Headers["Referer"].ToString());
            }

            TempData["UploadError"] = "Файл не выбран или пуст.";
            return Redirect(Request.Headers["Referer"].ToString());
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeletePhoto()
        {
            var user = await _userManager.GetUserAsync(User);

            if (!string.IsNullOrEmpty(user.Image))
            {
                var filePath = Path.Combine("wwwroot", user.Image.TrimStart('/'));

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                user.Image = null;
                await _userManager.UpdateAsync(user);
            }

            return Redirect(Request.Headers["Referer"].ToString());
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit()
        {
            var user = await _userManager.GetUserAsync(User);

            var model = new EditProfileViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                BirthDate = user.BirthDate,
                Status = user.Status,
                About = user.About,
                UserName = user.UserName,
                Email = user.Email

            };

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(EditProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;

            user.BirthDate = model.BirthDate;
            user.Status = model.Status;
            user.About = model.About;
            user.Email = model.Email;
            user.UserName = model.UserName;


            await _userManager.UpdateAsync(user);

            return Redirect("/" + user.UserName);
        }



    }
}
