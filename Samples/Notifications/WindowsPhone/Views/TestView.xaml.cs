using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Notifications.Sample.Core.ViewModels;

namespace Notifications.Sample.WindowsPhone.Views
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