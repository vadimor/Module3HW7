using System;
using System.Threading;
using System.Threading.Tasks;
using Module3HW7.Models;
using Module3HW7.Service.Abstract;

namespace Module3HW7.Service
{
    public class LoggerService : ILoggerService
    {
        private int _backupStep;
        private IFileService _fileService;
        private Config _config;
        private static readonly SemaphoreSlim _semaphore;
        private string _pathLog;
        private string _timeFormateFileName;
        private int _currentBackupStep;

        static LoggerService()
        {
            _semaphore = new SemaphoreSlim(1);
        }

        public LoggerService(IFileService fileService, IConfigService configService)
        {
            _fileService = fileService;
            _config = configService.GetConfigAsync().GetAwaiter().GetResult();
            Init();
        }

        public event Action<SemaphoreSlim, string> OnBackup;

        public async Task LogAsync(TypeLog typeLog, string message)
        {
            var consoleMessage = $"{DateTime.UtcNow}: {typeLog}: {message}{Environment.NewLine}";

            await _semaphore.WaitAsync();
            await _fileService.WriteStreamAsync(_pathLog, consoleMessage);
            await BackupAsync();
            Console.WriteLine(consoleMessage);
        }

        private async Task BackupAsync()
        {
            await Task.Run(() =>
            {
                if (++_currentBackupStep % _backupStep == 0)
                {
                    OnBackup.Invoke(_semaphore, _pathLog);
                    return;
                }

                _semaphore.Release();
            });
        }

        private void Init()
        {
            _timeFormateFileName = _config.LogConfig.TimeFormatFileName;
            _backupStep = _config.LogConfig.BackupStep;
            _currentBackupStep = 0;
            _pathLog = $"{_config.LogConfig.DirectoryLog}/{DateTime.UtcNow.ToString(_timeFormateFileName)}log.txt";
        }
    }
}
