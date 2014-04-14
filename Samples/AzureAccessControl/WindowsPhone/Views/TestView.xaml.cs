using AzureAccessControl.Sample.ViewModels;

namespace AzureAccessControl.Sample.WindowsPhone.Views
{
    public partial class TestView : BaseTestView
    {
        public TestView()
        {
            InitializeComponent();
        }
    }

    public abstract class BaseTestView : BaseView<TestViewModel>
    {
    }
}