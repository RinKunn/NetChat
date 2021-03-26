using System;

namespace NetChat.Desktop.InnerMessages
{
    public class ExceptionIM : IInnerMessage
    {
        public Exception Exception { get; }

        public ExceptionIM(Exception exception)
        {
            Exception = exception;
        }
    }
}
