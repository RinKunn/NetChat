using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NetChat.Desktop.ViewModel.Commands
{
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
            if (propertyChanged == null) return;
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
            }
            else
            {
                propertyChanged(this, new PropertyChangedEventArgs("IsSuccessfullyCompleted"));
                propertyChanged(this, new PropertyChangedEventArgs("Result"));
            }
        }

        public Task Task { get; private set; }
        public TaskStatus Status => Task.Status;
        public bool IsCompleted => Task.IsCompleted;
        public bool IsNotCompleted => !Task.IsCompleted;
        public bool IsSuccessfullyCompleted => Task.Status == TaskStatus.RanToCompletion;
        public bool IsCanceled => Task.IsCanceled;
        public bool IsFaulted => Task.IsFaulted;
        public AggregateException Exception => Task.Exception;
        public Exception InnerException =>
            (Exception == null) ? null : Exception.InnerException;
        public string ErrorMessage
            => (InnerException == null) ? null : InnerException.Message;

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
}
