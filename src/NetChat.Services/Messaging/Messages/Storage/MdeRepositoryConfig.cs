using System.Text;

namespace NetChat.Services.Messaging.Messages.Storage
{
    public class MdeRepositoryConfig
    {
        public MdeRepositoryConfig(string messagesSourcePath, Encoding messagesSourceEncoding)
        {
            MessagesSourcePath = messagesSourcePath;
            MessagesSourceEncoding = messagesSourceEncoding;
        }

        public string MessagesSourcePath { get; }
        public Encoding MessagesSourceEncoding { get; }
    }
}
