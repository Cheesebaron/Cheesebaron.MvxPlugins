using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Cheesebaron.MvxPlugins.ModernHttpClient
{
    public class RetryHandler : DelegatingHandler
    {
        private readonly int _maxRetries = 3;

        public RetryHandler(HttpMessageHandler innerHandler, int maxRetries = 3)
            : base(innerHandler) { _maxRetries = maxRetries; }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            Exception ex = null;
            HttpResponseMessage response = null;
            for (var i = 0; i < _maxRetries; i++)
            {
                try
                {
                    response = await base.SendAsync(request, cancellationToken);
                    if (response.IsSuccessStatusCode)
                    {
                        return response;
                    }  
                }
                catch(Exception e)
                {
                    ex = e;
                }
            }

            if (ex != null)
                throw ex;

            return response;
        }
    }
}
