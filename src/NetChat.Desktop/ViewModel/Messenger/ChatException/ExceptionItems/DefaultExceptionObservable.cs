using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChat.Desktop.ViewModel.Messenger.ChatException.ExceptionItems
{
    public class DefaultExceptionObservable : ExceptionObservableBase
    {
#if DEBUG
        public DefaultExceptionObservable() : base() { }
#endif

        public DefaultExceptionObservable(Exception exception, bool isCritical)
            : base(exception, isCritical) { }
    }
}
