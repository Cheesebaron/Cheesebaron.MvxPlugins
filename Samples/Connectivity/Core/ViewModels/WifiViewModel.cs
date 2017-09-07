using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Cheesebaron.MvxPlugins.Connectivity;
using Cheesebaron.MvxPlugins.Connectivity.Models;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Platform.Core;

namespace Core.ViewModels
{
    public class WifiInfoViewModel : MvxNotifyPropertyChanged
    {
        private readonly WifiInfo _info;

        public WifiInfoViewModel(WifiInfo info)
        {
            _info = info;
        }

        public string Ssid => _info.Ssid;
        public string Bssid => _info.Bssid;
        public string IpAddress => _info.Extra?.IpAddress;
        public string MacAddress => _info.Extra?.MacAddress;
        public string SecurityMode => _info.Extra?.SecurityMode;
        public int? Rssi => (int?) _info.Extra?.Rssi;
    }


    public class WifiViewModel : MvxViewModel
    {
        private readonly IWifi _wifi;
        private readonly IMvxMainThreadDispatcher _dispatcher;

        public WifiViewModel(IWifi wifi, IMvxMainThreadDispatcher dispatcher)
        {
            _wifi = wifi;
            _dispatcher = dispatcher;
        }

        public MvxObservableCollection<WifiInfoViewModel> WifiInfo { get; } =
            new MvxObservableCollection<WifiInfoViewModel>();

        private bool _isScanning;

        public bool IsScanning
        {
            get { return _isScanning; }
            set { SetProperty(ref _isScanning, value); }
        }

        private WifiInfoViewModel _currentInfo;

        public WifiInfoViewModel CurrentInfo
        {
            get { return _currentInfo; }
            set { SetProperty(ref _currentInfo, value); }
        }

        private MvxCommand _getCurrentWifiInfoCommand;

        public ICommand GetCurrentWifiInfoCommand =>
            _getCurrentWifiInfoCommand = _getCurrentWifiInfoCommand ?? new MvxCommand(DoGetCurrentWifiInfoCommand);

        private void DoGetCurrentWifiInfoCommand()
        {
            var info = _wifi.GetCurrentWifiInfo();
            CurrentInfo = new WifiInfoViewModel(info);
        }

        private MvxAsyncCommand _getAllWifiInfoCommand;

        public ICommand GetAllWifiInfoCommand =>
            _getAllWifiInfoCommand = _getAllWifiInfoCommand ?? new MvxAsyncCommand(DoGetAllWifiInfoCommand);

        private async Task DoGetAllWifiInfoCommand(CancellationToken token = default(CancellationToken))
        {
            IsScanning = true;
            try
            {
                var allInfo = await _wifi.GetAllWifiInfoAsync(token).ConfigureAwait(false);

                _dispatcher.RequestMainThreadAction(() =>
                {
                    WifiInfo.Clear();
                    WifiInfo.AddRange(allInfo.Select(info => new WifiInfoViewModel(info)));
                });
            }
            catch(Exception e)
            {
                Mvx.TaggedError("WifiViewModel", $"Failed getting All Wifi Info: {e}");
            }
            finally
            {
                IsScanning = false;
            }
        }
    }
}
