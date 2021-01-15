using System;
using GalaSoft.MvvmLight;

namespace NetChat.Desktop.ViewModel.Messenger
{
    public class ParticipantObservable: ObservableObject
    {
        private bool _isOnline;
        private DateTime _updateDateTime;

        public string UserId { get; }
        public bool IsOnline
        {
            get => _isOnline;
            private set => Set(ref _isOnline, value);
        }
        public DateTime UpdateDateTime
        {
            get => _updateDateTime;
            private set => Set(ref _updateDateTime, value);
        }


        public ParticipantObservable(string userId, bool isOnline, DateTime updateDateTime)
        {
            UserId = userId;
            IsOnline = isOnline;
            UpdateDateTime = updateDateTime;
        }

        public void ChangeStatus(bool isOnline, DateTime updateDateTime)
        {
            IsOnline = isOnline;
            UpdateDateTime = updateDateTime;
        }
    }
}
