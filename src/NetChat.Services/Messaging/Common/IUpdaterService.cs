using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChat.Services.Messaging.Common
{
    public interface IUpdaterService<T> where T : class
    {
        void Register(object obj, Action<T> action);
        void Unregister(object obj);
    }
}
