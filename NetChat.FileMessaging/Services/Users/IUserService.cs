using System.Collections.Generic;
using System.Threading.Tasks;
using NetChat.FileMessaging.Models;

namespace NetChat.FileMessaging.Services.Users
{
    public interface IUserService
    {
        Task<IList<UserData>> GetUsers();
        Task<UserData> GetUserById(string userId);
        bool IsMe(string userId);
        Task<int> OnlineUsersCount();
        Task Logon(string userId);
        Task Logout(string userId);
    }
}
