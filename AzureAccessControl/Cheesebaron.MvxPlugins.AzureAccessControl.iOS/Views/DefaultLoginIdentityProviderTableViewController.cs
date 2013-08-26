using System.Collections.Generic;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Cirrious.MvvmCross.Touch.Views;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Binding.Touch.Views;

namespace Cheesebaron.MvxPlugins.AzureAccessControl.iOS
{
    public class DefaultLoginIdentityProviderTableViewController 
        : MvxTableViewController
    {
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            Title = "Log In";

            var source = new TableSource(TableView);
            this.AddBindings(new Dictionary<object, string>
            {
                {source, "ItemsSource IdentityProviders; SelectionChangedCommand LoginSelectedIdentityProviderCommand"}
            });

            TableView.Source = source;
            TableView.ReloadData();
        }

        public class TableSource : MvxStandardTableViewSource
        {
            private static readonly NSString Identifier = new NSString("IdentityProviderCellIdentifier");
            private const string BindingText = "TitleText Name";

            public TableSource(UITableView tableView)
                : base(
                    tableView, UITableViewCellStyle.Default, Identifier, BindingText,
                    UITableViewCellAccessory.DisclosureIndicator)
            {
            }
        }
    }
}
