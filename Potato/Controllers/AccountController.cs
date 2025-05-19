using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Potato.DbContext.Models.Entity;
using Potato.ViewModels.Account;

namespace Potato.Controllers
{
    public class AccountController : Controller
    {
        private IMapper _mapper;

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
        }


        [Route("Login")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userModel = _mapper.Map<User>(model);
                var user = await _userManager.FindByEmailAsync(userModel.Email);

                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);

                    if (result.Succeeded)
                    {
                        return Redirect("/" + user.UserName);
                    }
                }
                ModelState.AddModelError("", "Неправильный логин или пароль.");
            }
            return View("Views/Home/Index.cshtml");
        }

        [Route("Logout")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [Route("Register")]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Проверка на существующий email
                var existingUser = await _userManager.FindByEmailAsync(model.EmailReg);
                if (existingUser != null)
                {
                    ModelState.AddModelError("EmailReg", "Пользователь с таким email уже существует.");
                    return View("_RegisterPart", model);
                }

                var user = _mapper.Map<User>(model);
                var result = await _userManager.CreateAsync(user, model.PasswordReg);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        switch (error.Code)
                        {
                            case "DuplicateUserName":
                                ModelState.AddModelError(nameof(model.UserName), "Имя пользователя уже занято.");
                                break;


                            default:
                                // Ошибка без конкретного поля — выводим в общее место
                                ModelState.AddModelError(string.Empty, error.Description);
                                break;
                        }
                    }
                }
            }

            return View("/Views/Home/Register.cshtml");
        }
    }
}
