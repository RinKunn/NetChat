using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetChat.Services.Exceptions;
using NLog;

namespace NetChat.Services.Messaging.Messages.Storage
{
    public class MdeHub : IMdeHub
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly IMdeRepository _repository;
        private readonly FileSystemWatcher _fileWatcher;
        
        public event OnUpdateHandler OnUpdate;
        public bool IsConnected => _fileWatcher.EnableRaisingEvents;

        public MdeHub(MdeRepositoryConfig repositoryConfigs)
        {
            if (repositoryConfigs == null)
                throw new ArgumentNullException(nameof(repositoryConfigs));
            if (string.IsNullOrWhiteSpace(repositoryConfigs.MessagesSourcePath))
                throw new ArgumentNullException(nameof(repositoryConfigs.MessagesSourcePath));
            
            string path = Path.GetFullPath(repositoryConfigs.MessagesSourcePath);
            string dir = Path.GetDirectoryName(path);

            if (!Directory.Exists(dir))
                throw new DirectoryNotFoundException(dir);
#if DEBUG
            if (!Directory.Exists(dir))
            {
                _logger.Debug("FileWatcher's directory is not exists: {0}", path);
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
            _repository.ReadLastDataFromIndex();
        }

        public void Dispose()
        {
            _logger.Debug("FileNetChatHub disposing...");
            _fileWatcher.Dispose();
        }
    }
}
