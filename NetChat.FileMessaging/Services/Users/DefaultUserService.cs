using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetChat.FileMessaging.Models;
using NetChat.FileMessaging.Repository.UserStatuses;

namespace NetChat.FileMessaging.Services.Users
{
    public class DefaultUserService : IUserService
    {
        private readonly IUserStatusRepository _userStatusRepository;

        public DefaultUserService(IUserStatusRepository userStatusRepository)
        {
            _userStatusRepository = userStatusRepository;
        }

        public async Task<User> GetUserById(string userId)
        {
            var res = await _userStatusRepository.GetUsersStatuses();
            var userStatus = res.FirstOrDefault(u => u.UserId == userId);

            return new User()
            {
                Id = userStatus.UserId,
                IsOnline = userStatus.IsOnline,
                LastChanged = userStatus.UpdateDateTime
            };
        }

        public bool IsMe(string userId)
        {
            return Environment.UserName == userId;
        }

        public async Task<IList<User>> GetUsers()
        {
            var res = await _userStatusRepository.GetUsersStatuses();
            return res
                .Select(us => new User()
                {
                    Id = us.UserId,
                    IsOnline = us.IsOnline,
                    LastChanged = us.UpdateDateTime
                })
                .ToArray();

        }

        public async Task<int> OnlineUsersCount()
        {
            var res = await _userStatusRepository.GetUsersStatuses();
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
