using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChat.Desktop.Repository
{
    public interface IMessageReceiver : IDisposable
    {
        event OnMessageReceivedHandler OnMessageReceived;
        event OnUserLoggedInHandler OnUserLoggedIn;
        event OnUserLoggedOutHandler OnUserLoggedOut;
    }
}
