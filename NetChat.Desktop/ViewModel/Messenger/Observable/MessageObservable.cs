using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace NetChat.Desktop.ViewModel.Messenger
{
    public abstract class MessageObservable : ObservableObject
    {
        private bool _isReaded;

        protected MessageObservable(string id, DateTime dateTime, ParticipantObservable sender, bool isOriginNative, bool isReaded)
        {
            Id = id;
            DateTime = dateTime;
            Sender = sender;
            IsOriginNative = isOriginNative;
            IsReaded = isOriginNative || isReaded;
        }

        public string Id { get; }
        public DateTime DateTime { get; }
        public ParticipantObservable Sender { get; }

        public bool IsOriginNative { get; }

        public bool IsReaded
        {
            get => _isReaded;
            set => Set(ref _isReaded, value);
        }

    }
}
