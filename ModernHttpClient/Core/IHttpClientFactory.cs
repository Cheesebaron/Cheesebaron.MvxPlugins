using System.Net.Http;

namespace Cheesebaron.MvxPlugins.ModernHttpClient
{
    public interface IHttpClientFactory
    {
        HttpClient Get();
    }
}
