using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChat.Desktop.Services.Messaging.Users
{
    public interface IUserLoader
    {
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUserById(string userId);
        bool IsMe(string userId);
        Task<int> OnlineUsersCount();
    }

    public class DefaultUserLoader : IUserLoader
    {
        private readonly Dictionary<string, User> _users = new Dictionary<string, User>();

        public DefaultUserLoader()
        {
            _users.Add(Environment.UserName, new User() { Id = Environment.UserName, IsOnline = true, LastChanged = DateTime.Now });
            _users.Add("UserOffline", new User() { Id = "UserOffline", IsOnline = false, LastChanged = DateTime.Now });
            _users.Add("UserOnline", new User() { Id = "UserOnline", IsOnline = true, LastChanged = DateTime.Now });
        }

        public bool IsMe(string userId)
        {
            return Environment.UserName == userId;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            var users = new List<User>();
            await Task.Delay(5000).ConfigureAwait(false);
            return users;
        }

        public async Task<int> OnlineUsersCount()
        {
            await Task.Delay(5000).ConfigureAwait(false);
            return 3;
        }

        public async Task<User> GetUserById(string userId)
        {
            await Task.Delay(100).ConfigureAwait(false);
            if (!_users.TryGetValue(userId, out var user))
                throw new KeyNotFoundException(userId);
            return user;
        }
    }
}
