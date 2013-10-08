using System.Collections.Generic;
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

        private bool _pingReachable;
        public bool PingReachable
        {
            get { return _pingReachable; }
            set
            {
                _pingReachable = value;
                RaisePropertyChanged("PingReachable");
            }
        }

        private bool _portReachable;
        public bool PortReachable
        {
            get { return _portReachable; }
            set
            {
                _portReachable = value;
                RaisePropertyChanged("PortReachable");
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

        private IEnumerable<ConnectionType> _connectionType;
        public IEnumerable<ConnectionType> ConnectionType
        {
            get { return _connectionType; }
            set
            {
                _connectionType = value;
                RaisePropertyChanged("ConnectionType");
            }
        }

        private IEnumerable<int> _bandwidth;
        public IEnumerable<int> Bandwidth
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
                        PingReachable = await _connectivity.IsPingReachable(Host);
                        PortReachable = await _connectivity.IsPortReachable(Host);
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
