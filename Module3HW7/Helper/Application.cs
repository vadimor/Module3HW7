using System.Collections.Generic;
using System.Threading.Tasks;
using Module3HW7.Models;
using Module3HW7.Service.Abstract;

namespace Module3HW7.Helper
{
    public class Application
    {
        private ILoggerService _logger;
        private IBackupService _backupService;
        public Application(ILoggerService loggerService, IBackupService backupService)
        {
            _logger = loggerService;
            _backupService = backupService;
        }

        public void Run()
        {
            _logger.OnBackup += async (sl, s) =>
            {
                await _backupService.BackupAsync(s);
                sl.Release();
            };

            var list = new List<Task>();
            list.Add(Task.Run(async () =>
            {
                await DoLogAsync();
            }));
            list.Add(Task.Run(async () =>
            {
                await DoLogAsync();
            }));

            Task.WaitAll(list.ToArray());
        }

        private async Task DoLogAsync()
        {
            for (var i = 0; i < 50; i++)
            {
                await _logger.LogAsync(TypeLog.Info, i.ToString());
            }
        }
    }
}
