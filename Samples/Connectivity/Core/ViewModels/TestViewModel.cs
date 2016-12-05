using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Cheesebaron.MvxPlugins.Connectivity;
using MvvmCross.Core.ViewModels;

namespace Core.ViewModels
{
    public class TestViewModel : MvxViewModel
    {
        private string _hostName;
        private bool _hostResolved;
        private MvxAsyncCommand _resolveHostCommand;
        private MvxCommand _goToWifiCommand;

        public TestViewModel(IConnectivity connectivity) { Connectivity = connectivity; }

        public IConnectivity Connectivity { get; }

        public string HostName
        {
            get { return _hostName; }
            set { SetProperty(ref _hostName, value); }
        }

        public bool HostResolved
        {
            get { return _hostResolved; }
            set { SetProperty(ref _hostResolved, value); }
        }

        public ICommand ResolveHostCommand =>
            _resolveHostCommand ?? (_resolveHostCommand = new MvxAsyncCommand(DoResolveHostCommand));

        public ICommand GoToWifiCommand =>
            _goToWifiCommand ?? (_goToWifiCommand = new MvxCommand(() => ShowViewModel<WifiViewModel>()));

        private async Task DoResolveHostCommand()
        {
            var cts = new CancellationTokenSource(new TimeSpan(0, 0, 1, 0));
            HostResolved = await Connectivity.GetHostReachableAsync(HostName, cts.Token);
        }
    }
}
