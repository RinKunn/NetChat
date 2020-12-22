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
}
