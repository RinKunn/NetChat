using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChat.Services.Messaging.Users
{
    public class UserStatus
    {
        public UserStatus(string userName, bool isOnline, DateTime updateDateTime)
        {
            UserName = userName;
            IsOnline = isOnline;
            UpdateDateTime = updateDateTime;
        }

        public string UserName { get; }
        public bool IsOnline { get; }
        public DateTime UpdateDateTime { get; }
    }
}
