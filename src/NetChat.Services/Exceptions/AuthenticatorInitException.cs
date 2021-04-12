using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChat.Services.Authentication.Exceptions
{
    public class AuthenticatorInitException : Exception
    {
        public AuthenticatorInitException(string userName, Exception innerException)
            : base($"Произошла ошибка при инициализации Аутентификатора", innerException) { }
    }
}
