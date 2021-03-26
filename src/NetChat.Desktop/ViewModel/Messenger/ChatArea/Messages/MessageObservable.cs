using System;
using GalaSoft.MvvmLight;

namespace NetChat.Desktop.ViewModel.Messenger.ChatArea.Messages
{
    public abstract class MessageObservable : ObservableObject
    {
        protected MessageObservable(string messageId, DateTime dateTime,
            string authorName, bool isOutgoing,
            ReplyObservable reply = null)
        {
            MessageId = messageId;
            DateTime = dateTime;
            AuthorName = authorName;
            IsOutgoing = isOutgoing;
            Reply = reply;
            HasReply = reply != null;
        }

        public string MessageId { get; }
        public DateTime DateTime { get; }
        public string AuthorName { get; }
        public bool IsOutgoing { get; }
        public bool HasReply { get; }
        public ReplyObservable Reply { get; }

        //public Message MessageData { get; }
    }
}
