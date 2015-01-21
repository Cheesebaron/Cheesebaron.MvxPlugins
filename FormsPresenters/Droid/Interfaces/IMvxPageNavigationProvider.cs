using Xamarin.Forms;

namespace Cheesebaron.MvxPlugins.FormsPresenters.Droid.Interfaces
{
    public interface IMvxPageNavigationProvider
    {
        void Push(Page page);
        void Pop();
    }
}