using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChat.Services.Messaging.Users
{
    public class UserData
    {
        public UserData(string id, string userName)
        {
            Id = id;
            UserName = userName;
        }

        public string Id { get; }
        public string UserName { get; }
    }
}
