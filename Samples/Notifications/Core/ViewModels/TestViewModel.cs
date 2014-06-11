using System;
using Cheesebaron.MvxPlugins.Notifications;
using Cirrious.MvvmCross.ViewModels;
using Notifications.Sample.Core.Services;

namespace Notifications.Sample.Core.ViewModels
{
    public class TestViewModel 
        : MvxViewModel
    {
        private readonly INotifications _notifications;
        private readonly IApiService _service;
        private string _registrationId;

        public TestViewModel(INotifications notifications, IApiService service)
        {
            _notifications = notifications;
            _service = service;

            _notifications.Registered += Registered;
            _notifications.Unregistered += UnRegistered;

            RegistrationId = _notifications.RegistrationId;
            if (!string.IsNullOrEmpty(RegistrationId))
                _service.TellApiAboutRegistrationAsync(RegistrationId);
        }

        public string RegistrationId
        {
            get { return _registrationId; }
            set
            {
                _registrationId = value;
                RaisePropertyChanged(() => RegistrationId);
            }
        }

        private async void UnRegistered(object sender, EventArgs e)
        {
            RegistrationId = string.Empty;
            await _service.TellApiAboutUnRegistrationAsync();
        }

        private async void Registered(object sender, DidRegisterForNotificationsEventArgs args)
        {
            RegistrationId = args.RegistrationId;
            await _service.TellApiAboutRegistrationAsync(RegistrationId);
        }

        public async void SubscribeToNotifications()
        {
            await _notifications.RegisterAsync();
        }

        public async void UnsubscribeToNotifications()
        {
            await _notifications.UnregisterAsync();
        }
    }
}
