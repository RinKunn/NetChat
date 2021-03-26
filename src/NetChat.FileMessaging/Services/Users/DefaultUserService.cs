using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetChat.FileMessaging.Models;
using NetChat.FileMessaging.Repository.UserStatuses;
using NLog;

namespace NetChat.FileMessaging.Services.Users
{
    public class DefaultUserService : IFileUserService
    {
        private readonly IUserStatusRepository _userStatusRepository;

        public DefaultUserService(IUserStatusRepository userStatusRepository)
        {
            _userStatusRepository = userStatusRepository;
        }

        public async Task<UserData> GetUserById(string userId)
        {
            var res = await _userStatusRepository.GetUsersStatuses();
            var userStatus = res.FirstOrDefault(u => u.UserId == userId);
            if (userStatus == null) return null;
            return new UserData()
            {
                Id = userStatus.UserId,
                IsOnline = userStatus.IsOnline,
                LastChanged = userStatus.UpdateDateTime
            };
        }

        public bool IsMe(string userId)
        {
            return Environment.UserName.ToUpper() == userId;
        }

        public async Task<IList<UserData>> GetUsers()
        {
            var res = await _userStatusRepository.GetUsersStatuses();
            if (res == null || res.Count == 0) return null;
            return res
                .Select(us => new UserData()
                {
                    Id = us.UserId,
                    IsOnline = us.IsOnline,
                    LastChanged = us.UpdateDateTime
                })
                .ToList();
        }

        public async Task<int> OnlineUsersCount()
        {
            var res = await _userStatusRepository.GetUsersStatuses();
            if (res == null || res.Count == 0) return 0;
            return res.Count(u => u.IsOnline);
        }

        public async Task Logon(string userId)
        {
            await _userStatusRepository.SetUserStatus(
                new UserStatus()
                {
                    UserId = userId,
                    UpdateDateTime = DateTime.Now,
                    IsOnline = true
                });
        }

        public async Task Logout(string userId)
        {
            await _userStatusRepository.SetUserStatus(
                new UserStatus()
                {
                    UserId = userId,
                    UpdateDateTime = DateTime.Now,
                    IsOnline = false
                });
        }
    }
}
