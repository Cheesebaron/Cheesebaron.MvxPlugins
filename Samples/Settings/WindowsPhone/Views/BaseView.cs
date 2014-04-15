using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.WindowsPhone.Views;

namespace Settings.Sample.WindowsPhone.Views
{
    public abstract class BaseView<TViewModel>
        : MvxPhonePage where TViewModel : MvxViewModel
    {
        public new TViewModel ViewModel
        {
            get { return (TViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }
    }
}
