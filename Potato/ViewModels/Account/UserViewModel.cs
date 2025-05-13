using Potato.DbContext.Models.Entity;

namespace Potato.ViewModels.Account
{
    public class UserViewModel
    {
        public User User { get; private set; }

        public IEnumerable<Frined> Friends { get; set; }
        public UserViewModel(User user)
        {
            User = user;
        }
    }
}
