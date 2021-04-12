using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChat.Services.Messaging.Users.Storage
{
    public interface IUserDataRepository
    {
        Task<UserData> GetByUserName(string userName);
    }
}
