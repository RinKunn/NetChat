using System.Text;

namespace NetChat.Services.FileMessaging
{
    public class MdeConfig
    {
        public MdeConfig(string messagesSourcePath, Encoding messagesSourceEncoding)
        {
            MessagesSourcePath = messagesSourcePath;
            MessagesSourceEncoding = messagesSourceEncoding;
        }

        public string MessagesSourcePath { get; }
        public Encoding MessagesSourceEncoding { get; }
    }
}
