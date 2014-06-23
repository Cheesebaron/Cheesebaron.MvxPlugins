using System;
using System.Collections.Generic;
using System.Linq;
using Cirrious.CrossCore.Core;
using Cirrious.MvvmCross.Plugins.DownloadCache;

namespace Cheesebaron.MvxPlugins.ModernHttpClient
{
    public class ModernHttpFileDownloader 
        : MvxLockableObject
            , IMvxHttpFileDownloader
    {
        private readonly Dictionary<MvxFastFileDownloadRequest, bool> _currentRequests =
            new Dictionary<MvxFastFileDownloadRequest, bool>();

        private const int DefaultMaxConcurrentDownloads = 30;
        private readonly int _maxConcurrentDownloads;
        private readonly Queue<MvxFastFileDownloadRequest> _queuedRequests = new Queue<MvxFastFileDownloadRequest>();

        public ModernHttpFileDownloader(int maxConcurrentDownloads = DefaultMaxConcurrentDownloads)
        {
            _maxConcurrentDownloads = maxConcurrentDownloads;
        }

        public void RequestDownload(string url, string downloadPath, Action success, Action<Exception> error)
        {
            var request = new MvxFastFileDownloadRequest(url, downloadPath);
            request.DownloadComplete += (sender, args) =>
            {
                OnRequestFinished(request);
                success();
            };
            request.DownloadFailed += (sender, args) =>
            {
                OnRequestFinished(request);
                error(args.Value);
            };

            RunSyncOrAsyncWithLock(() =>
            {
                _queuedRequests.Enqueue(request);
                if (_currentRequests.Count < _maxConcurrentDownloads)
                {
                    StartNextQueuedItem();
                }
            });
        }

        private void OnRequestFinished(MvxFastFileDownloadRequest request)
        {
            RunSyncOrAsyncWithLock(() =>
            {
                _currentRequests.Remove(request);
                if (_queuedRequests.Any())
                {
                    StartNextQueuedItem();
                }
            });
        }

        private void StartNextQueuedItem()
        {
            if (_currentRequests.Count >= _maxConcurrentDownloads)
                return;

            RunSyncOrAsyncWithLock(async () =>
            {
                if (_currentRequests.Count >= _maxConcurrentDownloads)
                    return;

                if (!_queuedRequests.Any())
                    return;

                var request = _queuedRequests.Dequeue();
                _currentRequests.Add(request, true);
                await request.Start();
            });
        }
    }
}