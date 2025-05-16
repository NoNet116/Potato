using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Potato.DbContext.Models.Entity;
using Potato.ViewModels;

namespace Potato.Controllers
{
    public class SearchController : Controller
    {
        private IMapper _mapper;

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public SearchController(IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        
        [Route("UserList")]
        [HttpPost]
        public IActionResult UserList(string search)
        {
            var model = new SearchViewModel
            {
                UserList = string.IsNullOrEmpty(search) ?
                _userManager.Users.ToList() : _userManager.Users.AsEnumerable().Where(x => x.GetFullName().Contains(search, StringComparison.CurrentCultureIgnoreCase)).ToList()
            };
            return View("UserList", model);
        }
    }
}
