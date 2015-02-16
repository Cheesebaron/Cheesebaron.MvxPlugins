using System;
using Xamarin.Forms;

namespace Cheesebaron.MvxPlugins.FormsPresenters.Core
{
    public class MvxFormsApp : Application
    {
        public event EventHandler Start;
        public event EventHandler Sleep;
        public event EventHandler Resume;

        protected override void OnStart()
        {
            var handler = Start;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        protected override void OnSleep()
        {
            var handler = Sleep;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        protected override void OnResume()
        {
            var handler = Resume;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }
    }
}
