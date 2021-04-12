using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChat.Services.Exceptions
{
    public class CriticalException : Exception
    {
        public string Code { get; }
        public CriticalException(string code, string message, Exception innerException)
            : base(message, innerException)
        {
            Code = "CR_" + code;
        }
    }
}
