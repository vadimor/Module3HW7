using System.Threading.Tasks;

namespace Module3HW7.Service.Abstract
{
    public interface IBackupService
    {
        public Task BackupAsync(string path);
    }
}
