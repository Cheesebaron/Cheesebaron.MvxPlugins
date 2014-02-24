using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;

namespace Cheesebaron.MvxPlugins.ModernHttpClient
{
    public class RetryHandler : DelegatingHandler
    {
        private readonly int _maxRetries;

        public RetryHandler(HttpMessageHandler innerHandler, int maxRetries = 5)
            : base(innerHandler) { _maxRetries = maxRetries; }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {

            Exception lastException = null;
            HttpResponseMessage response = null;
            for (var i = 0; i < _maxRetries; i++)
            {
                try
                {
                    response = await base.SendAsync(request, cancellationToken);
                    if (response.IsSuccessStatusCode)
                    {
                        Mvx.Trace(MvxTraceLevel.Diagnostic, "Request was ok after {0} retries...", i);
                        return response;
                    }
                }
                catch (Exception e)
                {
                    lastException = e;
                }
                Mvx.Trace(MvxTraceLevel.Diagnostic, "Request failed, retrying...\n{0}", lastException);
                await Task.Delay(500 + (i * 500), cancellationToken);
            }

            if (lastException != null)
                throw lastException;

            return response;
        }
    }
}
