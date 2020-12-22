using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChat.Desktop.Services.Messaging.Users
{
    public class User
    {
        public string Id { get; set; }
        public UserStatus Status { get; set; }
        public DateTime StatusChangedDateTime { get; set; }
    }

    public enum UserStatus
    {
        Offline = 0,
        Online = 1
    }
}
