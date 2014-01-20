//---------------------------------------------------------------------------------
// Copyright 2014 Tomasz Cielecki (tomasz@ostebaronen.dk)
// Licensed under the Apache License, Version 2.0 (the "License"); 
// You may not use this file except in compliance with the License. 
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0 

// THIS CODE IS PROVIDED *AS IS* BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, EITHER EXPRESS OR IMPLIED, 
// INCLUDING WITHOUT LIMITATION ANY IMPLIED WARRANTIES OR 
// CONDITIONS OF TITLE, FITNESS FOR A PARTICULAR PURPOSE, 
// MERCHANTABLITY OR NON-INFRINGEMENT. 

// See the Apache 2 License for the specific language governing 
// permissions and limitations under the License.
//---------------------------------------------------------------------------------

using System;
using Android.App;
using Android.Content;
using Android.OS;

namespace CheeseyDroidExtensions
{
    public class BroadcastKillableActivity : Activity
    {
        private string _intentFilterName = "cheeseydroidextension.KillEmAll";
        public string IntentFilterName
        {
            get { return _intentFilterName; }
            set { _intentFilterName = value; }
        }

        protected KillBroadcastReceiver Receiver;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            AddReceiver();
        }

        public override void FinishActivity(int requestCode)
        {
            RemoveReceiver();
            base.FinishActivity(requestCode);
        }

        public override void Finish()
        {
            RemoveReceiver();
            base.Finish();
        }

        private void AddReceiver()
        {
            var filter = new IntentFilter();
            filter.AddAction(IntentFilterName);
            Receiver = new KillBroadcastReceiver { OnReceiveAction = () => RunOnUiThread(Finish) };
            RegisterReceiver(Receiver, filter);  
        }

        private void RemoveReceiver()
        {
            try
            {
                if (Receiver != null)
                    UnregisterReceiver(Receiver);
            }
            catch (Java.Lang.IllegalArgumentException) { }
        }

        public class KillBroadcastReceiver : BroadcastReceiver
        {
            public Action OnReceiveAction;
            public override void OnReceive(Context context, Intent intent)
            {
                if (OnReceiveAction != null) OnReceiveAction.Invoke();
            }
        }
    }
}