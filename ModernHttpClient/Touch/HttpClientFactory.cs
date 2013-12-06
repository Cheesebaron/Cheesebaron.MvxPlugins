using System.Net.Http;
using ModernHttpClient;

namespace Cheesebaron.MvxPlugins.ModernHttpClient.Touch
{
    public class HttpClientFactory : IHttpClientFactory
    {
        public HttpClient Get()
        {
            return new HttpClient(new AFNetworkHandler());
        }
    }
}