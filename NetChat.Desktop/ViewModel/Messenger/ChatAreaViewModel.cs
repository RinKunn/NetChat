using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using NetChat.Desktop.Services.Messaging.Messages;

namespace NetChat.Desktop.ViewModel.Messenger
{
    public class ChatAreaViewModel : ViewModelBase
    {
        private IMessageLoader _messageLoader;

        private ObservableCollection<MessageTextObservable> _messages;
        public ObservableCollection<MessageTextObservable> Messages
        {
            get => _messages;
            private set => Set(ref _messages, value);
        }

        public ChatAreaViewModel()
        {
            if(IsInDesignModeStatic)
            {
                Messages = new ObservableCollection<MessageTextObservable>();
                Messages.Add(new MessageTextObservable("Hello, cols", "1", DateTime.Now.AddMinutes(-3), new ParticipantObservable("User1", true, DateTime.Now.AddHours(-1))));
                Messages.Add(new MessageTextObservable("Hello, User1", "2", DateTime.Now.AddMinutes(-2), new ParticipantObservable("User2", true, DateTime.Now.AddHours(-2)), true));
                Messages.Add(new MessageTextObservable("Hello, User1 and User2, asdsadasdddddd dddddddddddddd ddddddddddddd ddddd", "3", DateTime.Now.AddMinutes(-1), new ParticipantObservable("User3", true, DateTime.Now.AddHours(-3))));
            }
        }

        public ChatAreaViewModel(IMessageLoader messageLoader)
        {
            _messageLoader = messageLoader;
        }
    }
}
