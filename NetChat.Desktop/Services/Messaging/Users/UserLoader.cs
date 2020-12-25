using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetChat.Desktop.Repository;

namespace NetChat.Desktop.Services.Messaging.Users
{
    public class UserLoader : IUserLoader
    {
        private readonly IUserRepository _userRepository;
        private ConcurrentDictionary<string, User> _usersCache = new ConcurrentDictionary<string, User>();
        
        public UserLoader(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> GetUserById(string userId)
        {
            await LoadUsers();
            if (!_usersCache.TryGetValue(userId, out var user))
                throw new KeyNotFoundException(userId);
            return user;
        }

        public bool IsMe(string userId)
        {
            return Environment.UserName == userId;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            await LoadUsers();
            return _usersCache.Values;
        }

        public async Task<int> OnlineUsersCount()
        {
            await LoadUsers();
            return _usersCache.Count;
        }



        private async Task LoadUsers()
        {
            if (_usersCache.Count > 0) return;
            var userData = await _userRepository.GetUsers();
            userData.Select(ud => _usersCache.GetOrAdd(ud.UserId, UserDataToUser(ud)));
        }

        private User UserDataToUser(UserData userData)
        {
            return new User()
            {
                Id = userData.UserId,
                IsOnline = userData.IsOnline,
                LastChanged = userData.LastChanged
            };
        }
    }
}
