using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChat.Services.Messaging.Users
{
    public class UserStatusData
    {
        public UserData UserData { get; }
        public bool IsOnline { get; }
        public DateTime ChangedDate { get; }
    }
}
