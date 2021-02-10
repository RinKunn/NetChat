using System;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using NetChat.Desktop.Services.Messaging;
using NetChat.Desktop.ViewModel.InnerMessages;
using NetChat.Desktop.ViewModel.Messenger;
using NLog;

namespace NetChat.Desktop.ViewModel.Notifications
{
    public class NotificationsViewModel : ViewModelBase
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly Timer _timer;
        private int _showingMaxCount;
        private bool _enableParticipantNotification = false;
        private bool _enableMessagesNotification = false;
        private readonly IReceiverHub _receiverHub;
        private readonly IMessenger _innerCommunication;

        public ObservableCollection<NotificationItem> Notifications { get; }

        public bool EnableParticipantNotification
        {
            get => _enableParticipantNotification;
            set
            {
                if (_enableParticipantNotification && !value)
                {
                    _logger.Trace("Unsubscribe from participant's notifications");
                    _receiverHub.UnsubscribeUserStatusChanged(this);
                }
                else if (!_enableParticipantNotification && value)
                {
                    _logger.Trace("Subscribe to participant's notifications");
                    _receiverHub.SubscribeUserStatusChanged(this, OnUserStatusChanged);
                }
                Set(ref _enableParticipantNotification, value);
            }
        }

        public bool EnableMessagesNotification
        {
            get => _enableMessagesNotification;
            set
            {
                if (_enableMessagesNotification && !value)
                {
                    _logger.Trace("Unsubscribe from message's notifications");
                    _receiverHub.UnsubscribeMessageReceived(this);
                }    
                else if (!_enableMessagesNotification && value)
                {
                    _logger.Trace("Subscribe to message's notifications");
                    _receiverHub.SubscribeMessageReceived(this, OnMessageReceived);
                }
                Set(ref _enableMessagesNotification, value);
            }
        }

#if DEBUG
        public NotificationsViewModel()
        {
            Notifications = new ObservableCollection<NotificationItem>
            {
                new MessageNotificationItem("str1", "Sender1", "Message1"),
                new MessageNotificationItem("str2", "Sender2", "Message2"),
                new MessageNotificationItem("str3", "Sender3", "Message3")
            };
        }
#endif

        public NotificationsViewModel(IReceiverHub receiverHub, IMessenger messenger, NotificationConfiguration configuration)
        {
            _logger.Debug("Initing notification model...");
            _receiverHub = receiverHub ?? throw new ArgumentNullException(nameof(receiverHub));
            _innerCommunication = messenger ?? throw new ArgumentNullException(nameof(messenger));
            _timer = new Timer();
            _timer.Tick += Timer_Tick;
            Notifications = new ObservableCollection<NotificationItem>();
            Notifications.CollectionChanged += Notifications_CollectionChanged;
            _innerCommunication.Register<NotificationEnablingInnerMessage>(this, (m) =>
            {
                DispatcherHelper.CheckBeginInvokeOnUI(() => EnableMessagesNotification = m.Enable);
            });
            ApplyConfiguration(configuration);
            _logger.Debug("Notification model inited!");
        }

        private void Notifications_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add
                && _timer.Enabled)
                _timer.Stop();
        }

        private void ApplyConfiguration(NotificationConfiguration configuration)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            EnableMessagesNotification = configuration.EnableMessageNotifications;
            EnableParticipantNotification = configuration.EnableParticipantNotifications;
            _showingMaxCount = configuration.ShowingMaxCount;
            _timer.Interval = (int)configuration.HideTimeout.TotalMilliseconds;

            _logger.Debug("Notification's configuration:");
            _logger.Debug("Hide after: {0} sec", configuration.HideTimeout.TotalSeconds);
            _logger.Debug("MessNotify enabled: {0}, ParticipNotify enabled: {1}",
                configuration.EnableMessageNotifications, configuration.EnableParticipantNotifications);
            _logger.Debug("Only {0} messages can be displayed at same time", configuration.ShowingMaxCount);
        }


        public override void Cleanup()
        {
            _logger.Debug("Notification's view cleaning...");
            _innerCommunication.Unregister<NotificationEnablingInnerMessage>(this);
            if(_enableMessagesNotification) _receiverHub.UnsubscribeMessageReceived(this);
            if(_enableParticipantNotification) _receiverHub.UnsubscribeUserStatusChanged(this);
            base.Cleanup();
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
                if (Notifications.Count > _showingMaxCount)
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
            Notifications.Clear();
            _logger.Trace("Notification of {0} are hidden", Notifications.Count);
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
            Notifications.Remove(notification);
            _logger.Trace("Notification with MessageId='{0}' id hidden by User", notification.MessageId);
        }

        private RelayCommand _startClosingTimerCommand;
        public RelayCommand StartClosingTimerCommand => _startClosingTimerCommand ??
            (_startClosingTimerCommand = new RelayCommand(StartClosingTimer));
        private void StartClosingTimer()
        {
            if (_timer.Enabled) return;
            _timer.Start();
            _logger.Trace("Hide timer is started");
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            HideAll();
            _timer.Stop();
        }

        private RelayCommand _stopClosingTimerCommand;
        public RelayCommand StopClosingTimerCommand => _stopClosingTimerCommand ??
            (_stopClosingTimerCommand = new RelayCommand(StopClosingTimer));
        private void StopClosingTimer()
        {
            if (_timer.Enabled)
            {
                _timer.Stop();
                _logger.Trace("Hide timer is stopped");
            }
        }

        private RelayCommand<NotificationItem> _gotoMessageCommand;
        public RelayCommand<NotificationItem> GotoMessageCommand => _gotoMessageCommand ??
            (_gotoMessageCommand = new RelayCommand<NotificationItem>(ShowMessage));
        private void ShowMessage(NotificationItem notification)
        {
            _logger.Trace("Go to message clicked: MessageId='{0}'", notification.MessageId);
            _innerCommunication.Send(new GoToMessageInnerMessage(notification.MessageId));
            CloseNotification(notification);
        }
    }
}
