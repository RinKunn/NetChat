using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace NetChat.FileMessaging.Common
{
    public static class FileHelper
    {
        public static async Task<string[]> GetStringMessagesAsync(string filePath, Encoding encoding, int lastLines = 0, CancellationToken token = default)
        {
            int DefaultBufferSize = 4096;
            FileOptions DefaultOptions = FileOptions.Asynchronous | FileOptions.SequentialScan;
            var lines = new List<string>();
            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, DefaultBufferSize, DefaultOptions))
            using (var reader = new StreamReader(stream, encoding))
            {
                string line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    token.ThrowIfCancellationRequested();
                    lines.Add(line);
                }
            }
            return (lastLines == 0 ? lines.ToArray() : lines.Skip(Math.Max(0, lines.Count - lastLines)).ToArray());
        }

        public static string ReadLastLine(string filePath, Encoding encoding, string newline = "\n")
        {
            int charsize = encoding.GetByteCount("\n");
            byte[] buffer = encoding.GetBytes(newline);
            using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                long endpos = stream.Length / charsize;

                for (long pos = charsize; pos <= endpos; pos += charsize)
                {
                    stream.Seek(-pos, SeekOrigin.End);
                    stream.Read(buffer, 0, buffer.Length);

                    if (encoding.GetString(buffer) == newline && stream.Length - stream.Position > charsize)
                    {
                        buffer = new byte[stream.Length - stream.Position];
                        stream.Read(buffer, 0, buffer.Length);
                        return Regex.Replace(encoding.GetString(buffer), @"[\u0000-\u001F]", string.Empty);
                    }
                    if (pos == endpos)
                    {
                        stream.Seek(-pos, SeekOrigin.End);
                        buffer = new byte[stream.Length];
                        stream.Read(buffer, 0, buffer.Length);
                        return Regex.Replace(encoding.GetString(buffer), @"[\u0000-\u001F]", string.Empty);
                    }
                }
            }
            return null;
        }
    }
}
