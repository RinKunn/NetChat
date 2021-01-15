using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChat.FileMessaging.Repository.UserStatuses
{
    public class UserStatus
    {
        public string UserId { get; set; }
        public DateTime UpdateDateTime { get; set; }
        public bool IsOnline { get; set; }
    }
}
