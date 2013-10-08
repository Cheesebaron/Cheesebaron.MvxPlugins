using System.Windows.Input;
using Cheesebaron.MvxPlugins.Connectivity;
using Cirrious.MvvmCross.ViewModels;

namespace Core.ViewModels
{
    public class FirstViewModel 
        : MvxViewModel
    {
        private readonly IConnectivity _connectivity;
        public FirstViewModel(IConnectivity connectivity)
        {
            _connectivity = connectivity;
        }

        private string _host = "ostebaronen.dk";
        public string Host
        {
            get { return _host; }
            set
            {
                _host = value;
                RaisePropertyChanged("Host");
            }
        }

        private bool _hostReachable;
        public bool HostReachable
        {
            get { return _hostReachable; }
            set
            {
                _hostReachable = value;
                RaisePropertyChanged("HostReachable");
            }
        }

        private bool _isConnected;
        public bool IsConnected
        {
            get { return _isConnected; }
            set
            {
                _isConnected = value;
                RaisePropertyChanged("IsConnected");
            }
        }

        private ConnectionType[] _connectionType;
        public ConnectionType[] ConnectionType
        {
            get { return _connectionType; }
            set
            {
                _connectionType = value;
                RaisePropertyChanged("ConnectionType");
            }
        }

        private int[] _bandwidth;
        public int[] Bandwidth
        {
            get { return _bandwidth; }
            set
            {
                _bandwidth = value;
                RaisePropertyChanged("Bandwidth");
            }
        }
        
        public ICommand GetOtherInfoCommand
        {
            get
            {
                return new MvxCommand(() =>
                    {
                        Bandwidth = _connectivity.Bandwidths;
                        ConnectionType = _connectivity.ConnectionTypes;
                    });
            }
        }

        public ICommand CheckIfHostReachableCommand
        {
            get
            {
                return new MvxCommand(async () =>
                    {
                        HostReachable = await _connectivity.IsHostReachable(Host);
                    });
            }
        }

        public ICommand CheckIfPhoneConnected
        {
            get
            {
                return new MvxCommand(() =>
                    {
                        IsConnected = _connectivity.IsConnected;
                    });
            }
        }
    }
}
