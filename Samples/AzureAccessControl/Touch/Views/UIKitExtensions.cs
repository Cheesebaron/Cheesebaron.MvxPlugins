using MonoTouch.ObjCRuntime;
using MonoTouch.UIKit;

namespace AzureAccessControl.Sample.Touch.Views
{
    public static class UIKitExtensions
    {
        public static void SetEdgesForExtendedLayout(
            this UIViewController viewController, UIRectEdge edgesForExtendedLayout = UIRectEdge.All)
        {
            if (viewController.RespondsToSelector(new Selector("setEdgesForExtendedLayout:")))
                viewController.EdgesForExtendedLayout = edgesForExtendedLayout;
        }
    }
}