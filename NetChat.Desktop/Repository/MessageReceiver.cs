using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace NetChat.Desktop.Repository
{
    public class MessageReceiver : IMessageReceiver
    {
        private readonly Encoding _encoding;
        private readonly string _path;
        private readonly string _filename;
        private readonly FileSystemWatcher _fileWatcher;

        public event OnMessageReceivedHandler OnMessageReceived;
        public event OnUserLoggedInHandler OnUserLoggedIn;
        public event OnUserLoggedOutHandler OnUserLoggedOut;


        public MessageReceiver(RepositoryConfigs repositoryConfigs)
        {
            _path = repositoryConfigs.MessagesSourcePath;
            _path = !string.IsNullOrWhiteSpace(_path) ? Path.GetFullPath(_path) : throw new ArgumentNullException(nameof(_path));
            _encoding = repositoryConfigs.MessagesSourceEncoding;
            if (!Directory.Exists(Path.GetDirectoryName(_path)))
                Directory.CreateDirectory(Path.GetDirectoryName(_path));

            _filename = Path.GetFileName(_path);
            _fileWatcher = new FileSystemWatcher(Path.GetDirectoryName(_path));
            _fileWatcher.Filter = _filename;
            _fileWatcher.NotifyFilter = NotifyFilters.LastWrite;
            _fileWatcher.Changed += OnFileChangedHandler;
            _fileWatcher.EnableRaisingEvents = true;
        }

        private void OnFileChangedHandler(object sender, FileSystemEventArgs e)
        {
            if (e.Name != _filename) return;
            string newLine = FileHelper.ReadLastLine(_path, _encoding);
            var message = new NetChatMessage(newLine);
            switch (message.Text)
            {
                case "Logon":
                    OnUserLoggedIn?.Invoke(new OnUserStatusChangedArgs(message.UserName, message.DateTime));
                    break;
                case "Logout":
                    OnUserLoggedOut?.Invoke(new OnUserStatusChangedArgs(message.UserName, message.DateTime));
                    break;
                default:
                    OnMessageReceived?.Invoke(new MessageTextData() 
                    { 
                        Id = message.Id,
                        DateTime = message.DateTime,
                        UserId = message.UserName,
                        Text = message.Text
                    });
                    break;
            }
        }

        public void StopWatching()
        {
            _fileWatcher.EnableRaisingEvents = false;
            _fileWatcher.Changed -= OnFileChangedHandler;
        }

        public void Dispose()
        {
            StopWatching();
            //_fileWatcher.Dispose();
        }
    }
}
