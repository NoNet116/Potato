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
            var result = await _userManager.GetUserAsync(User);
            var allUsers = _userManager.Users.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var lowerSearch = search.ToLower();
                allUsers = allUsers.Where(u => u.GetFullName().ToLower().Contains(lowerSearch));
            }

            var userList = new List<UserWithFriendExt>();

            if (result != null)
            {
                var friends = await GetAllFriend();
                foreach (var user in allUsers)
                {
                    var mappedUser = _mapper.Map<UserWithFriendExt>(user);
                    mappedUser.IsFriendWithCurrent = friends.Any(f => f.Id == user.Id || user.Id == result.Id);
                    userList.Add(mappedUser);
                }
            }
            else
            {
                userList = allUsers.Select(u => _mapper.Map<UserWithFriendExt>(u)).ToList();
            }

            return new SearchViewModel
            {
                UserList = userList
            };
        }


        public async Task<List<User>> GetAllFriend()
        {
            var user = User;

            var result = await _userManager.GetUserAsync(user);

            var repository = _unitOfWork.GetRepository<Friend>() as FriendsRepository;

            return repository.GetFriendsByUser(result);
        }
    }
}
