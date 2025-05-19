using Potato.DbContext.Models.Entity;

namespace Potato.ViewModels.Account
{
    public class UserViewModel
    {
        public User User { get; private set; }

        public List<User> Friends { get; set; }
        public UserViewModel(User user)
        {
            User = user;
        }
    }
}
