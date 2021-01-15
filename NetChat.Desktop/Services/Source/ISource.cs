using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetChat.Desktop.Services.Source
{
    public interface ISource<T>
    {
        Task WriteData(T data);
        Task<IList<T>> ReadData(int lastDatas = 0, CancellationToken token = default);
        Task<T> ReadLastData();
    }
}
