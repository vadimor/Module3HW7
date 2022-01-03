using System;
using System.Threading.Tasks;
using Module3HW7.Models;
using Module3HW7.Service.Abstract;

namespace Module3HW7.Service
{
    public class BackupService : IBackupService
    {
        private string _directoryBackup;
        private string _timeFormateFileName;
        private Config _config;
        private IFileService _fileService;
        public BackupService(IFileService fileService, IConfigService configService)
        {
            _fileService = fileService;
            _config = configService.GetConfigAsync().GetAwaiter().GetResult();
            Init();
        }

        public async Task BackupAsync(string path)
        {
            var backupPath = $"{_directoryBackup}/{DateTime.UtcNow.ToString(_timeFormateFileName)}backup.txt";
            var data = await _fileService.ReadStreamAsync(path);
            await _fileService.WriteAsync(backupPath, data);
        }

        public void Init()
        {
            _timeFormateFileName = _config.BackupConfig.TimeFormatFileName;
            _directoryBackup = _config.BackupConfig.DirectoryBackup;
        }
    }
}
