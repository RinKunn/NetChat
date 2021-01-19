using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChat.Desktop
{
    public class NetChatContext
    {
        public string CurrentUserName { get; }

        public NetChatContext(string name)
        {
            CurrentUserName = name;
        }
    }
}
