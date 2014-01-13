using System.Threading;
using System.Threading.Tasks;
using Cheesebaron.MvxPlugins.ModernHttpClient;

namespace Core.Services
{
    public class DownloadService : IDownloadService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private CancellationTokenSource _currentToken;

        public DownloadService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> Download(string url, CancellationTokenSource token = null)
        {
            _currentToken = token ?? new CancellationTokenSource();

            var handler = _httpClientFactory.GetHandler();
            var outerHandler = new RetryHandler(handler, 3);
            var client = _httpClientFactory.Get(outerHandler);
            var msg = await client.GetAsync(url, _currentToken.Token);

            if (!msg.IsSuccessStatusCode) return "Something derped";

            var result = await msg.Content.ReadAsStringAsync();
            return result;
        }

        public void CancelCurrent()
        {
            if (_currentToken != null)
                _currentToken.Cancel();
        }

        public void Cancel(CancellationTokenSource token)
        {
            if (token == null)
                CancelCurrent();
            else
                token.Cancel();
        }
    }
}
