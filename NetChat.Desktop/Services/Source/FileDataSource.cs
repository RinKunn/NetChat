using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

namespace NetChat.Desktop.Services.Source
{
    public class FileDataSource : ISource<string>
    {
        private readonly string _filename;
        private readonly Encoding _encoding;

        public FileDataSource(string filename, Encoding encoding)
        {
            _filename = filename;
            _encoding = encoding;
        }

        public async Task<IList<string>> ReadData(int lastDatas = 0, CancellationToken token = default)
        {
            int DefaultBufferSize = 4096;
            FileOptions DefaultOptions = FileOptions.Asynchronous | FileOptions.SequentialScan;
            var lines = new List<string>();
            using (var stream = new FileStream(_filename, FileMode.Open, FileAccess.Read, FileShare.Read, DefaultBufferSize, DefaultOptions))
            using (var reader = new StreamReader(stream, _encoding))
            {
                string line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    token.ThrowIfCancellationRequested();
                    lines.Add(line);
                }
            }
            return (lastDatas == 0 ? lines.ToArray() : lines.Skip(Math.Max(0, lines.Count - lastDatas)).ToArray());
        }

        public Task<string> ReadLastData()
        {
            return Task.FromResult("");
        }

        public async Task WriteData(string data)
        {
            await Task.Run(() => File.AppendAllText(_filename, data.ToString() + '\n', _encoding));
        }
    }
}

