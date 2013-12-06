using System.Net.Http;
using ModernHttpClient;

namespace Cheesebaron.MvxPlugins.ModernHttpClient.Droid
{
    public class HttpClientFactory : IHttpClientFactory
    {
        public HttpClient Get()
        {
            return new HttpClient(new OkHttpNetworkHandler());
        }
    }
}