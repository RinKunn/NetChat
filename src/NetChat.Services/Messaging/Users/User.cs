using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetChat.Services.Messaging.Messages;

namespace NetChat.Services.Messaging.Users
{
    public class User
    {
        public User(UserData userData)
        {
            UserData = userData;
        }

        public UserData UserData { get; }
    }
}
