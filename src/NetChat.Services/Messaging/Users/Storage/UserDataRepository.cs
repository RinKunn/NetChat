using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetChat.Services.Caching;

namespace NetChat.Services.Messaging.Users.Storage
{
    public class UserDataRepository : IUserDataRepository
    {
        public Task<UserData> GetByUserName(string userName)
        {
            return Task.FromResult(new UserData(userName, userName));
        }
    }
}
