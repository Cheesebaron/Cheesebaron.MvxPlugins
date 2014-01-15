using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;

namespace Cheesebaron.MvxPlugins.AzureAccessControl.ViewModels
{
    public class DefaultIdentityProviderViewModel 
        : MvxViewModel
    {
        private readonly IdentityProviderInformation _model;
        private DefaultIdentityProviderCollectionViewModel _parent;

        public DefaultIdentityProviderViewModel(IdentityProviderInformation model)
        {
            _model = model;
        }

        public string Name { get { return _model.Name; } }

        public string LoginUrl { get { return _model.LoginUrl; }}

        public string LogoutUrl { get { return _model.LogoutUrl; } }

        public DefaultIdentityProviderCollectionViewModel Parent
        {
            get { return _parent; }
            set
            {
                _parent = value;
                RaisePropertyChanged("Parent");
            }
        }

        public ICommand LoginIdentityProviderCommand
        {
            get { return new MvxCommand(() => Parent.LoginSelectedIdentityProviderCommand.Execute(this)); }
        }
    }
}
