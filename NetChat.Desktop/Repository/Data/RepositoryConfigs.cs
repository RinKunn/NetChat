using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChat.Desktop.Repository
{
    public class RepositoryConfigs
    {
        public RepositoryConfigs(string messagesSourcePath, Encoding messagesSourceEncoding)
        {
            MessagesSourcePath = messagesSourcePath;
            MessagesSourceEncoding = messagesSourceEncoding;
        }

        public string MessagesSourcePath { get; private set; }
        public Encoding MessagesSourceEncoding { get; private set; }

        
    }
}
