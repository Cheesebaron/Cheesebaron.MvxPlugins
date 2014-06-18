using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Core;
using Cirrious.MvvmCross.Plugins.DownloadCache;

namespace Cheesebaron.MvxPlugins.ModernHttpClient
{
    public class MvxFastFileDownloadRequest
    {
        public string DownloadPath { get; private set; }
        public string Url { get; private set; }

        public MvxFastFileDownloadRequest(string url, string downloadPath)
        {
            Url = url;
            DownloadPath = downloadPath;
        }

        public event EventHandler<MvxFileDownloadedEventArgs> DownloadComplete;
        public event EventHandler<MvxValueEventArgs<Exception>> DownloadFailed;

        private static HttpClient CreateClient()
        {
            return Mvx.Resolve<IModernHttpClient>().Get();
        }

        public async Task Start()
        {
            try
            {
                var client = CreateClient();

                var result = await client.GetAsync(Url);
                result.EnsureSuccessStatusCode();

                using (var stream = await result.Content.ReadAsStreamAsync())
                    HandleSuccess(stream);
            }
            catch (Exception ex)
            {
                FireDownloadFailed(ex);
            }
        }

        private void HandleSuccess(Stream result)
        {
            try
            {
                var fileService = MvxFileStoreHelper.SafeGetFileStore();
                var tempFilePath = DownloadPath + ".tmp";

                fileService.WriteFile(tempFilePath, result.CopyTo);

                fileService.TryMove(tempFilePath, DownloadPath, true);
            }
            catch (Exception exception)
            {
                FireDownloadFailed(exception);
                return;
            }

            FireDownloadComplete();
        }

        private void FireDownloadFailed(Exception exception)
        {
            var handler = DownloadFailed;
            if (handler != null)
                handler(this, new MvxValueEventArgs<Exception>(exception));
        }

        private void FireDownloadComplete()
        {
            var handler = DownloadComplete;
            if (handler != null)
                handler(this, new MvxFileDownloadedEventArgs(Url, DownloadPath));
        }
    }
}