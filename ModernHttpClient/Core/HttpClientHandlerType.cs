namespace Cheesebaron.MvxPlugins.ModernHttpClient
{
    public enum HttpClientHandlerType
    {
        /// <summary>
        /// AFNetworkHandler (Only available on iOS)
        /// </summary>
        AFNetworkHandler = 0,
        /// <summary>
        /// CFNetworkHandler (Only available on iOS)
        /// </summary>
        CFNetworkHandler = 1,
        /// <summary>
        /// OkHttpHandler (Only available on Android)
        /// </summary>
        OkHttpHandler = 2,
        /// <summary>
        /// HttpClientHandler, default handler used in HttpClient
        /// </summary>
        HttpClientHandler = 3
    }

}
