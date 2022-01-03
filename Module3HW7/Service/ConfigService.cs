using System.Threading.Tasks;
using Module3HW7.Models;
using Module3HW7.Service.Abstract;

namespace Module3HW7.Service
{
    public class ConfigService : IConfigService
    {
        private const string _ConfigPath = "./config.json";
        private IFileService _fileService;
        private IJsonService _jsonService;
        private Config _config;
        public ConfigService(IFileService fileService, IJsonService jsonService)
        {
            _fileService = fileService;
            _jsonService = jsonService;
            InitAsync().GetAwaiter().GetResult();
        }

        public async Task<Config> GetConfigAsync()
        {
            return await Task.Run(() =>
            {
                return _config;
            });
        }

        private async Task InitAsync()
        {
            var data = await _fileService.ReadAsync(_ConfigPath);
            _config = await _jsonService.DeserializationAsync(data, typeof(Config)) as Config;
        }
    }
}
