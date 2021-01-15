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
        protected MessageObservable(string id, DateTime dateTime, ParticipantObservable sender, bool isOriginNative)
        {
            Id = id;
            DateTime = dateTime;
            Sender = sender;
            IsOriginNative = isOriginNative;
        }

        public string Id { get; }
        public DateTime DateTime { get; }
        public ParticipantObservable Sender { get; }
        public bool IsOriginNative { get; }
    }
}
