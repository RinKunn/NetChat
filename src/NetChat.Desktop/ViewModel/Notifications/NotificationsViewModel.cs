using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using NetChat.Desktop.InnerMessages;
using NetChat.Services.Messaging.Messages;
using NetChat.Services.Messaging.Users;
using NLog;

namespace NetChat.Desktop.ViewModel.Notifications
{
    public class NotificationsViewModel : ViewModelBase
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly System.Windows.Forms.Timer _timer;
        private int _showingMaxCount;
        private bool _enableParticipantNotification = false;
        private bool _enableMessagesNotification = false;
        private readonly IMessageUpdater _messageUpdater;
        private readonly IUserUpdater _userUpdater;
        private readonly IMessenger _innerMessageBus;

        public ObservableCollection<NotificationBase> Notifications { get; }

        public bool EnableParticipantNotification
        {
            get => _enableParticipantNotification;
            set
            {
                if (_enableParticipantNotification && !value)
                {
                    _logger.Trace("Unsubscribe from participant's notifications");
                    _userUpdater.Unregister(this);
                }
                else if (!_enableParticipantNotification && value)
                {
                    _logger.Trace("Subscribe to participant's notifications");
                    _userUpdater.Register(this, OnUserStatusChanged);
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
                    _messageUpdater.Unregister(this);
                }    
                else if (!_enableMessagesNotification && value)
                {
                    _logger.Trace("Subscribe to message's notifications");
                    _messageUpdater.Register(this, OnMessageReceived);
                }
                Set(ref _enableMessagesNotification, value);
            }
        }

#if DEBUG
        public NotificationsViewModel()
        {
            Notifications = new ObservableCollection<NotificationBase>
            {
                new MessageNotification("str1", "Sender1", "Message1"),
                new MessageNotification("str2", "Sender2", "Message2"),
                new MessageNotification("str3", "Sender3", "Message3")
            };
        }
#endif

        public NotificationsViewModel(IMessageUpdater messageUpdater,
            IUserUpdater userUpdater,
            IMessenger innerMessageBus, 
            NotificationConfiguration configuration)
        {
            _logger.Debug("Initing notification model...");
            _messageUpdater = messageUpdater ?? throw new ArgumentNullException(nameof(messageUpdater));
            _userUpdater = userUpdater ?? throw new ArgumentNullException(nameof(userUpdater));
            _innerMessageBus = innerMessageBus ?? throw new ArgumentNullException(nameof(innerMessageBus));
            _timer = new System.Windows.Forms.Timer();
            _timer.Tick += Timer_Tick;
            Notifications = new ObservableCollection<NotificationBase>();
            Notifications.CollectionChanged += Notifications_CollectionChanged;
            _innerMessageBus.Register<NotificationEnablingIM>(this, (m) =>
            {
                DispatcherHelper.CheckBeginInvokeOnUI(() => EnableMessagesNotification = m.Enable);
            });
            ApplyConfiguration(configuration);
            _logger.Debug("Notification model inited!");
        }

        private void Notifications_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add
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
            _logger.Debug("Notification's model cleaning...");
            _innerMessageBus.Unregister<NotificationEnablingIM>(this);
            if (_enableMessagesNotification) _messageUpdater.Unregister(this);
            if(_enableParticipantNotification) _userUpdater.Unregister(this);
            base.Cleanup();
        }



        private void OnMessageReceived(Message message)
        {
#if !DEBUG
            if (message.MessageData.IsOutgoing) return;
#endif
            string notificationText =
                message.MessageData.Content is MessageTextContent textContent 
                ? textContent.Text 
                : "Новое сообщение";

            AppendNotification(
                    new MessageNotification(message.MessageData.Id,
                        message.UserData.UserName,
                        notificationText));
        }

        private void OnUserStatusChanged(UserStatusData userStatus)
        {
            AppendNotification(
                new ParticipantNotification(
                    userStatus.UserData.UserName,
                    userStatus.IsOnline));
        }

        private void AppendNotification(NotificationBase notification)
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

        private RelayCommand<NotificationBase> _closeNotificationCommand;
        public RelayCommand<NotificationBase> CloseNotificationCommand => _closeNotificationCommand ??
            (_closeNotificationCommand = new RelayCommand<NotificationBase>(CloseNotification));
        private void CloseNotification(NotificationBase notification)
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

        private RelayCommand<NotificationBase> _gotoMessageCommand;
        public RelayCommand<NotificationBase> GotoMessageCommand => _gotoMessageCommand ??
            (_gotoMessageCommand = new RelayCommand<NotificationBase>(ShowMessage));
        private void ShowMessage(NotificationBase notification)
        {
            _logger.Trace("Go to message clicked: MessageId='{0}'", notification.MessageId);
            _innerMessageBus.Send(new GoToMessageIM(notification.MessageId));
            CloseNotification(notification);
        }
    }
}
