using System;

namespace Cheesebaron.MvxPlugins.AzureAccessControl
{
    public delegate void LoginErrorEventHandler(object sender, LoginErrorEventArgs args);
    public class LoginErrorEventArgs : EventArgs
    {
        public Exception Exception { get; set; }
        public String Message { get; set; }
    }
}
