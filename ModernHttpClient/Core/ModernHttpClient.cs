using System.Net.Http;
using ModernHttpClient;

namespace Cheesebaron.MvxPlugins.ModernHttpClient
{
    public class ModernHttpClient : IModernHttpClient
    {
        public HttpClient Get()
        {
            return new HttpClient(GetNativeHandler());
        }

        public HttpClient Get(HttpMessageHandler handler)
        {
            return new HttpClient(handler);
        }

        public HttpMessageHandler GetNativeHandler(bool throwOnCaptiveNetwork = false)
        {
            return new NativeMessageHandler(throwOnCaptiveNetwork);
        }
    }
}
