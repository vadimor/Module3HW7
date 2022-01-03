using Microsoft.Extensions.DependencyInjection;
using Module3HW7.Service;
using Module3HW7.Service.Abstract;

namespace Module3HW7.Helper
{
    public class Starter
    {
        public void Start()
        {
            var starter = new ServiceCollection()
                .AddSingleton<ILoggerService, LoggerService>()
                .AddSingleton<IBackupService, BackupService>()
                .AddSingleton<IFileService, FileService>()
                .AddSingleton<IJsonService, JsonService>()
                .AddSingleton<IConfigService, ConfigService>()
                .AddScoped<Application>()
                .BuildServiceProvider();
            var app = starter.GetService<Application>();
            app.Run();
            starter.Dispose();
        }
    }
}
