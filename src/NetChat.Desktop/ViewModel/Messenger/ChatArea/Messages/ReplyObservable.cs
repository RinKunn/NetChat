using GalaSoft.MvvmLight;

namespace NetChat.Desktop.ViewModel.Messenger.ChatArea.Messages
{
    public class ReplyObservable : ObservableObject
    {
        public ReplyObservable(string messageId, string authorName, string text)
        {
            MessageId = messageId;
            AuthorName = authorName;
            Text = text;
        }

        public string MessageId { get; }
        public string AuthorName { get; }
        public string Text { get; }
        //private Message _message { get; }
    }
}
