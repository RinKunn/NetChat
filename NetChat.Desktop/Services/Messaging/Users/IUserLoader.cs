using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChat.Desktop.Services.Messaging.Users
{
    public interface IUserLoader
    {
        Task<IEnumerable<User>> LoadUsers();
        bool IsMe(string userId);
        Task<int> OnlineUsersCount();
    }

    public class DefaultUserLoader : IUserLoader
    {
        public bool IsMe(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<User>> LoadUsers()
        {
            var users = new List<User>();
            users.Add(new User() { Id = "UserMe", Status = UserStatus.Online, StatusChangedDateTime = DateTime.Now });
            users.Add(new User() { Id = "UserOffline", Status = UserStatus.Offline, StatusChangedDateTime = DateTime.Now });
            users.Add(new User() { Id = "UserOnline", Status = UserStatus.Online, StatusChangedDateTime = DateTime.Now });
            await Task.Delay(5000).ConfigureAwait(false);
            return users;
        }

        public async Task<int> OnlineUsersCount()
        {
            await Task.Delay(5000).ConfigureAwait(false);
            return 3;
        }
    }
}
