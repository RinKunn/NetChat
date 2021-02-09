using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Threading;
using NetChat.Desktop.Services.Messaging;
using NetChat.Desktop.ViewModel.InnerMessages;
using NetChat.Desktop.ViewModel.Messenger;

namespace NetChat.Desktop.ViewModel.Notifications
{
    public class NotificationsViewModel : ViewModelBase
    {
        private bool _isHiding = false;
        private const int MAX_QUEUE_COUNT = 3;
        private readonly TimeSpan TIMER_DELAY = TimeSpan.FromSeconds(5);
        private readonly IReceiverHub _receiverHub;
        private bool _notifyOnParticipantStatusChanged = false;

        public bool IsHideButtonVisible => Notifications.Count > 1;
        public ObservableCollection<NotificationItem> Notifications { get; }

#if DEBUG
        public NotificationsViewModel()
        {
            Notifications = new ObservableCollection<NotificationItem>();
            Notifications.Add(new NotificationItem("str1", "Title1", "Sender1", "Message1"));
            Notifications.Add(new NotificationItem("str2", "Title2", "Sender2", "Message2"));
            Notifications.Add(new NotificationItem("str3", "Title3", "Sender3", "Message3"));
        }
#endif

        public NotificationsViewModel(IReceiverHub receiverHub)
        {
            _receiverHub = receiverHub ?? throw new ArgumentNullException(nameof(receiverHub));
            Notifications = new ObservableCollection<NotificationItem>();
            Notifications.CollectionChanged += (o, e) => RaisePropertyChanged(nameof(IsHideButtonVisible));
            _receiverHub.SubscribeMessageReceived(this, OnMessageReceived);
            if(_notifyOnParticipantStatusChanged)
                _receiverHub.SubscribeUserStatusChanged(this, OnUserStatusChanged);
        }

        private void OnMessageReceived(MessageObservable message)
        {
            if(message is TextMessageObservable textMessage /*&& !textMessage.IsOriginNative*/)
            {
                AppendNotification(
                    new MessageNotificationItem(message.Id, textMessage.Sender.UserId, textMessage.Text));
            }
        }

        private void OnUserStatusChanged(ParticipantObservable participant)
        {
            AppendNotification(
                new ParticipantNotificationItem(participant.UserId, participant.IsOnline));
        }

        private void AppendNotification(NotificationItem notification)
        {
            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                if (Notifications.Count > MAX_QUEUE_COUNT)
                {
                    CloseNotification(Notifications[0]);
                }
                Notifications.Add(notification);
            });
        }

        private RelayCommand _hideAllCommand;
        public RelayCommand HideAllCommand => _hideAllCommand ??
            (_hideAllCommand = new RelayCommand(HideAll, HideAllEnable));
        private void HideAll()
        {
            if (Notifications.Count == 0) return;
            Console.WriteLine("Hide all: {0}", Notifications.Count);
            Notifications.Clear();
        }
        private bool HideAllEnable()
        {
            return Notifications.Count > 0;
        }

        private RelayCommand<NotificationItem> _closeNotificationCommand;
        public RelayCommand<NotificationItem> CloseNotificationCommand => _closeNotificationCommand ??
            (_closeNotificationCommand = new RelayCommand<NotificationItem>(CloseNotification));
        private void CloseNotification(NotificationItem notification)
        {
            Console.WriteLine("Closing notification {0}", notification.Message);
            Notifications.Remove(notification);
        }

        private RelayCommand _startClosingTimerCommand;
        public RelayCommand StartClosingTimerCommand => _startClosingTimerCommand ??
            (_startClosingTimerCommand = new RelayCommand(StartClosingTimer));
        private async void StartClosingTimer()
        {
            if (_isHiding) return;
            _isHiding = true;
            Console.WriteLine("Start closing after {0} sec.", TIMER_DELAY.TotalSeconds);
            await Task.Delay(TIMER_DELAY);
            HideAll();
            _isHiding = false;
        }

        private RelayCommand<NotificationItem> _gotoMessageCommand;
        public RelayCommand<NotificationItem> GotoMessageCommand => _gotoMessageCommand ??
            (_gotoMessageCommand = new RelayCommand<NotificationItem>(ShowMessage));

        private void ShowMessage(NotificationItem notification)
        {
            Console.WriteLine("Showing message {0}", notification.Message);
            MessengerInstance.Send<GoToMessageIMessage>(new GoToMessageIMessage(notification.MessageId));
        }
    }
}
