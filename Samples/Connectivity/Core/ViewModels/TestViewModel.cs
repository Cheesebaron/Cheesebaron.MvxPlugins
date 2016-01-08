using System;
using System.Threading;
using System.Windows.Input;
using Cheesebaron.MvxPlugins.Connectivity;
using Cirrious.MvvmCross.ViewModels;

namespace Core.ViewModels
{
    public class TestViewModel : MvxViewModel
    {
        private string _hostName;
        private bool _hostResolved;
        private MvxCommand _resolveHostCommand;

        public TestViewModel(IConnectivity connectivity) { Connectivity = connectivity; }

        public IConnectivity Connectivity { get; }

        public string HostName
        {
            get { return _hostName; }
            set
            {
                _hostName = value;
                RaisePropertyChanged(() => HostName);
            }
        }

        public bool HostResolved
        {
            get { return _hostResolved; }
            set
            {
                _hostResolved = value;
                RaisePropertyChanged(() => HostResolved);
            }
        }

        public ICommand ResolveHostCommand =>
            _resolveHostCommand ?? (_resolveHostCommand = new MvxCommand(DoResolveHostCommand));

        private async void DoResolveHostCommand()
        {
            var cts = new CancellationTokenSource(new TimeSpan(0, 0, 1, 0));
            HostResolved = await Connectivity.GetHostReachableAsync(HostName, cts.Token).ConfigureAwait(false);
        }
    }
}
