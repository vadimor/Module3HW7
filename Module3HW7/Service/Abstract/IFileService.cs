using System;
using System.Threading.Tasks;

namespace Module3HW7.Service.Abstract
{
    public interface IFileService : IDisposable
    {
        public Task WriteStreamAsync(string path, string text);
        public Task<string> ReadStreamAsync(string path);
        public Task<string> ReadAsync(string path);
        public Task WriteAsync(string path, string text);
    }
}
