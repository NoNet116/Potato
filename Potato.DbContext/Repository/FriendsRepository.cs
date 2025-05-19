using Microsoft.EntityFrameworkCore;
using Potato.DbContext.Models.Entity;

namespace Potato.DbContext.Repository
{
    public class FriendsRepository : Repository<Friend>
    {
        public FriendsRepository(AppDbContext db) : base(db)
        {

        }

        public void AddFriend(User target, User Friend)
        {
            var friends = Set.AsEnumerable().FirstOrDefault(x => x.UserId == target.Id && x.CurrentFriendId == Friend.Id);

            if (friends == null)
            {
                var item = new Friend()
                {
                    UserId = target.Id,
                    User = target,
                    CurrentFriend = Friend,
                    CurrentFriendId = Friend.Id,
                };

                Create(item);
            }
        }

        public List<User> GetFriendsByUser(User target)
        {
            var friends = Set.Include(x => x.CurrentFriend).Include(x => x.User).AsEnumerable().Where(x => x.User.Id == target.Id).Select(x => x.CurrentFriend);

            return friends.ToList();
        }

       /* public List<User> GetFriendsByUser(User target)
        {
            if (target == null || string.IsNullOrEmpty(target.Id))
                throw new ArgumentNullException(nameof(target), "Пользователь не может быть null");

            var friends = Set.Include(x => x.CurrentFriend)
                             .Include(x => x.User)
                             .Where(x => x.User != null && x.User.Id == target.Id)
                             .Select(x => x.CurrentFriend)
                             .Where(cf => cf != null) // защищаем от null
                             .ToList();

            return friends;
        }*/
        public void DeleteFriend(User target, User Friend)
        {
            var friends = Set.AsEnumerable().FirstOrDefault(x => x.UserId == target.Id && x.CurrentFriendId == Friend.Id);

            if (friends != null)
            {
                Delete(friends);
            }
        }

    }
}
