using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChat.Services.Messaging.Users
{
    public interface IUserLoader
    {
        Task<int> GetOnlineUsersCount();
        Task<UserData> GetMeAsync();
    }
}
