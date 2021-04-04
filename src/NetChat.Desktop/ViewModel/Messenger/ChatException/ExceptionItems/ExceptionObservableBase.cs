using System;
using GalaSoft.MvvmLight;

namespace NetChat.Desktop.ViewModel.Messenger.ChatException.ExceptionItems
{
    public abstract class ExceptionObservableBase : ObservableObject
    {
        private Exception _exception;

        public bool IsCritical { get; }
        public string ErrorMessage => _exception.Message;
        public string InnerErrorMessage => _exception.InnerException?.Message;
        public string Source => _exception.Source;
        public string StackTrace => _exception.StackTrace;

#if DEBUG
        public ExceptionObservableBase()
        {
            _exception = new Exception("Exception example message",
                new Exception("Exception inner message example"));
            IsCritical = false;
        }
#endif

        public ExceptionObservableBase(Exception exception, bool isCritical)
        {
            _exception = exception ?? throw new ArgumentNullException(nameof(exception));
            IsCritical = isCritical;
        }
    }
}
