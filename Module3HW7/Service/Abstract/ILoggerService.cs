using System;
using System.Threading;
using System.Threading.Tasks;
using Module3HW7.Models;

namespace Module3HW7.Service.Abstract
{
    public interface ILoggerService
    {
        public event Action<SemaphoreSlim, string> OnBackup;
        public Task LogAsync(TypeLog typeLog, string message);
    }
}
