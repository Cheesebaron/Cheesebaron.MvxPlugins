using System;

namespace Cheesebaron.MvxPlugins.ModernHttpClient
{
    public enum HttpClientHandlerType
    {
        /// <summary>
        /// AFNetworkHandler (Only available on iOS)
        /// </summary>
        [Obsolete("Use CFNetworkHandler instead")]
        AFNetworkHandler = 0,
        /// <summary>
        /// CFNetworkHandler (Only available on iOS)
        /// </summary>
        CFNetworkHandler = 1,
        /// <summary>
        /// NSUrlSessionHandler (Only available on iOS)
        /// </summary>
        NSUrlSessionHandler = 2,
        /// <summary>
        /// OkHttpHandler (Only available on Android)
        /// </summary>
        OkHttpHandler = 3,
        /// <summary>
        /// HttpClientHandler, default handler used in HttpClient
        /// </summary>
        HttpClientHandler = 4
    }

}
