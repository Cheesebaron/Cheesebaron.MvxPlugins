using System.Linq;
using System.Windows.Input;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.WindowsCommon.Views;

#if WINDOWS_PHONE_APP
using Windows.Phone.UI.Input;
#endif

namespace Store.Views
{
    public abstract class BaseView<TViewModel>
        : MvxWindowsPage<TViewModel>
        where TViewModel : MvxViewModel
    {
        private ICommand _goBackCommand;

        public ICommand GoBackCommand
        {
            get { return _goBackCommand ?? (_goBackCommand = new MvxCommand(GoBack, CanGoBack)); }
            set { _goBackCommand = value; }
        }

        private ICommand _goForwardCommand;

        public ICommand GoForwardCommand
        {
            get { return _goForwardCommand ?? (_goForwardCommand = new MvxCommand(GoBack, CanGoBack)); }
            set { _goForwardCommand = value; }
        }

        protected BaseView()
        {
#if WINDOWS_PHONE_APP
            NavigationCacheMode = NavigationCacheMode.Required;
            Loaded += (s, e) =>
            {
                HardwareButtons.BackPressed += HardwareButtonsBackPressed;
            };

            Unloaded += (s, e) =>
            {
                HardwareButtons.BackPressed -= HardwareButtonsBackPressed;
            };
#endif

#if WINDOWS_APP
            Loaded += (s, a) => {
                if (ActualHeight == Window.Current.Bounds.Height &&
                    ActualWidth == Window.Current.Bounds.Width)
                {
                    Window.Current.CoreWindow.Dispatcher.AcceleratorKeyActivated += DispatcherOnAcceleratorKeyActivated;
                    Window.Current.CoreWindow.PointerPressed += CoreWindowOnPointerPressed;
                }
            };

            Unloaded += (s, a) => {
                Window.Current.CoreWindow.Dispatcher.AcceleratorKeyActivated -= DispatcherOnAcceleratorKeyActivated;
                Window.Current.CoreWindow.PointerPressed -= CoreWindowOnPointerPressed;
            };
#endif
        }

#if WINDOWS_APP
        private void CoreWindowOnPointerPressed(CoreWindow sender, PointerEventArgs args)
        {
            var properties = args.CurrentPoint.Properties;

            if (properties.IsLeftButtonPressed || properties.IsRightButtonPressed ||
                properties.IsMiddleButtonPressed)
                return;

            var backPressed = properties.IsXButton1Pressed;
            var forwardPressed = properties.IsXButton2Pressed;
            if (backPressed ^ forwardPressed)
            {
                args.Handled = true;
                if (backPressed) GoBackCommand.Execute(null);
                if (forwardPressed) GoForwardCommand.Execute(null);
            }
        }

        private void DispatcherOnAcceleratorKeyActivated(CoreDispatcher sender,
            AcceleratorKeyEventArgs args)
        {
            var virtualKey = args.VirtualKey;

            var acceptedEventTypes =
                new[] { CoreAcceleratorKeyEventType.SystemKeyDown, CoreAcceleratorKeyEventType.KeyDown };

            var acceptedVirtualKeys = new[] { (int)VirtualKey.Left, (int)VirtualKey.Right, 166, 167 };

            if (acceptedEventTypes.Contains(args.EventType) &&
                acceptedVirtualKeys.Contains((int)virtualKey))
            {
                var coreWindow = Window.Current.CoreWindow;
                var downState = CoreVirtualKeyStates.Down;
                var menuKey = (coreWindow.GetKeyState(VirtualKey.Menu) & downState) == downState;
                var controlKey = (coreWindow.GetKeyState(VirtualKey.Control) & downState) == downState;
                var shiftKey = (coreWindow.GetKeyState(VirtualKey.Shift) & downState) == downState;
                var noModifiers = !menuKey && !controlKey && !shiftKey;
                var onlyAlt = menuKey && !controlKey && !shiftKey;

                if (((int)virtualKey == 166 && noModifiers) ||
                    (virtualKey == VirtualKey.Left && onlyAlt))
                {
                    // When the next key or Alt+Left are pressed navigate back
                    args.Handled = true;
                    GoBackCommand.Execute(null);
                }
                else if (((int)virtualKey == 167 && noModifiers) ||
                         (virtualKey == VirtualKey.Right && onlyAlt))
                {
                    // When the next key or Alt+Right are pressed navigate forward
                    args.Handled = true;
                    GoForwardCommand.Execute(null);
                }
            }
        }
#endif

        public virtual void GoBack()
        {
            if (Frame != null && Frame.CanGoBack) Frame.GoBack();
        }

        public virtual bool CanGoBack()
        {
            return Frame != null && Frame.CanGoBack;
        }

        public virtual bool CanGoForward()
        {
            return Frame != null && Frame.CanGoForward;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.New)
                ViewModel = null;

            base.OnNavigatedTo(e);
        }

#if WINDOWS_PHONE_APP
        private void HardwareButtonsBackPressed(object sender, BackPressedEventArgs e)
        {
            if (GoBackCommand.CanExecute(null))
            {
                e.Handled = true;
                GoBackCommand.Execute(null);
            }
        }
#endif
    }
}
