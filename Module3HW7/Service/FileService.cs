using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Module3HW7.Service.Abstract;

namespace Module3HW7.Service
{
    public class FileService : IFileService
    {
        private static object _lockerReader;
        private static object _lockerWriter;
        private Dictionary<string, StreamReader> _streamReaderList;
        private Dictionary<string, StreamWriter> _streamWriterList;
        private bool _disposed;
        static FileService()
        {
            _lockerReader = new object();
            _lockerWriter = new object();
        }

        public FileService()
        {
            Init();
        }

        public async Task WriteStreamAsync(string path, string text)
        {
            var directory = await GetDirectoryByFilePathAsync(path);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var fullPath = Path.GetFullPath(path);
            lock (_lockerWriter)
            {
                if (!_streamWriterList.ContainsKey(fullPath))
                {
                    _streamWriterList.Add(fullPath, new StreamWriter(new FileStream(fullPath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite)));
                }
            }

            await _streamWriterList[fullPath].WriteAsync(text);
            await _streamWriterList[fullPath].FlushAsync();
        }

        public async Task<string> ReadStreamAsync(string path)
        {
            if (!File.Exists(path))
            {
                return null;
            }

            var fullPath = Path.GetFullPath(path);
            lock (_lockerReader)
            {
                if (!_streamReaderList.ContainsKey(fullPath))
                {
                    _streamReaderList.Add(fullPath, new StreamReader(new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)));
                }
            }

            _streamReaderList[fullPath].BaseStream.Position = 0;
            return await _streamReaderList[fullPath].ReadToEndAsync();
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;
            foreach (var item in _streamReaderList)
            {
                item.Value.Close();
                item.Value.Dispose();
            }

            foreach (var item in _streamWriterList)
            {
                item.Value.Flush();
                item.Value.Close();
                item.Value.Dispose();
            }
        }

        public async Task<string> ReadAsync(string path)
        {
            return await File.ReadAllTextAsync(path);
        }

        public async Task WriteAsync(string path, string text)
        {
            var directory = await GetDirectoryByFilePathAsync(path);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            await File.WriteAllTextAsync(path, text);
        }

        private async Task<string> GetDirectoryByFilePathAsync(string path)
        {
            return await Task.Run(() =>
            {
                var splitPath = path.Split("/");
                var sizeFileName = splitPath[splitPath.Length - 1].Length;
                return path.Substring(0, path.Length - sizeFileName);
            });
        }

        private void Init()
        {
            _streamReaderList = new Dictionary<string, StreamReader>();
            _streamWriterList = new Dictionary<string, StreamWriter>();
        }
    }
}
