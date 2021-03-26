using System;
using System.IO;
using System.Text;
using NetChat.FileMessaging.Common;
using NetChat.FileMessaging.Models;
using NetChat.FileMessaging.Repository;
using NetChat.FileMessaging.Repository.Messages;
using NLog;

namespace NetChat.FileMessaging.Services
{
    public class FileNetChatHub : INetChatHub
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        private readonly Encoding _encoding;
        private readonly string _path;
        private readonly string _filename;
        private readonly FileSystemWatcher _fileWatcher;

        public event OnMessageReceivedHandlerAsync OnMessageReceived;
        public event OnUserStatusChangedHandlerAsync OnUserStatusChanged;
        public bool IsConnected => _fileWatcher.EnableRaisingEvents;

        public FileNetChatHub(RepositoriesConfig repositoryConfigs)
        {
            _path = repositoryConfigs.MessagesSourcePath;
            _path = !string.IsNullOrWhiteSpace(_path) ? Path.GetFullPath(_path) : throw new ArgumentNullException(nameof(_path));
            if (!Directory.Exists(Path.GetDirectoryName(_path)))
            {
                _logger.Debug("FileWatcher's directory is not exists: {0}", _path);
                try
                {
                    var dirInfo = Directory.CreateDirectory(Path.GetDirectoryName(_path));
                    _logger.Debug("FileWatcher's directory '{0}' created", dirInfo.Name);
                }
                catch(Exception e)
                {
                    _logger.Debug("{0} | {1}", e.Message, e.InnerException?.Message);
                    throw;
                }
            }

            _filename = Path.GetFileName(_path);
            _encoding = repositoryConfigs.MessagesSourceEncoding;

            _fileWatcher = new FileSystemWatcher(Path.GetDirectoryName(_path))
            {
                Filter = _filename,
                NotifyFilter = NotifyFilters.LastWrite,
            };
            _logger.Debug("FileWatcher inited! Parameters: file={0}, encoding={1}", _fileWatcher.Filter, _encoding.HeaderName);
        }

        public void Connect()
        {
            if (IsConnected) return;
            
            _fileWatcher.Changed += OnFileChangedHandler;
            _fileWatcher.EnableRaisingEvents = true;
            _logger.Debug("FileWatcher's raising events is started");
        }

        public void Disconnect()
        {
            if (!IsConnected) return;
            _fileWatcher.EnableRaisingEvents = false;
            _fileWatcher.Changed -= OnFileChangedHandler;
            _logger.Debug("FileWatcher's raising events is stopped");
        }

        private void OnFileChangedHandler(object sender, FileSystemEventArgs e)
        {
            if (e.Name != _filename) return;
            string newLine = FileHelper.ReadLastLine(_path, _encoding);
            var message = MessageDataEntity.Parse(newLine);
            switch (message.Text)
            {
                case "Logon":
                    OnUserStatusChanged?.Invoke(new OnUserStatusChangedArgs(message.UserName, true, message.DateTime));
                    break;
                case "Logout":
                    OnUserStatusChanged?.Invoke(new OnUserStatusChangedArgs(message.UserName, false, message.DateTime));
                    break;
                default:
                    OnMessageReceived?.Invoke(new OnMessageReceivedArgs(new MessageData(
                        message.Id, 
                        message.DateTime,
                        message.UserName,
                        message.Text,
                        message.UserName == CurrentUserContext.CurrentUserName,
                        message.ReplyToMessageId)));
                    break;
            }
        }

        public void Dispose()
        {
            _logger.Debug("FileNetChatHub disposing...");
            _fileWatcher.Dispose();
        }
    }
}
