// MvxFormsApp.cs
// 2015 (c) Copyright Cheesebaron. http://ostebaronen.dk
// Cheesebaron.MvxPlugins.FormsPresenters is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Tomasz Cielecki, @cheesebaron, mvxplugins@ostebaronen.dk
// Contributor - Marcos Cobeña Morián, @CobenaMarcos, marcoscm@me.com
﻿
﻿using System;
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
