using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;
using Core.Services;

namespace Core.ViewModels
{
    public class TestViewModel : MvxViewModel
    {
        private string _raw;
        private string _time;
        private string _url;
        private bool _cancelable;
        private readonly IDownloadService _downloadService;
        private CancellationTokenSource _token;

        public TestViewModel(IDownloadService downloadService)
        {
            _downloadService = downloadService;
            Url = "https://bruelandkjaer.accesscontrol.windows.net/v2/metadata/IdentityProviders.js?protocol=javascriptnotify&realm=uri://setupcompanion-dev.noisesentinel.com/&version=1.0";
        }
        
        public string Raw
        {
            get { return _raw; }
            set
            {
                _raw = value;
                RaisePropertyChanged(() => Raw);
            }
        }

        public string Time
        {
            get { return _time; }
            set
            {
                _time = value;
                RaisePropertyChanged(() => Time);
            }
        }

        public string Url
        {
            get { return _url; }
            set
            {
                _url = value;
                RaisePropertyChanged(() => Url);
            }
        }

        public bool Cancelable
        {
            get { return _cancelable; }
            set
            {
                _cancelable = value;
                RaisePropertyChanged(() => Cancelable);
            }
        }

        public ICommand CancelCommand
        {
            get
            {
                return new MvxCommand(() => _downloadService.Cancel(_token), () => Cancelable);
            }
        }

        public ICommand DownloadCommand
        {
            get
            {
                return new MvxCommand(async () =>
                {
                    var st = new Stopwatch();

                    st.Start();
                    Time = "0";
                    Raw = "";
                    try
                    {
                        if (Cancelable)
                        {
                            _token = new CancellationTokenSource();
                            Raw = await _downloadService.Download(Url);
                        }
                        else
                        {
                            Raw = await _downloadService.Download(Url, _token);
                        }
                    }
                    catch(Exception ex)
                    {
                        Raw = "Exception " + ex;
                    }
                    finally
                    {
                        st.Stop();
                        Time = st.Elapsed.Seconds.ToString();
                    }
                });
            }
        }
    }
}
