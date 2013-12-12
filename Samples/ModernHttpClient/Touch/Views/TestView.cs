using Cirrious.FluentLayouts.Touch;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Touch.Views;
using Core.ViewModels;
using MonoTouch.UIKit;

namespace Touch.Views
{
    public class TestView : MvxViewController
    {
        private UILabel _urlLabel;
        private UITextField _url;
        private UILabel _timeLabel;
        private UILabel _time;
        private UILabel _rawLabel;
        private UITextField _raw;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View.BackgroundColor = UIColor.White;
            Title = "Sample";

            _urlLabel = new UILabel{Text = "Url:"};
            _url = new UITextField
            {
                Placeholder = "Enter Url",
                ShouldReturn = ShouldReturn,
                KeyboardType = UIKeyboardType.Url,
                ClearButtonMode = UITextFieldViewMode.Always,
                ReturnKeyType = UIReturnKeyType.Default,
                ShouldClear = field => true,
                AutocapitalizationType = UITextAutocapitalizationType.None,
            };
            _timeLabel = new UILabel{Text = "Time:"};
            _time = new UILabel();
            _rawLabel = new UILabel{Text = "Page source:"};
            _raw = new UITextField
            {
                Enabled = false,
                ClipsToBounds = true,
            };

            var bSet = this.CreateBindingSet<TestView, TestViewModel>();
            bSet.Bind(_url).To(vm => vm.Url).TwoWay();
            bSet.Bind(_time).To(vm => vm.Time);
            bSet.Bind(_raw).To(vm => vm.Raw);
            bSet.Apply();

            Add(_urlLabel);
            Add(_url);
            Add(_timeLabel);
            Add(_time);
            Add(_rawLabel);
            Add(_raw);

            View.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

            var download = new UIBarButtonItem {Title = "Download"};
            download.Clicked += (s, e) => ViewModel.DownloadCommand.Execute(null);

            var cancel = new UIBarButtonItem { Title = "Cancel" };
            cancel.Clicked += (s, e) => ViewModel.CancelCommand.Execute(null);

            NavigationItem.RightBarButtonItems = new[] {download, cancel};
        }

        private bool ShouldReturn(UITextField textfield)
        {
            _url.ResignFirstResponder();
            return true;
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();

            float yTop = 0;
            var yBottom = View.Frame.Bottom;

            if (UIDevice.CurrentDevice.CheckSystemVersion(7, 0))
            {
                yTop = TopLayoutGuide.Length;
                yBottom = BottomLayoutGuide.Length;
            }

            const int margin = 10;
            View.AddConstraints(
                _urlLabel.AtLeftOf(View, margin),
                _urlLabel.AtTopOf(View, yTop + margin),
                _urlLabel.AtRightOf(View, margin),

                _url.WithSameLeft(_urlLabel),
                _url.WithSameRight(_urlLabel),
                _url.Below(_urlLabel, margin),

                _timeLabel.WithSameLeft(_url),
                _timeLabel.WithSameRight(_url),
                _timeLabel.Below(_url, margin),

                _time.WithSameLeft(_timeLabel),
                _time.WithSameRight(_timeLabel),
                _time.Below(_timeLabel, margin),

                _rawLabel.WithSameLeft(_time),
                _rawLabel.WithSameRight(_time),
                _rawLabel.Below(_time, margin),

                _raw.WithSameLeft(_rawLabel),
                _raw.WithSameRight(_rawLabel),
                _raw.Below(_rawLabel, margin)
                );

            View.LayoutSubviews();
        }

        public new TestViewModel ViewModel
        {
            get { return (TestViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }
    }
}