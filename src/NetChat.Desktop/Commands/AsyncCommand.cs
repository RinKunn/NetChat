using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NetChat.Desktop.Commands
{
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
            return !Execution.IsNotCompleted && _canExecute(parameter);
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

        public NotifyTaskCompletion Execution
        {
            get => _execution;
            set
            {
                _execution = value;
                if (PropertyChanged == null) return;
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
            return !Execution.IsNotCompleted && _canExecute(parameter);
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
                if (PropertyChanged == null) return;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Execution)));
            }
        }
    }
}
