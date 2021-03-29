using System.Text;

namespace NetChat.Services.FileMessaging.Repository
{
    public class RepositoryConfig
    {
        public RepositoryConfig(string messagesSourcePath, Encoding messagesSourceEncoding)
        {
            MessagesSourcePath = messagesSourcePath;
            MessagesSourceEncoding = messagesSourceEncoding;
        }

        public string MessagesSourcePath { get; }
        public Encoding MessagesSourceEncoding { get; }
    }
}
