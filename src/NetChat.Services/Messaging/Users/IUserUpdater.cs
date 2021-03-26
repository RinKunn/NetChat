using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetChat.Services.Messaging.Common;

namespace NetChat.Services.Messaging.Users
{
    public interface IUserUpdater 
        : IUpdaterService<UserStatusData>
    {

    }
}
