using Cheesebaron.MvxPlugins.AzureAccessControl;
using Cheesebaron.MvxPlugins.AzureAccessControl.ViewModels;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;

namespace AzureAccessControl.Sample
{
    public abstract class BaseApp
        : MvxApplication
    {
        protected BaseApp()
        {
            InitializePlugins();
        }

        private void InitializePlugins()
        {
            Cheesebaron.MvxPlugins.AzureAccessControl.PluginLoader.Instance.EnsureLoaded();
        }
    }

    public class App
        : BaseApp
    {
        public override void Initialize()
        {
            var canResolve = Mvx.CanResolve<ISimpleWebTokenStore>();

            RegisterAppStart<DefaultIdentityProviderCollectionViewModel>();
        }
    }
}
