using System;

namespace NetChat.Desktop.ViewModel.Messenger.ChatArea.Messages
{
    public class TextMessageObservable : MessageObservable
    {
        public string Text { get; }

        public TextMessageObservable(string messageId, DateTime dateTime,
            string authorName, bool isOutgoing, string text, ReplyObservable reply = null)
            : base(messageId, dateTime, authorName, isOutgoing, reply)
        {
            Text = text;
        }
    }
}
