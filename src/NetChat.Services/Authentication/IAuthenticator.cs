using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChat.Services.Authentication
{
    public interface IAuthenticator
    {
        Task SetupParameters();
        Task Logon();
        Task Logout();
    }
}
