using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using NetChat.Desktop.ViewModel.Messenger;
using NetChat.FileMessaging.Models;
using NetChat.FileMessaging.Services.Users;
using NLog;

namespace NetChat.Desktop.Services.Messaging.Users
{
    public class UserLoader : IUserLoader
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly IUserService _userService;

#if DEBUG
        private static string _userName;
#endif

        public UserLoader(IUserService userService)
        {
            _userService = userService;
        }

        public bool IsMe(string userId)
        {
#if DEBUG
            if(string.IsNullOrEmpty(_userName))
            {
                _userName = Environment.UserName.ToUpper();
                string currentProcess = Process.GetCurrentProcess().ProcessName;
                int runnedProc = Process.GetProcessesByName(currentProcess).Length;
                if (runnedProc > 1)
                    _userName += runnedProc.ToString();
            }
            return _userName == userId;
#else
            return _userService.IsMe(userId);
#endif
        }

        public async Task<ParticipantObservable> GetUserById(string userId)
        {
            var res = await _userService.GetUserById(userId);
            if (res == null)
            {
                _logger.Warn("User with id='{0}' dont exists", userId);
                return null;
            }
            return ToObservable(res);
        }

        public async Task<IList<ParticipantObservable>> GetUsers()
        {
            var res = await _userService.GetUsers();
            if (res == null)
            {
                _logger.Warn("Users not exists");
                return null;
            }
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
