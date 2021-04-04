using System;

namespace NetChat.Desktop.InnerMessages
{
    public class ExceptionIM : IInnerMessage
    {
        public Exception Exception { get; }
        public bool IsCritical { get; }

        public ExceptionIM(Exception exception, bool isCritical)
        {
            Exception = exception;
            IsCritical = isCritical;
        }
    }
}
