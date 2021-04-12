using System;
using System.IO;
using NLog;

namespace NetChat.Services.FileMessaging
{
    public class FileMdeHub : IMdeHub
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private string _lastReadedMessageId = null;
        private readonly IMdeRepository _repository;
        private readonly FileSystemWatcher _fileWatcher;

        public event OnUpdateEventHandler OnUpdate;
        public bool IsConnected => _fileWatcher.EnableRaisingEvents;

        public FileMdeHub(MdeConfig repositoryConfigs)
        {
            if (repositoryConfigs == null)
                throw new ArgumentNullException(nameof(repositoryConfigs));
            if (string.IsNullOrWhiteSpace(repositoryConfigs.MessagesSourcePath))
                throw new ArgumentNullException(nameof(repositoryConfigs.MessagesSourcePath));

            string path = Path.GetFullPath(repositoryConfigs.MessagesSourcePath);
            string dir = Path.GetDirectoryName(path);

            if (!Directory.Exists(dir))
                throw new DirectoryNotFoundException(dir);

            _logger.Debug("Initing FileHub: path='{0}'", path);
#if DEBUG
            if (!Directory.Exists(dir))
            {
                _logger.Debug("FileWatcher's directory is not exists: '{0}'", dir);
                try
                {
                    var dirInfo = Directory.CreateDirectory(dir);
                    _logger.Debug("FileWatcher's directory '{0}' created", dirInfo.Name);
                }
                catch (Exception e)
                {
                    _logger.Error("{0} | {1}", e.Message, e.InnerException?.Message);
                    throw;
                }
            }
#endif
            _fileWatcher = new FileSystemWatcher(dir)
            {
                Filter = Path.GetFileName(path),
                NotifyFilter = NotifyFilters.LastWrite,
            };
            _logger.Debug("FileWatcher inited for file = '{0}'", _fileWatcher.Filter);
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
            var messages = _repository.GetMessagesFromId(_lastReadedMessageId);
            if (messages == null || messages.Count == 0)
            {
                _logger.Warn("File changed, but readed messages list is empty from id: {0}", _lastReadedMessageId);
                return;
            }
            _lastReadedMessageId = messages[messages.Count - 1].Id;
            OnUpdate?.Invoke(new OnUpdateEventArgs(messages));
        }

        public void Dispose()
        {
            _logger.Debug("FileNetChatHub disposing...");
            _fileWatcher.Dispose();
        }
    }
}
