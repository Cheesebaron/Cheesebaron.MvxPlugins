using System.Net.Http;

namespace Cheesebaron.MvxPlugins.ModernHttpClient.WindowsPhone
{
    public class HttpClientFactory : IHttpClientFactory
    {
        public HttpClient Get() { return new HttpClient(); }
    }
}
