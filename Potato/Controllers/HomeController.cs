using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Potato.DbContext.Models.Entity;
using Potato.DbContext.Repository;
using Potato.Helpers;
using Potato.ViewModels.Account;

namespace Potato.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly GenerateUsers _userGenerator;


        public HomeController(ILogger<HomeController> logger, 
            UserManager<User> userManager, 
            SignInManager<User> signInManager, 
            IUnitOfWork unitOfWork,
            GenerateUsers userGenerator)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _userGenerator = userGenerator;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Index()
        {
            if (!_signInManager.IsSignedIn(User))
                return View();

            var user = await _userManager.GetUserAsync(User);
            
            return user == null
                ? NotFound()
                : RedirectToAction("Profile", new { UserName = user.UserName });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [HttpGet("{UserName}")]
        public async Task<IActionResult> Profile(string UserName)
        {
            var user = await _userManager.FindByNameAsync(UserName);
            if (user == null) return NotFound();

            var model = new UserViewModel(user);
            model.Friends = await GetAllFriend(user);
            return View("Index", model); 
        }
        public IActionResult Register()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            
            return View(new ViewModels.ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<List<User>> GetAllFriend(User user)
        {
            var repository = _unitOfWork.GetRepository<Friend>() as FriendsRepository;
            return repository.GetFriendsByUser(user); // просто передаём нужного пользователя
        }


        [Route("Generate")]
        [HttpGet]
        public async Task<IActionResult> Generate()
        {

            var userlist = await _userGenerator.PopulateAsync(10);
            int successCount = 0;

            foreach (var user in userlist)
            {
                var result = await _userManager.CreateAsync(user, "12345");
                if (result.Succeeded)
                    successCount++;
            }
            TempData["Notification"] = $"Создано {userlist.Count} пользователей"+Environment.NewLine+"Для авторизации используйте почту и пароль 12345";
            return RedirectToAction("Index", "Home");
        }
    }
}
