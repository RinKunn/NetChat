using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetChat.Desktop.ViewModel.Notifications;

namespace NetChat.Desktop
{
    public class UserContext
    {
        public string CurrentUserName { get; set; }

        public UserContext(string userName)
        {
            CurrentUserName = userName;
        }
    }
}
