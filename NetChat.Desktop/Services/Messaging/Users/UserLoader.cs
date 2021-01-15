using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetChat.Desktop.ViewModel.Messenger;
using NetChat.FileMessaging.Models;
using NetChat.FileMessaging.Services.Users;

namespace NetChat.Desktop.Services.Messaging.Users
{
    public class UserLoader : IUserLoader
    {
        private readonly IUserService _userService;
        
        public UserLoader(IUserService userService)
        {
            _userService = userService;
        }


        public bool IsMe(string userId)
        {
            return _userService.IsMe(userId);
        }

        public async Task<ParticipantObservable> GetUserById(string userId)
        {
            var res = await _userService.GetUserById(userId);
            return ToObservable(res);
        }

        public async Task<IList<ParticipantObservable>> GetUsers()
        {
            var res = await _userService.GetUsers();
            return res.Select(u => ToObservable(u)).ToArray();
        }

        public async Task<int> OnlineUsersCount()
        {
            return await _userService.OnlineUsersCount();
        }

        
        private ParticipantObservable ToObservable(User user)
        {
            return new ParticipantObservable(user.Id, user.IsOnline, user.LastChanged);
        }
    }
}
