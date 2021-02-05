using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using NetChat.Desktop.Services.Messaging;
using NetChat.Desktop.ViewModel.Messenger;

namespace NetChat.Desktop.ViewModel.Notifications
{
    public class NotificationViewModel : ViewModelBase
    {
        private const int MAX_QUEUE_COUNT = 3;
        private readonly IReceiverHub _receiverHub;

        public ObservableCollection<NotificationBase> Notifications;

        public NotificationViewModel(IReceiverHub receiverHub)
        {
            _receiverHub = receiverHub ?? throw new ArgumentNullException(nameof(receiverHub));
            Notifications = new ObservableCollection<NotificationBase>();
            Notifications.CollectionChanged += Notifications_CollectionChanged;
            _receiverHub.SubscribeMessageReceived(this, OnMessageReceived);
            _receiverHub.SubscribeUserStatusChanged(this, OnUserStatusChanged);
        }

        private void Notifications_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if(e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                if (Notifications.Count > MAX_QUEUE_COUNT)
                    Notifications.RemoveAt(0);
            }
        }

        private void OnMessageReceived(MessageObservable message)
        {
            if(message is TextMessageObservable textMessage && !textMessage.IsOriginNative)
            {
                Notifications.Add(
                    new MessageNotification(textMessage.Sender.UserId, textMessage.Text));
            }
        }

        private void OnUserStatusChanged(ParticipantObservable participant)
        {
            Notifications.Add(
                new ParticipantNotification(participant.UserId, participant.IsOnline));
        }


        private RelayCommand _hideAllCommand;
        public RelayCommand HideAllCommand => _hideAllCommand ??
            (_hideAllCommand = new RelayCommand(null));

        private RelayCommand _closeNotificationCommand;
        public RelayCommand CloseNotificationCommand => _closeNotificationCommand ??
            (_closeNotificationCommand = new RelayCommand(null));

        private RelayCommand _startClosingTimerCommand;
        public RelayCommand StartClosingTimerCommand => _startClosingTimerCommand ??
            (_startClosingTimerCommand = new RelayCommand(null));

        private RelayCommand _gotoMessageCommand;
        public RelayCommand GotoMessageCommand => _gotoMessageCommand ??
            (_gotoMessageCommand = new RelayCommand(null));

        private void HideAllNotifications()
        {
            Notifications.Clear();
        }
    }
}
