using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChat.Desktop
{
    public class UserContext
    {
        public string CurrentUserName { get; }
        
        public UserContext(string name)
        {
            CurrentUserName = name;
        }
    }
}
