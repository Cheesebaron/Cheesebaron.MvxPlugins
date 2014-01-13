using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Cheesebaron.MvxPlugins.ModernHttpClient
{
    public class RetryHandler : DelegatingHandler
    {
        // Strongly consider limiting the number of retries - "retry forever" is
        // probably not the most user friendly way you could respond to "the
        // network cable got pulled out."
        private readonly int _maxRetries = 3;

        public RetryHandler(HttpMessageHandler innerHandler, int maxRetries = 3)
            : base(innerHandler) { _maxRetries = maxRetries; }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            HttpResponseMessage response = null;
            for (var i = 0; i < _maxRetries; i++)
            {
                response = await base.SendAsync(request, cancellationToken);
                if (response.IsSuccessStatusCode)
                {
                    return response;
                }    
            }

            return response;
        }
    }
}
