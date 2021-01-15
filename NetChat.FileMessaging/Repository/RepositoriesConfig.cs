using System.IO;
using System.Text;

namespace NetChat.FileMessaging.Repository
{
    public class RepositoriesConfig
    {
        public RepositoriesConfig(string messagesSourcePath, Encoding messagesSourceEncoding)
        {
            MessagesSourcePath = messagesSourcePath;
            if (!File.Exists(messagesSourcePath))
                File.Create(messagesSourcePath).Close();
            MessagesSourceEncoding = messagesSourceEncoding;
        }

        public string MessagesSourcePath { get; }
        public Encoding MessagesSourceEncoding { get; }
    }
}
