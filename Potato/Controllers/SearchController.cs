using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Potato.DbContext.Models.Entity;
using Potato.DbContext.Repository;
using Potato.ViewModels;

namespace Potato.Controllers
{
    public class SearchController : Controller
    {
        private IMapper _mapper;

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IUnitOfWork _unitOfWork;

        public SearchController(IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager, IUnitOfWork unitOfWork )
        {
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
        }


        [Route("UserList")]
        [HttpGet]
        public async Task<IActionResult> UserList(string search)
        {
            var model = await CreateSearch(search);
            return View("UserList", model);
        }

        private async Task<SearchViewModel> CreateSearch(string search)
        {
            var currentUser = User;
            var result = await _userManager.GetUserAsync(currentUser);

            IEnumerable<User> list;

            if (string.IsNullOrWhiteSpace(search))
            {
                list = _userManager.Users.AsEnumerable();
            }
            else
            {
                var lowerSearch = search.ToLower();
                list = _userManager.Users.AsEnumerable()
                    .Where(x => x.GetFullName().ToLower().Contains(lowerSearch));
            }

            var withFriend = await GetAllFriend();

            var data = new List<UserWithFriendExt>();

            foreach (var x in list)
            {
                var t = _mapper.Map<UserWithFriendExt>(x);
                t.IsFriendWithCurrent = withFriend.Any(y => y.Id == x.Id || x.Id == result.Id);
                data.Add(t);
            }

            var model = new SearchViewModel()
            {
                UserList = data
            };

            return model;
        }


        private async Task<List<User>> GetAllFriend()
        {
            var user = User;

            var result = await _userManager.GetUserAsync(user);

            var repository = _unitOfWork.GetRepository<Friend>() as FriendsRepository;

            return repository.GetFriendsByUser(result);
        }
    }
}
