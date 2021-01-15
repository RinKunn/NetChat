using System;
using System.IO;
using System.Text;
using NetChat.FileMessaging.Common;
using NetChat.FileMessaging.Models;
using NetChat.FileMessaging.Repository;
using NetChat.FileMessaging.Repository.Messages;

namespace NetChat.FileMessaging.Services
{
    public class FileNetChatHub : INetChatHub
    {
        private readonly Encoding _encoding;
        private readonly string _path;
        private readonly string _filename;
        private readonly FileSystemWatcher _fileWatcher;

        public event OnMessageReceivedHandler OnMessageReceived;
        public event OnUserStatusChangedHandler OnUserStatusChanged;
        public bool IsConnected => _fileWatcher.EnableRaisingEvents;

        public FileNetChatHub(RepositoriesConfig repositoryConfigs)
        {
            _path = repositoryConfigs.MessagesSourcePath;
            _path = !string.IsNullOrWhiteSpace(_path) ? Path.GetFullPath(_path) : throw new ArgumentNullException(nameof(_path));
            _encoding = repositoryConfigs.MessagesSourceEncoding;
            if (!Directory.Exists(Path.GetDirectoryName(_path)))
                Directory.CreateDirectory(Path.GetDirectoryName(_path));

            _filename = Path.GetFileName(_path);
            _fileWatcher = new FileSystemWatcher(Path.GetDirectoryName(_path))
            {
                Filter = _filename,
                NotifyFilter = NotifyFilters.LastWrite
            };
        }

        public void Connect()
        {
            _fileWatcher.Changed += OnFileChangedHandler;
            _fileWatcher.EnableRaisingEvents = true;
        }

        public void Disconnect()
        {
            _fileWatcher.EnableRaisingEvents = false;
            _fileWatcher.Changed -= OnFileChangedHandler;
        }

        private void OnFileChangedHandler(object sender, FileSystemEventArgs e)
        {
            if (e.Name != _filename) return;
            string newLine = FileHelper.ReadLastLine(_path, _encoding);
            var message = new TextMessageData(newLine);
            switch (message.Text)
            {
                case "Logon":
                    OnUserStatusChanged?.Invoke(new OnUserStatusChangedArgs(message.UserName, true, message.DateTime));
                    break;
                case "Logout":
                    OnUserStatusChanged?.Invoke(new OnUserStatusChangedArgs(message.UserName, false, message.DateTime));
                    break;
                default:
                    OnMessageReceived?.Invoke(new TextMessage()
                    {
                        Id = message.Id,
                        DateTime = message.DateTime,
                        SenderId = message.UserName,
                        Text = message.Text
                    });
                    break;
            }
        }

        public void Dispose()
        {
            _fileWatcher.Dispose();
        }
    }
}
