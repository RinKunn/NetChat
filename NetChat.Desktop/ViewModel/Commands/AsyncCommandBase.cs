using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NetChat.Desktop.ViewModel.Commands
{
    public interface IAsyncCommand : ICommand
    {
        Task ExecuteAsync(object parameter);
        void Refresh();
    }
    public class NotifyTaskCompletion : INotifyPropertyChanged
    {
        public NotifyTaskCompletion(Task task)
        {
            Task = task;
            if (!task.IsCompleted)
                TaskCompletion = WatchTaskAsync(task);
        }

        public Task TaskCompletion { get; private set; }
        private async Task WatchTaskAsync(Task task)
        {
            try
            {
                await task;
            }
            catch { }
            
            var propertyChanged = PropertyChanged;
            if (propertyChanged == null)
                return;
            propertyChanged(this, new PropertyChangedEventArgs("Status"));
            propertyChanged(this, new PropertyChangedEventArgs("IsCompleted"));
            propertyChanged(this, new PropertyChangedEventArgs("IsNotCompleted"));
            if (task.IsCanceled)
            {
                propertyChanged(this, new PropertyChangedEventArgs("IsCanceled"));
            }
            else if (task.IsFaulted)
            {
                propertyChanged(this, new PropertyChangedEventArgs("IsFaulted"));
                propertyChanged(this, new PropertyChangedEventArgs("Exception"));
                propertyChanged(this, new PropertyChangedEventArgs("InnerException"));
                propertyChanged(this, new PropertyChangedEventArgs("ErrorMessage"));
                task.han
            }
            else
            {
                propertyChanged(this, new PropertyChangedEventArgs("IsSuccessfullyCompleted"));
                propertyChanged(this, new PropertyChangedEventArgs("Result"));
            }
        }

        public Task Task { get; private set; }
        public TaskStatus Status
            => Task.Status;
        public bool IsCompleted
            => Task.IsCompleted;
        public bool IsNotCompleted
            => !Task.IsCompleted;
        public bool IsSuccessfullyCompleted
        {
            get
            {
                return Task.Status == TaskStatus.RanToCompletion;
            }
        }
        public bool IsCanceled => Task.IsCanceled;
        public bool IsFaulted => Task.IsFaulted;
        public AggregateException Exception
            => Task.Exception;
        public Exception InnerException
        {
            get
            {
                return (Exception == null) ? null : Exception.InnerException;
            }
        }
        public string ErrorMessage
        {
            get
            {
                return (InnerException == null) ? null : InnerException.Message;
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }

    public sealed class NotifyTaskCompletion<TResult> : NotifyTaskCompletion
    {
        public NotifyTaskCompletion(Task<TResult> task) : base(task) { }

        public TResult Result
        {
            get
            {
                return (Task.Status == TaskStatus.RanToCompletion) ? ((Task<TResult>)Task).Result : default(TResult);
            }
        }
    }

    public abstract class AsyncCommandBase : IAsyncCommand
    {
        public abstract bool CanExecute(object parameter);
        public abstract Task ExecuteAsync(object parameter);
        public async void Execute(object parameter)
        {
            await ExecuteAsync(parameter);
        }
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        protected void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
        public abstract void Refresh();
    }

    public class AsyncCommand : AsyncCommandBase, INotifyPropertyChanged
    {
        private readonly Func<Task> _command;
        private readonly Predicate<object> _canExecute;
        public NotifyTaskCompletion _execution;

        public event PropertyChangedEventHandler PropertyChanged;

        public AsyncCommand(Func<Task> command) : this(command, null) { }
        public AsyncCommand(Func<Task> command, Predicate<object> canExecute)
        {
            _command = command;
            _canExecute = canExecute;
        }

        public override bool CanExecute(object parameter)
        {
            if (Execution == null || !Execution.IsNotCompleted && _canExecute == null) return true;
            return (!Execution.IsNotCompleted && _canExecute(parameter));
        }
        public override Task ExecuteAsync(object parameter)
        {
            Execution = new NotifyTaskCompletion(_command());
            return Execution.TaskCompletion;
        }
        public override void Refresh()
        {
            Execution = null;
        }

        // Raises PropertyChanged
        
        public NotifyTaskCompletion Execution
        {
            get => _execution;
            set
            {
                _execution = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Execution)));
            }
        }
    }

    public class AsyncCommand<TResult> : AsyncCommandBase, INotifyPropertyChanged
    {
        private readonly Func<Task<TResult>> _command;
        private readonly Predicate<object> _canExecute;
        private NotifyTaskCompletion<TResult> _execution;

        public event PropertyChangedEventHandler PropertyChanged;

        public AsyncCommand(Func<Task<TResult>> command) : this(command, null) { }
        public AsyncCommand(Func<Task<TResult>> command, Predicate<object> canExecute)
        {
            _command = command;
            _canExecute = canExecute;
        }

        public override bool CanExecute(object parameter)
        {
            if (!Execution.IsNotCompleted && _canExecute == null) return true;
            return (!Execution.IsNotCompleted && _canExecute(parameter));
        }
        public override Task ExecuteAsync(object parameter)
        {
            Execution = new NotifyTaskCompletion<TResult>(_command());
            return Execution.TaskCompletion;
        }
        public override void Refresh()
        {
            Execution = null;
        }

        // Raises PropertyChanged
        public NotifyTaskCompletion<TResult> Execution
        {
            get => _execution;
            set
            {
                _execution = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Execution)));
            }
        }
    }
}
