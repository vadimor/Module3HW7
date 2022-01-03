using System.Threading.Tasks;
using Module3HW7.Models;

namespace Module3HW7.Service.Abstract
{
    public interface IConfigService
    {
        public Task<Config> GetConfigAsync();
    }
}
