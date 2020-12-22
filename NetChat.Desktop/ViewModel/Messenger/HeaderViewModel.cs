using GalaSoft.MvvmLight;
using NetChat.Desktop.Services.Messaging.Users;
using NetChat.Desktop.ViewModel.InnerMessages;
using Locator = CommonServiceLocator.ServiceLocator;

namespace NetChat.Desktop.ViewModel.Messenger
{
    public class HeaderViewModel : ViewModelBase
    {
        private IUserLoader _userLoader;

        private string _title;
        private int _participantOnlineCount;
        private int _unreadMessagesCount;

        public string Title => _title;
        public int ParticipantOnlineCount
        {
            get => _participantOnlineCount;
            set => Set(ref _participantOnlineCount, value);
        }
        public int UnreadMessagesCount
        {
            get => _unreadMessagesCount;
            set => Set(ref _unreadMessagesCount, value);
        }

        public HeaderViewModel()
        {
            if (IsInDesignModeStatic)
            {
                _title = "Hello world";
                ParticipantOnlineCount = 189;
            }
        }

        public HeaderViewModel(string title) : this(
            title,
            Locator.Current.GetService<IUserLoader>())
        { }

        public HeaderViewModel(string title, IUserLoader userLoader)
        {
            _title = title;
            _userLoader = userLoader;
            _unreadMessagesCount = 0;
            _participantOnlineCount = 0;
            this.MessengerInstance.Register<ParticipantLoggedInIMessage>(this, (o) => ParticipantOnlineCount++);
            this.MessengerInstance.Register<ParticipantLoggedOutIMessage>(this, (o) => ParticipantOnlineCount--);
            this.MessengerInstance.Register<MessageUnreadChangedIMessage>(this, (o) => UnreadMessagesCount = o.Count);
        }

        public override void Cleanup()
        {
            _userLoader = null;
            base.Cleanup();
            this.MessengerInstance.Unregister<ParticipantLoggedInIMessage>(this);
            this.MessengerInstance.Unregister<ParticipantLoggedOutIMessage>(this);
            this.MessengerInstance.Unregister<MessageUnreadChangedIMessage>(this);
        }
    }
}
