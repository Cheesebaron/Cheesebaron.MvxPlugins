//---------------------------------------------------------------------------------
// Copyright 2012 Tomasz Cielecki (tomasz@ostebaronen.dk)
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
using Android.Runtime;
using Android.Webkit;

namespace Cheesebaron.MvxPlugins.AzureAccessControl.Droid
{
    [Register("cheesebaron/mvxplugins/azureaccesscontrol/droid/AccessControlJavascriptNotify", DoNotGenerateAcw = true)]
    public abstract class ManagedAccessControlJavascriptNotify : Java.Lang.Object
    {
        static IntPtr class_ref = JNIEnv.FindClass("cheesebaron/mvxplugins/azureaccesscontrol/droid/AccessControlJavascriptNotify");

        protected ManagedAccessControlJavascriptNotify()
        {
        }

        protected ManagedAccessControlJavascriptNotify(IntPtr handle, JniHandleOwnership transfer)
            : base(handle, transfer)
        {
        }

        protected override Type ThresholdType
        {
            get { return typeof(ManagedAccessControlJavascriptNotify); }
        }

        protected override IntPtr ThresholdClass
        {
            get { return class_ref; }
        }

        #region Notify
        static IntPtr id_notify;

        [Register("notify", "(Ljava/lang/String;)V", "GetNotifyHandler")]
        public virtual void Notify(Java.Lang.String securityTokenResponse)
        {
            if (id_notify == IntPtr.Zero)
                id_notify = JNIEnv.GetMethodID(class_ref, "notify", "(Ljava/lang/String;)V");
            if (GetType() == ThresholdType)
                JNIEnv.CallVoidMethod(Handle, id_notify, new JValue[] { new JValue(securityTokenResponse) });
            else
                JNIEnv.CallNonvirtualObjectMethod(Handle, ThresholdClass, id_notify, new JValue[] { new JValue(securityTokenResponse) });
        }

#pragma warning disable 0169 //suppress warning about cb_notify not being used, it is!
        static Delegate cb_notify;
        static Delegate GetNotifyHandler()
        {
            if (cb_notify == null)
                cb_notify = JNINativeWrapper.CreateDelegate((Action<IntPtr, IntPtr, IntPtr>)n_Notify);
            return cb_notify;
        }

        static void n_Notify(IntPtr jnienv, IntPtr lrefThis, IntPtr native_securityTokenResponse)
        {
            ManagedAccessControlJavascriptNotify __this = Java.Lang.Object.GetObject<ManagedAccessControlJavascriptNotify>(lrefThis, JniHandleOwnership.DoNotTransfer);
            Java.Lang.String securityTokenResponse = Java.Lang.Object.GetObject<Java.Lang.String>(native_securityTokenResponse, JniHandleOwnership.DoNotTransfer);
            __this.Notify(securityTokenResponse);
        }
#pragma warning restore 0169
        #endregion
    }
}