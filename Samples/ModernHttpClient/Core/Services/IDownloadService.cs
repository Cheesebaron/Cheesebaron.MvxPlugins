using System.Threading;
using System.Threading.Tasks;

namespace Core.Services
{
    public interface IDownloadService
    {
        Task<string> Download(string url, CancellationTokenSource token = null);
        void CancelCurrent();
        void Cancel(CancellationTokenSource token);
    }
}
