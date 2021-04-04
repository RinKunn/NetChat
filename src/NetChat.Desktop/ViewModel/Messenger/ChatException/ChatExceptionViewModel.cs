using System;
using System.Collections.Generic;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using NetChat.Desktop.InnerMessages;
using NetChat.Desktop.ViewModel.Messenger.ChatException.ExceptionItems;
using NLog;

namespace NetChat.Desktop.ViewModel.Messenger.ChatException
{
    public class ChatExceptionViewModel : ViewModelBase
    {
        private bool _isActive;
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly IMessenger _innerMessageBus;
        private Queue<ExceptionObservableBase> _exceptionsQueue = new Queue<ExceptionObservableBase>();
        private ExceptionObservableBase _currentException;
        private object _locker = new object();

        public ExceptionObservableBase CurrentException
        {
            get => _currentException;
            set => Set(ref _currentException, value);
        }
        public bool IsActive
        {
            get => _isActive;
            set => Set(ref _isActive, value);
        }

#if DEBUG
        public ChatExceptionViewModel()
        {
            if(IsInDesignMode)
            {
                CurrentException = new DefaultExceptionObservable(
                    new Exception("Error accured",
                        new Exception("Inner exception error message")),
                    true);
                IsActive = true;
            }
        }
        public ChatExceptionViewModel(bool isActive)
        {
            if (IsInDesignMode)
            {
                if (isActive)
                {
                    CurrentException = new DefaultExceptionObservable(
                    new Exception("Error accured",
                        new Exception("Inner exception error message")),
                    true);
                    IsActive = true;
                }
                else
                    IsActive = false;
            }
        }
#endif

        public ChatExceptionViewModel(IMessenger innerMessageBus)
        {
            _innerMessageBus = innerMessageBus ?? throw new ArgumentNullException(nameof(innerMessageBus));
            _innerMessageBus.Register<ExceptionIM>(this, OnExceptionReceive);
            IsActive = false;
        }

        public override void Cleanup()
        {
            _innerMessageBus.Unregister(this);
            base.Cleanup();
        }

        private void OnExceptionReceive(ExceptionIM innerMessage)
        {
            if (innerMessage == null) return;
            lock (_locker)
            {
                IsActive = true;
                var exc = new DefaultExceptionObservable(innerMessage.Exception, innerMessage.IsCritical);
                if (CurrentException == null)
                {
                    CurrentException = exc;
                }
                else
                {
                    _exceptionsQueue.Enqueue(exc);
                }
            }
        }

        private RelayCommand _closeExceptionCommand;
        public RelayCommand CloseExceptionCommand => _closeExceptionCommand ??
            (_closeExceptionCommand = new RelayCommand(CloseExceptionHandler));
        private void CloseExceptionHandler()
        {
            lock (_locker)
            {
                if (_exceptionsQueue.Count > 0)
                    CurrentException = _exceptionsQueue.Dequeue();
                else
                {
                    CurrentException = null;
                    IsActive = false;
                }
            }
        }
    }
}
